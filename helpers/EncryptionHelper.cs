using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Cryptography;


namespace ChatNova.Helpers
{
    /// <summary>
    /// AES-256 encryption for message text
    /// Encrypt before INSERT — Decrypt after SELECT
    /// </summary>
    public static class EncryptionHelper
    {
        // MUST be exactly 32 characters (256 bits)
        private static readonly string Key = "ChatNova_AES_Key_32Chars_Here!!!";

        // MUST be exactly 16 characters (128 bits)
        private static readonly string IV = "ChatNova_IV_16Ch";

        /// <summary>
        /// Encrypt message before saving to DB
        /// </summary>
        public static string Encrypt(string plainText)
        {
            if (string.IsNullOrWhiteSpace(plainText)) return plainText;

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(Key);
                aes.IV = Encoding.UTF8.GetBytes(IV);
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor();
                byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(
                                                inputBytes, 0, inputBytes.Length);

                return Convert.ToBase64String(encryptedBytes);
            }
        }

        /// <summary>
        /// Decrypt message after loading from DB
        /// </summary>
        public static string Decrypt(string encryptedText)
        {
            if (string.IsNullOrWhiteSpace(encryptedText)) return encryptedText;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(Key);
                    aes.IV = Encoding.UTF8.GetBytes(IV);
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    ICryptoTransform decryptor = aes.CreateDecryptor();
                    byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
                    byte[] decryptedBytes = decryptor.TransformFinalBlock(
                                                    encryptedBytes, 0, encryptedBytes.Length);

                    return Encoding.UTF8.GetString(decryptedBytes);
                }
            }
            catch
            {
                return "[Decryption Failed]";
            }
        }
    }
}