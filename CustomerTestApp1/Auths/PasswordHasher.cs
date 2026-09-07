using CustomerTestApp1.Models;
using Microsoft.AspNetCore.Identity;

namespace CustomerTestApp1.Auths
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            var hasher = new PasswordHasher<User>();



            return "";
        }
    }

    public interface IPasswordHasher
    {
        string Hash(string password);
    }

}
