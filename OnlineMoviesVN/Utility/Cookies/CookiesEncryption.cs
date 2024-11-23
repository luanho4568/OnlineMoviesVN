using System.Security.Cryptography;
using System.Text;

namespace OnlineMoviesVN.Utils
{
    public class CookiesEncryption
    {
        private static readonly byte[] keyBytes = Encoding.UTF8.GetBytes("4619E97E56E34E5A");
        private static readonly byte[] iv = Encoding.UTF8.GetBytes("BA11F6FB70424C3F");

        public static string Decrypt(string cipherText)
        {
            var encrypted = Convert.FromBase64String(cipherText);
            var decrypted = DecryptStringFromBytes(encrypted, keyBytes, iv);
            return decrypted;
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText.Length <= 0 || key.Length <= 0 || iv.Length <= 0)
            {
                goto CAN_NOT_CRYPT;
            }

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                try
                {
                    // Create the streams used for decryption.
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                    goto CAN_NOT_CRYPT;
                }
            }
            return plaintext;
        CAN_NOT_CRYPT:
            return "";
        }

        public static string Encrypt(string cipherText)
        {
            var encrypted = EncryptStringToBytes(cipherText, keyBytes, iv);
            // return Encoding.UTF8.GetString(encrypted);
            return encrypted;
        }
        private static string EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (string.IsNullOrEmpty(plainText) || key.Length <= 0 || iv.Length <= 0)
            {
                goto CAN_NOT_CRYPT;
            }

            byte[] encrypted;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);
                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);
        CAN_NOT_CRYPT:
            return "";
        }
    }
}