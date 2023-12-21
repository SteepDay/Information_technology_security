using System;
using System.Text;
using System.Security.Cryptography;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace lab_2_server
{
    /// <summary>
    /// Интерфейс для работы с пользователями
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Получить права пользователя на основе логина и пароля
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>Список прав пользователя</returns>
        public List<string> GetUserRights(string login, string password);
    }

    /// <summary>
    /// Сервис для работы с пользователями
    /// </summary>
    public class UserService : IUserService
    {
        private readonly AppServerContext _dbContext;

        public UserService(AppServerContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Получить права пользователя на основе логина и пароля
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns>Список прав пользователя</returns>
        public List<string> GetUserRights(string login, string password)
        {
            List<string> rights = new List<string>();
            var passwordHash = SHA1.HashData(Encoding.UTF8.GetBytes(password));
            var userFind = _dbContext.Users.FirstOrDefault(u => u.Login == login && u.Password == Encoding.UTF8.GetString(passwordHash));
            if (userFind != null)
            {
                _dbContext.Entry(userFind).Collection(c => c.Rights).Load();
                foreach (Right right in userFind.Rights)
                {
                    rights.Add(right.TypeRight);
                }
            }
            return rights;
        }
    }
}
