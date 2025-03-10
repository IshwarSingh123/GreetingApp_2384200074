using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        string Login(LoginUserModel userLoginModel);

        UserEntity Registration(RegistrationUserModel registrationUserModel);
        string PasswordHashing(string userPass);
        bool VerifyPassword(string userPass, string storedHashPass);
        UserEntity ForgetPassword(string password);
    }
}
