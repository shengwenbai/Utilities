using System;

namespace Utilities
{
    class Program
    {
        static void Main(string[] args)
        {
            var encryptHelper = new EncryptHelper
            {
                Iv = "pianyiliang",
                Key = "abcdefgh"
            };
            var text = "矿山救护队卡的埃克设计的！@@";
            Console.WriteLine($"Text to encrypt:{text}");
            var encrypted = encryptHelper.Encrypt(text);
            Console.WriteLine($"Encrypted: {encrypted}");
            Console.WriteLine($"Decrypted: {encryptHelper.Decrypt(encrypted)}");
            encryptHelper.EncryptFile(@"e:\origin.txt", @"e:\encrypted.txt");
            encryptHelper.DecryptFile(@"e:\encrypted.txt", @"e:\decrypted.txt");


        }
    }
}
