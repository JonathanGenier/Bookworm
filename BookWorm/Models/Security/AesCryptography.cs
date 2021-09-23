using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookWorm.Models.Security
{
    public class AesCryptography
    {
        public enum Source { Web, Database };

        private static readonly byte[] webKey = Encoding.UTF8.GetBytes("5v8y/B?E(H+MbQeT");
        private static readonly byte[] webIv = Encoding.UTF8.GetBytes("bQeThWmZq4t7w9z$");
        private static readonly byte[] databaseKey = Encoding.UTF8.GetBytes("$C&E)H@McQfTjWnZ");
        private static readonly byte[] databaseIv = Encoding.UTF8.GetBytes("Xp2s5v8y/B?E(H+M$");

        public static string DecryptAES(string cipherText, Source source)
        {
            byte[] keybytes = (source == Source.Web ? webKey : databaseKey);
            byte[] iv = (source == Source.Web ? webIv : databaseIv);

            byte[] encrypted = Convert.FromBase64String(cipherText);
            string decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }
        
        /*
        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // On verifie si les parametres ne sont pas null ou vide.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
           
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");

            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("key");

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

                // Create le streams pour l'encryption.  
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Ecrit les donnees dans le stream.  
                            swEncrypt.Write(plainText);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.  
            return encrypted;
        }
        */
        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // On verifie si les parametres ne sont pas null ou vide.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");

            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");

            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("key");

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
                                // Read the decrypted bytes from the decrypting stream and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }
    }
}
