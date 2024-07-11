using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace UnityMagicNet.Core
{
    public static class SecurityUtils
    {
        private static readonly string EncryptionKey = "your-encryption-key";

        public static byte[] Compress(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            using (var outputStream = new MemoryStream())
            {
                using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                {
                    gzipStream.Write(bytes, 0, bytes.Length);
                }
                return outputStream.ToArray();
            }
        }

        public static string Decompress(byte[] data)
        {
            using (var inputStream = new MemoryStream(data))
            {
                using (var outputStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        gzipStream.CopyTo(outputStream);
                    }
                    return Encoding.UTF8.GetString(outputStream.ToArray());
                }
            }
        }

        public static string Encrypt(string plainText)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            using (var aes = Aes.Create())
            {
                var key = new Rfc2898DeriveBytes(EncryptionKey, Encoding.UTF8.GetBytes("SaltIsGoodForYou"));
                aes.Key = key.GetBytes(32);  // 256 bits
                aes.IV = key.GetBytes(16);   // 128 bits

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                    }
                    return Convert.ToBase64String(memoryStream.ToArray());
                }
            }
        }

        public static string Decrypt(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            using (var aes = Aes.Create())
            {
                var key = new Rfc2898DeriveBytes(EncryptionKey, Encoding.UTF8.GetBytes("SaltIsGoodForYou"));
                aes.Key = key.GetBytes(32);  // 256 bits
                aes.IV = key.GetBytes(16);   // 128 bits

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var memoryStream = new MemoryStream(encryptedBytes))
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    byte[] decryptedBytes = new byte[encryptedBytes.Length];
                    int bytesRead = cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                    return Encoding.UTF8.GetString(decryptedBytes, 0, bytesRead);
                }
            }
        }

        public static string GenerateToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}