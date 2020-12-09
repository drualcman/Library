﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

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
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        /// <summary>
        /// Generar MD5
        /// </summary>
        /// <param name="number">Numero a convertir</param>
        /// <returns></returns>
        public static string GetMD5(int number)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            numeros n = new numeros();
            string str = n.number2String(number);
            n = null;
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        /// <summary>
        /// Generar MD5
        /// </summary>
        /// <param name="number">Numero a convertir</param>
        /// <returns></returns>
        public static string GetMD5(long number)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            numeros n = new numeros();
            string str = n.number2String(number);
            n = null;
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        /// <summary>
        /// Generar MD5
        /// </summary>
        /// <param name="number">Numero a convertir</param>
        /// <returns></returns>
        public static string GetMD5(double number)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            numeros n = new numeros();
            string str = n.number2String(number);
            n = null;
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        /// <summary>
        /// Generar MD5
        /// </summary>
        /// <param name="number">Numero a convertir</param>
        /// <returns></returns>
        public static string GetMD5(decimal number)
        {
            MD5 md5 = MD5CryptoServiceProvider.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            numeros n = new numeros();
            string str = n.number2String(number);
            n = null;
            stream = md5.ComputeHash(encoding.GetBytes(str));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }

        public string Encrypt(string clearText, string EncryptionKey)
        {
            if (string.IsNullOrEmpty(EncryptionKey) || string.IsNullOrWhiteSpace(EncryptionKey)) EncryptionKey = "!@#$%^&*()";
            //EncryptionKey = Encriptacion.GetMD5(EncryptionKey);
            byte[] clearBytes = ASCIIEncoding.Unicode.GetBytes(clearText);

            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
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
                if (string.IsNullOrEmpty(EncryptionKey) || string.IsNullOrWhiteSpace(EncryptionKey)) EncryptionKey = "!@#$%^&*()";
                //EncryptionKey = Encriptacion.GetMD5(EncryptionKey);
                cipherText = cipherText.Replace(" ", "+").Replace("~", "/");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        cipherText = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
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
