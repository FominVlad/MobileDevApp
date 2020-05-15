using System.Security.Cryptography;
using System.Text;

namespace MobileDevApp.Helpers
{
    public class HashHelper
    {
        public string GenerateHash(string inputStr)
        {
            // Переводим строку в байт-массим
            byte[] bytes = Encoding.Unicode.GetBytes(inputStr);

            // Создаем объект для получения средств шифрования
            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            // Вычисляем хеш-представление в байтах
            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;

            // Формируем одну цельную строку из массива
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return hash;
        }
    }
}
