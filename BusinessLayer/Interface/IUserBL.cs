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

        Task<UserEntity> Registration(RegistrationUserModel registrationUserModel);
        string PasswordHashing(string userPass);
        bool VerifyPassword(string userPass, string storedHashPass);
        Task<bool> ForgetPasswordAsync(ForgetPasswordModel model);
        Task<bool> ResetPasswordAsync(ResetPasswordModel model);
    }
}
