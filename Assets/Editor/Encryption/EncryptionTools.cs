using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Tools
{
    public class EncryptionTools
    {
        public static string EncryptionFileBySHA256(string filePath)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                using (FileStream fileStream = File.OpenRead(filePath))
                {
                    byte[] hash = sha256.ComputeHash(fileStream);
                    StringBuilder stringBuilder = new StringBuilder();

                    foreach (byte b in hash)
                    {
                        stringBuilder.Append(b.ToString("X2"));
                    }

                    return stringBuilder.ToString();
                }
            }
        }
    }
}
