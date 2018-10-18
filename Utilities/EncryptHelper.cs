using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Utilities
{
    public class EncryptHelper
    {
        private string iv = "LengthGreaterThan8";
        private string key = "equalTo8";

        public string Iv
        {
            get => iv;
            set => iv = value;
        }
        public string Key
        {
            get => key;
            set => key = value;
        }

        public string Encrypt(string sourceString)
        {
            byte[] btKey = Encoding.Default.GetBytes(key);
            byte[] btIv = Encoding.Default.GetBytes(iv);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Encoding.Default.GetBytes(sourceString);
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIv),
                    CryptoStreamMode.Write))
                {
                    cs.Write(inData, 0, inData.Length);
                    cs.FlushFinalBlock();
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public string Decrypt(string encryptedString)
        {
            byte[] btKey = Encoding.Default.GetBytes(key);
            byte[] btIv = Encoding.Default.GetBytes(iv);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] inData = Convert.FromBase64String(encryptedString);
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIv),
                    CryptoStreamMode.Write))
                {
                    cs.Write(inData, 0, inData.Length);
                    cs.FlushFinalBlock();
                }

                return Encoding.Default.GetString(ms.ToArray());
            }
        }

        public void EncryptFile(string sourceFile, string destFile)
        {
            if(!File.Exists(sourceFile)) throw new FileNotFoundException("File cannot be found in this directory!", sourceFile);
            byte[] btKey = Encoding.Default.GetBytes(key);
            byte[] btIv = Encoding.Default.GetBytes(iv);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] btFile = File.ReadAllBytes(sourceFile);
            using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    using (CryptoStream cs = new CryptoStream(fs, des.CreateEncryptor(btKey, btIv),
                        CryptoStreamMode.Write))
                    {
                        cs.Write(btFile, 0, btFile.Length);
                        cs.FlushFinalBlock();
                    }
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        public void EncryptFile(string sourceFile)
        {
            EncryptFile(sourceFile,sourceFile);
        }

        public void DecryptFile(string sourceFile, string destFile)
        {
            if(!File.Exists(sourceFile)) throw new FileNotFoundException("File cannot be found in this directory!", sourceFile);
            byte[] btKey = Encoding.Default.GetBytes(key);
            byte[] btIv = Encoding.Default.GetBytes(iv);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] btFile = File.ReadAllBytes(sourceFile);
            using (FileStream fs = new FileStream(destFile, FileMode.Create, FileAccess.Write))
            {
                try
                {
                    using (CryptoStream cs = new CryptoStream(fs, des.CreateEncryptor(btKey, btIv),
                        CryptoStreamMode.Write))
                    {
                        cs.Write(btFile, 0, btFile.Length);
                        cs.FlushFinalBlock();
                    }
                }
                finally
                {
                    fs.Close();
                }
            }
        }

        public void DecryptFile(string sourceFile)
        {
            DecryptFile(sourceFile, sourceFile);
        }

    }
}