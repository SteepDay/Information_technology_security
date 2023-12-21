using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace lab_3_client
{
    // Класс для работы с шифрованием
    internal static class EncryptionHandler
    {
        private static readonly string EncryptKey = "1eNa1Zx9KKTqQteM4Mj1G7M9kr0upqY2";
        private static readonly string EncryptIV = "En3wn0vh4JAKdNwZ";

        // Метод для создания потока для шифрования
        public static CryptoStream EncryptStream(Stream responseStream)
        {
            Aes aes = GetEncryptionAlgorithm();
            ToBase64Transform base64Transform = new ToBase64Transform();
            CryptoStream base64EncodedStream = new CryptoStream(responseStream, base64Transform, CryptoStreamMode.Write);
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            CryptoStream cryptoStream = new CryptoStream(base64EncodedStream, encryptor, CryptoStreamMode.Write);
            return cryptoStream;
        }

        // Метод для шифрования массива байтов
        public static byte[] EncryptByteArray(byte[] data)
        {
            using (Aes aes = GetEncryptionAlgorithm())
            {
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var encryptedStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(encryptedStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(data, 0, data.Length);
                    }
                    return encryptedStream.ToArray();
                }
            }
        }

        // Метод для дешифрования потока
        public static Stream DecryptStream(Stream cipherStream)
        {
            Aes aes = GetEncryptionAlgorithm();
            FromBase64Transform base64Transform = new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces);
            CryptoStream base64DecodedStream = new CryptoStream(cipherStream, base64Transform, CryptoStreamMode.Read);
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            CryptoStream decryptedStream = new CryptoStream(base64DecodedStream, decryptor, CryptoStreamMode.Read);
            return decryptedStream;
        }

        // Метод для дешифрования строки
        public static string DecryptString(string cipherText)
        {
            Aes aes = GetEncryptionAlgorithm();
            byte[] buffer = Convert.FromBase64String(cipherText);
            MemoryStream memoryStream = new MemoryStream(buffer);
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            StreamReader streamReader = new StreamReader(cryptoStream);
            return streamReader.ReadToEnd();
        }

        // Метод для получения алгоритма шифрования
        private static Aes GetEncryptionAlgorithm()
        {
            Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Key = Encoding.UTF8.GetBytes(EncryptionHandler.EncryptKey);
            aes.IV = Encoding.UTF8.GetBytes(EncryptionHandler.EncryptIV);
            return aes;
        }
    }
}
