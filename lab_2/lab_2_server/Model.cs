using System.ComponentModel.DataAnnotations.Schema;

namespace lab_2_server
{
    // Класс представляющий сущность "Пользователь"
    [Table("users")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }  // Идентификатор пользователя

        [Column("login")]
        public string Login { get; set; } = "";  // Логин пользователя

        [Column("password")]
        public string Password { get; set; } = "";  // Пароль пользователя

        public List<Right> Rights { get; set; } = new();  // Права пользователя

        public List<UserRight> UserRights { get; set; } = new();  // Связь с таблицей "Пользователь-Права"
    }

    // Класс представляющий сущность "Право"
    [Table("rights")]
    public class Right
    {
        [Column("id")]
        public int Id { get; set; }  // Идентификатор права

        [Column("right")]
        public string TypeRight { get; set; } = "";  // Тип права

        public List<User> Users { get; set; } = new();  // Пользователи, у которых есть это право

        public List<UserRight> UserRights { get; set; } = new();  // Связь с таблицей "Пользователь-Права"
    }

    // Класс представляющий сущность "Связь Пользователь-Право"
    [Table("users_rights")]
    public class UserRight
    {
        [Column("users_id")]
        public int UserId { get; set; }  // Идентификатор пользователя

        public User? User { get; set; }  // Ссылка на пользователя

        [Column("rights_id")]
        public int RightId { get; set; }  // Идентификатор права

        public Right? Right { get; set; }  // Ссылка на право
    }

    // Класс представляющий сущность "Данные"
    [Table("data")]
    public class Data
    {
        [Column("id")]
        public int Id { get; set; }  // Идентификатор данных

        [Column("value")]
        public string Value { get; set; } = "";  // Значение данных
    }

    // Класс для представления данных (например, при отправке через API)
    public class ValueData
    {
        public string Value { get; set; } = "";  // Значение данных
    }

    // Класс для представления логина и пароля (например, при отправке через API)
    public class LoginPassword
    {
        public string Login { get; set; } = "";  // Логин

        public string Password { get; set; } = "";  // Пароль
    }
}
