using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace lab_1_server
{
    // Класс, представляющий пользователя
    [Table("users")]
    public class User
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("login")]
        public string Login { get; set; } = "";

        [Column("password")]
        public string Password { get; set; } = "";

        public List<Right> Rights { get; set; } = new();
        public List<UserRight> UserRights { get; set; } = new();
    }

    // Класс, представляющий право пользователя
    [Table("rights")]
    public class Right
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("right")]
        public string TypeRight { get; set; } = "";

        public List<User> Users { get; set; } = new();
        public List<UserRight> UserRights { get; set; } = new();
    }

    // Класс, представляющий связь между пользователем и правом
    [Table("users_rights")]
    public class UserRight
    {
        [Column("users_id")]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Column("rights_id")]
        public int RightId { get; set; }
        public Right? Right { get; set; }
    }

    // Класс, представляющий данные
    [Table("data")]
    public class Data
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("value")]
        public string Value { get; set; } = "";
    }

    // Класс, представляющий значение данных
    public class ValueData
    {
        public string Value { get; set; } = "";
    }
}
