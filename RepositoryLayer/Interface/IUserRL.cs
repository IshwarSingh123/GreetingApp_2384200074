using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        UserEntity Login(LoginUserModel userLoginModel);
        Task<UserEntity> GetEmailAsync(string email);
        Task<UserEntity> Registration(UserEntity userEntity);
        Task<bool> UpdatePasswordAsync(UserEntity userEntity);



    }
}
