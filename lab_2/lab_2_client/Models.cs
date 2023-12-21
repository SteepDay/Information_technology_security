using System;

namespace lab_2_client
{
    /// <summary>
    /// Класс, представляющий данные
    /// </summary>
    internal class Data
    {
        public int Id { get; set; }  // Идентификатор
        public string Value { get; set; } = "";  // Значение данных
    }

    /// <summary>
    /// Класс, представляющий значение данных
    /// </summary>
    internal class DataValue
    {
        public DataValue(string value)
        {
            Value = value;
        }

        public string Value { get; set; } = "";  // Значение данных
    }

    /// <summary>
    /// Класс, представляющий логин и пароль
    /// </summary>
    internal class LoginPassword
    {
        public LoginPassword(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; set; } = "";  // Логин
        public string Password { get; set; } = "";  // Пароль
    }
}
