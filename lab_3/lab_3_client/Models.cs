using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab_3_client
{
    internal class Data
    {
        public int Id { get; set; }
        public string Value { get; set; } = "";
    }

    internal class DataValue
    {
        public DataValue(string value)
        {
            Value = value;
        }

        public string Value { get; set; } = "";
    }

    internal class LoginPassword
    {
        public LoginPassword(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
    }
}

