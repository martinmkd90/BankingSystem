using Microsoft.EntityFrameworkCore;
using Banking.Data.Context;
using Banking.Domain.Models;
using Banking.Services.Interfaces;
using Banking.Services.Services;
using Banking.API.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace BankingSyst.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add DbContext
            services.AddDbContext<BankingDbContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("BankingDbConnection"),
                b => b.MigrationsAssembly("Banking.API")
            ));

            services.Configure<AlphaVantageSettings>(Configuration.GetSection("AlphaVantage"));
            services.Configure<ExchangeApiSettings>(Configuration.GetSection("ExchangeApiSettings"));
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            services.Configure<TokenSettings>(Configuration.GetSection("TokenSettings"));

            services.AddTransient<ILoanCalculatorService, LoanCalculatorService>();
            services.AddScoped<ILoanEligibilityService, LoanEligibilityService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddScoped<IUserService, UserService>();



            services.AddControllers();
            services.AddMemoryCache();
            services.AddHttpClient();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["JwtSettings:Issuer"],
                ValidAudience = Configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtSettings:SecretKey"] ?? ""))
            };
        });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSerilogRequestLogging();
            app.UseCors("AllowAllOrigins");

            app.UseRouting();

            app.UseAuthentication(); // Ensure this is placed before UseAuthorization
            app.UseAuthorization();
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
