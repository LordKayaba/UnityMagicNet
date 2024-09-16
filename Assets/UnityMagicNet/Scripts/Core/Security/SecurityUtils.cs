using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace UnityMagicNet.Core
{
    public static class SecurityUtils
    {
        private static readonly string EncryptionKey = "your-encryption-key";

        public static async Task<byte[]> Compress(string data)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            return await Task.Run(() =>
            {
                using (var outputStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    {
                        gzipStream.Write(bytes, 0, bytes.Length);
                    }
                    return outputStream.ToArray();
                }
            });
        }

        public static async Task<string> Decompress(byte[] data)
        {
            return await Task.Run(() =>
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
            });
        }

        public static async Task<string> Encrypt(string plainText)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            return await Task.Run(() =>
            {
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
            });
        }

        public static async Task<string> Decrypt(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            return await Task.Run(() =>
            {
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
            });
        }

        public static string GenerateToken()
        {
            return Guid.NewGuid().ToString();
        }

        public static async Task<string> HashPasswordAsync(string password)
        {
            return await Task.Run(() =>
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            });
        }

        public static async Task<bool> VerifyPassword(string enteredPassword, string storedHash)
        {
            string enteredHash = await HashPasswordAsync(enteredPassword);
            return enteredHash.Equals(storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}
