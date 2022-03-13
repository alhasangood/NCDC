using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class Security
    {
        public static string version = "2.0.0";
        public static string MasterKey = Base64Decode("VXo0U2pXVkJCRENpM1lFdTZkekhaQ1ZzSk5QdkRENE0=");
        public static string Salt = Base64Decode("QzByZVRlY0AyMDIwJGVjdXJpdHk=");

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string EncryptString(string text, string keyString)
        {
            RijndaelManaged aes256 = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.ECB,
                Key = Encoding.ASCII.GetBytes(keyString)
            };
            aes256.GenerateIV();

            ICryptoTransform encryptor = aes256.CreateEncryptor();
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            StreamWriter mSWriter = new StreamWriter(cs);
            mSWriter.Write(text);
            mSWriter.Flush();
            cs.FlushFinalBlock();
            byte[] cypherTextBytes = ms.ToArray();
            ms.Close();
            return Convert.ToBase64String(cypherTextBytes);
        }

        public static string DecryptString(string cipherText, string keyString)
        {
            RijndaelManaged aes256 = new RijndaelManaged
            {
                KeySize = 256,
                BlockSize = 128,
                Padding = PaddingMode.Zeros,
                Mode = CipherMode.ECB,
                Key = Encoding.ASCII.GetBytes(keyString)
            };
            aes256.GenerateIV();
            byte[] encryptedData = Convert.FromBase64String(cipherText);
            ICryptoTransform transform = aes256.CreateDecryptor();
            byte[] plainText = transform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            return Encoding.UTF8.GetString(plainText).Replace("\0", "");
        }

        public static string GenerateToken()
        {
            var TDES = new TripleDESCryptoServiceProvider();

            TDES.GenerateKey();

            return Convert.ToBase64String(TDES.Key).Replace("+", "-").Replace("/", "_");
        }

        public static string ComputeHash(string password, string salt, int iteration = 50)
        {
            string hash = password;

            for (int i = 0; i < iteration; i++)
            {
                hash = CreateSHAHash(hash, salt);
            }

            return hash;
        }

        private static string CreateSHAHash(string password, string salt)
        {
            SHA512Managed HashTool = new SHA512Managed();
            byte[] PasswordAsByte = Encoding.UTF8.GetBytes(string.Concat(password, salt));
            byte[] EncryptedBytes = HashTool.ComputeHash(PasswordAsByte);
            HashTool.Clear();
            return Convert.ToBase64String(EncryptedBytes);
        }
    }
}
