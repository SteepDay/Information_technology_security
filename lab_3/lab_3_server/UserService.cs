using System.Text;
using System.Security.Cryptography;

namespace lab_3_server
{
    public interface IUserService
    {
        public List<string> GetUserRights(string login, string password);
    }

    public class UserService : IUserService
    {
        private readonly AppServerContext _dbContext;
        public UserService(
            AppServerContext dbContext
            )
        {
            _dbContext = dbContext;
        }

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
