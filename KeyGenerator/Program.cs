using System;
using System.Security.Cryptography;

public class KeyGenerator
{
    public static void GenerateKeyAndIV(out byte[] key, out byte[] iv)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            key = new byte[16];
            iv = new byte[16];
            rng.GetBytes(key);
            rng.GetBytes(iv);
        }
    }
}

public class Program
{
    public static void Main()
    {
        byte[] key, iv;
        KeyGenerator.GenerateKeyAndIV(out key, out iv);

        Console.WriteLine("Key: " + BitConverter.ToString(key));
        Console.WriteLine("IV: " + BitConverter.ToString(iv));
    }
}
