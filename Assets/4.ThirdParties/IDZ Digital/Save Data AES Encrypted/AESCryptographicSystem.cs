// ReSharper disable InconsistentNaming

namespace IDZ_Digital.Extensions
{
    using System;
    using System.Text;
    using System.IO;
    using System.Security.Cryptography;

    public static class AESCryptographicSystem
    {
        private const string _PASSWORD = "DFg%KzaUpf@k#H*FaJ8s";
        private static readonly byte[] _SALT = new byte[] { 0x52, 0x41, 0x16, 0x79, 0x86, 0x64, 0x97, 0x22 };

        /// <summary>
        /// Encrypts input byte array with AES and returns it.
        /// </summary>
        /// <param name="input">Byte array to be encrypted.</param>
        /// <param name="password">Password for encryption. If not set [NOT RECOMMENDED], default 20 chars Password will be used.</param>
        /// <param name="salt">If not set [NOT RECOMMENDED], default 8 byte SALT will be used.</param>
        /// <returns>Encrypted byte array.</returns>
        /// <remarks>Recommendation: Provide password and SALT while encrypting and keep it safe. Failure to store password and SALT correctly/securely would result in loss of data or data getting compromised.</remarks>
        public static byte[] Encrypt(byte[] input, string password = null, byte[] salt = null)
        {
            password ??= _PASSWORD;
            salt ??= _SALT;

            var pdb = new Rfc2898DeriveBytes(password, salt);

            var ms = new MemoryStream();
            var aes = Aes.Create();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);

            var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.Close();

            return ms.ToArray();
        }
        /// <summary>
        /// Encrypts input string with AES and returns it.
        /// </summary>
        /// <param name="input">String to be encrypted.</param>
        /// <param name="password">Password for encryption. If not set [NOT RECOMMENDED], default 20 chars Password will be used.</param>
        /// <param name="salt">If not set [NOT RECOMMENDED], default 8 byte SALT will be used.</param>
        /// <returns>Encrypted string.</returns>
        /// <remarks>Recommendation: Provide password and SALT while encrypting and keep it safe. Failure to store password and SALT correctly/securely would result in loss of data or data getting compromised.</remarks>
        public static string Encrypt(string input, string password = null, byte[] salt = null)
        {
            password ??= _PASSWORD;
            salt ??= _SALT;

            return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(input), password, salt));
        }
        /// <summary>
        /// Decrypts input byte array and returns it.
        /// </summary>
        /// <param name="input">Byte array to be decrypted.</param>
        /// <param name="password">Password that was used for encryption.</param>
        /// <param name="salt">Salt that was used for encryption</param>
        /// <returns>Decrypted byte array.</returns>
        public static byte[] Decrypt(byte[] input, string password = null, byte[] salt = null)
        {
            password ??= _PASSWORD;
            salt ??= _SALT;


            var pdb = new Rfc2898DeriveBytes(password, salt);
            var ms = new MemoryStream();
            var aes = Aes.Create();
            aes.Key = pdb.GetBytes(aes.KeySize / 8);
            aes.IV = pdb.GetBytes(aes.BlockSize / 8);
            var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(input, 0, input.Length);
            cs.Close();
            return ms.ToArray();
        }

        /// <summary>
        /// Decrypts input string and returns it.
        /// </summary>
        /// <param name="input">String to be decrypted.</param>
        /// <param name="password">Password that was used for encryption.</param>
        /// <param name="salt">Salt that was used for encryption</param>
        /// <returns>Decrypted string.</returns>
        public static string Decrypt(string input, string password = null, byte[] salt = null)
        {
            password ??= _PASSWORD;
            salt ??= _SALT;

            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(input), password, salt));
        }
    }
}