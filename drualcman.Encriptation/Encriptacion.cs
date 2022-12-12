using System.Security.Cryptography;
using System.Text;

namespace drualcman
{
    public class Encriptacion : Hash
    {
        /// <summary>
        /// Generar MD5
        /// </summary>
        /// <param name="str">String a convertir</param>
        /// <returns></returns>
        public static string GetMD5(string str)
        {
            return MD5(str);
        }

        /// <summary>
        /// Generar MD5
        /// </summary>
        /// <param name="number">Numero a convertir</param>
        /// <returns></returns>
        public static string GetMD5(int number)
        {
            return MD5(number);
        }

        /// <summary>
        /// Generar MD5
        /// </summary>
        /// <param name="number">Numero a convertir</param>
        /// <returns></returns>
        public static string GetMD5(long number)
        {
            return MD5(number);
        }

        /// <summary>
        /// Generar MD5
        /// </summary>
        /// <param name="number">Numero a convertir</param>
        /// <returns></returns>
        public static string GetMD5(double number)
        {
            return MD5(number);
        }

        /// <summary>
        /// Generar MD5
        /// </summary>
        /// <param name="number">Numero a convertir</param>
        /// <returns></returns>
        public static string GetMD5(decimal number)
        {
            return MD5(number);
        }

        public string Encrypt(string clearText, string EncryptionKey)
        {
            if(string.IsNullOrEmpty(EncryptionKey) || string.IsNullOrWhiteSpace(EncryptionKey)) EncryptionKey = "!@#$%^&*()";
            //EncryptionKey = Encriptacion.GetMD5(EncryptionKey);
            byte[] clearBytes = ASCIIEncoding.Unicode.GetBytes(clearText);

            using(Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }, 69);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using(MemoryStream ms = new MemoryStream())
                {
                    using(CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            clearText = clearText.Replace("+", " ").Replace("/", "~");
            return clearText;
        }

        public string Decrypt(string cipherText, string EncryptionKey)
        {
            try
            {
                if(string.IsNullOrEmpty(EncryptionKey) || string.IsNullOrWhiteSpace(EncryptionKey)) EncryptionKey = "!@#$%^&*()";
                //EncryptionKey = Encriptacion.GetMD5(EncryptionKey);
                cipherText = cipherText.Replace(" ", "+").Replace("~", "/");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using(Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 }, 69);
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using(MemoryStream ms = new MemoryStream())
                    {
                        using(CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch(Exception ex)
            {
                cipherText = ex.ToString();
            }

            return cipherText;
        }

        public string Decrypt(string stringToDecrypt)
        {
            return Hash.Base64Decode(stringToDecrypt);
        }

        public string Encrypt(string stringToEncrypt)
        {
            return Hash.Base64Encode(stringToEncrypt);
        }
    }
}
