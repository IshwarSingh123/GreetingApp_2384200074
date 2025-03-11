using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        
        public async Task<UserEntity> GetEmailAsync(string email)
        {
            var user = await _dbContext.User.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return null;
            }

            return user;
        }


        public async Task<UserEntity> Registration(UserEntity userEntity)
        {
            try
            {
                var data = await _dbContext.User.FirstOrDefaultAsync(e => e.Email == userEntity.Email);
                if (data != null)
                {
                    throw new Exception("User Already registered!");
                }
                _dbContext.User.Add(userEntity);
                await _dbContext.SaveChangesAsync();
                return userEntity;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error in Registration: " + ex.Message);
                throw;
            }
        }


        public async Task<bool> UpdatePasswordAsync(UserEntity userEntity)
        {
            
            _dbContext.User.Update(userEntity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}
