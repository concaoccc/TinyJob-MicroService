using WebService.Models;
using Common.Database.Repositories;
using Common.Database.PO;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace WebService.Services
{
    public class UserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public void ValidateUserCredential(UserCredential userCredential)
        {
            var user = userRepository.GetByUserName(userCredential.UserName);
            bool isValid = user != null && IsValidCredential(userCredential, user);
            if (!isValid)
            {
                throw new InvalidCredentialException();
            }
        }

        public bool IsValidCredential(UserCredential userCredential, User user)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            string pwdHash = BitConverter.ToString(md5.ComputeHash(Encoding.UTF8.GetBytes(userCredential.Password)));
            return user.Name == userCredential.UserName && user.Pwd == pwdHash;
        }
    }
}
