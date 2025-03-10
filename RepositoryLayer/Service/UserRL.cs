using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        HelloGreetingAppContext _dbContext;
        private readonly ILogger<UserRL> _logger;

        public UserRL(HelloGreetingAppContext dbContext, ILogger<UserRL> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public UserEntity Login(LoginUserModel userLoginModel)
        {
            try
            {
                var data = _dbContext.User.FirstOrDefault(e => e.Email == userLoginModel.Email);
                if (data != null)
                {
                    return data;
                }
                return null;
            }
            catch(Exception ex)
            {
                _logger.LogError("Id does not exist");
                throw;
            }
            

        }
        public UserEntity GetEmail(string mail)
        {
            return _dbContext.User.FirstOrDefault(e => e.Email == mail);
        }

        public UserEntity Registration(UserEntity userEntity)
        {
            try
            {
                var data = _dbContext.User.FirstOrDefault(e => e.Email == userEntity.Email);
                if (data != null)
                {
                    throw new Exception("User Already registered!");
                }
                _dbContext.User.Add(userEntity);
                _dbContext.SaveChanges();
                return data;


            }
            catch (Exception ex)
            {
                _logger.LogError("Id does not exist");
                throw;
            }
        }

       
    }
}
