using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Editor.Encryption
{
    public class EncryptionTools
    {
        /// <summary>
        /// 获取SHA256值来验证文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string EncryptionFileBySHA256(string filePath)
        {
            using SHA256     sha256     = SHA256.Create();
            using FileStream fileStream = File.OpenRead(filePath);

            byte[]        hash          = sha256.ComputeHash(fileStream);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.Append(b.ToString("X2"));
            }

            return stringBuilder.ToString();
        }
    }
}