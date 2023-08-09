using Banking.Data.Context;
using Banking.Domain.Models;
using Banking.Services.DTOs;
using Banking.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Banking.Services.Services
{
    public class AccountAggregatorService : IAccountAggregatorService
    {
        private readonly BankingDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AccountAggregatorService(BankingDbContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IEnumerable<ExternalAccount>> GetAllExternalAccountsForUser(int userId)
        {
            return await _context.ExternalAccounts.Where(ea => ea.UserId == userId).ToListAsync();
        }

        public async Task LinkExternalAccount(int userId, AccountLinkingRequest request)
        {
            // Here, we would initiate the OAuth2 flow with the third-party aggregator.
            // For simplicity, let's assume we've obtained the access and refresh tokens.

            var encryptedAccessToken = Encrypt(request.AccessToken);
            var encryptedRefreshToken = Encrypt(request.RefreshToken);

            var externalAccount = new ExternalAccount
            {
                UserId = userId,
                BankName = request.BankName,
                AccountNumber = request.AccountNumber,
                AccessToken = encryptedAccessToken,
                RefreshToken = encryptedRefreshToken,
                LastUpdated = DateTime.UtcNow
            };

            _context.ExternalAccounts.Add(externalAccount);
            await _context.SaveChangesAsync();
        }

        public async Task RefreshAccountData(int userId, int externalAccountId)
        {
            var externalAccount = await _context.ExternalAccounts.FindAsync(externalAccountId);
            if (externalAccount == null || externalAccount.UserId != userId)
                throw new InvalidOperationException("Account not found or unauthorized.");

            var decryptedAccessToken = Decrypt(externalAccount.AccessToken);

            // Use the decrypted access token to fetch the latest account data from the third-party aggregator.
            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.GetStringAsync($"{_configuration["AggregatorEndpoint"]}?accessToken={decryptedAccessToken}");
            var updatedData = JsonConvert.DeserializeObject<ExternalAccountData>(response);

            externalAccount.Balance = updatedData.Balance;
            externalAccount.LastUpdated = DateTime.UtcNow;

            _context.Update(externalAccount);
            await _context.SaveChangesAsync();
        }

        private static readonly byte[][] keyAndIV = GenerateKeyAndIV();
        private static readonly byte[] Key = keyAndIV[0];
        private static readonly byte[] IV = keyAndIV[1];

        private static string Encrypt(string data)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);
            using (StreamWriter sw = new(cs))
            {
                sw.Write(data);
            }
            return Convert.ToBase64String(ms.ToArray());
        }

        public static byte[][] GenerateKeyAndIV()
        {
            byte[] key, iv;
            using (var rng = RandomNumberGenerator.Create())
            {
                key = new byte[16];
                iv = new byte[16];
                rng.GetBytes(key);
                rng.GetBytes(iv);
            }
            return new byte[][] { key, iv };
        }
        private static string Decrypt(string encryptedData)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using MemoryStream ms = new(Convert.FromBase64String(encryptedData));
            using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
            using StreamReader sr = new(cs);
            return sr.ReadToEnd();
        }
    }
}
