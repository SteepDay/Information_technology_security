using System.Security.Cryptography;
using System.Text;

namespace lab_3_server
{
    // Класс для обработки шифрования и дешифрования данных
    public class EncryptionMiddleware
    {
        private readonly RequestDelegate _next;

        // Ключ и вектор инициализации для шифрования и дешифрования
        private static readonly string EncryptKey = "1eNa1Zx9KKTqQteM4Mj1G7M9kr0upqY2";
        private static readonly string EncryptIV = "En3wn0vh4JAKdNwZ";

        public EncryptionMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        // Обработка запроса
        public async Task InvokeAsync(HttpContext httpContext)
        {
            // Шифрование тела ответа
            httpContext.Response.Body = EncryptStream(httpContext.Response.Body);
            // Дешифрование тела запроса
            httpContext.Request.Body = DecryptStream(httpContext.Request.Body);

            await _next.Invoke(httpContext);

            await httpContext.Request.Body.DisposeAsync();
            await httpContext.Response.Body.DisposeAsync();
        }

        // Шифрование потока данных
        private static CryptoStream EncryptStream(Stream responseStream)
        {
            Aes aes = GetEncryptionAlgorithm();
            ToBase64Transform base64Transform = new ToBase64Transform();
            CryptoStream base64EncodedStream = new CryptoStream(responseStream, base64Transform, CryptoStreamMode.Write);
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            CryptoStream cryptoStream = new CryptoStream(base64EncodedStream, encryptor, CryptoStreamMode.Write);
            return cryptoStream;
        }

        // Дешифрование потока данных
        private static Stream DecryptStream(Stream cipherStream)
        {
            Aes aes = GetEncryptionAlgorithm();
            FromBase64Transform base64Transform = new FromBase64Transform(FromBase64TransformMode.IgnoreWhiteSpaces);
            CryptoStream base64DecodedStream = new CryptoStream(cipherStream, base64Transform, CryptoStreamMode.Read);
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            CryptoStream decryptedStream = new CryptoStream(base64DecodedStream, decryptor, CryptoStreamMode.Read);
            return decryptedStream;
        }

        // Получение объекта для шифрования
        private static Aes GetEncryptionAlgorithm()
        {
            Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.Key = Encoding.UTF8.GetBytes(EncryptKey);
            aes.IV = Encoding.UTF8.GetBytes(EncryptIV);
            return aes;
        }
    }
}
