using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using Microsoft.Extensions.Logging;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {

        // Constants for PBKDF2
        private const int SaltSize = 16; 
        private const int HashSize = 20; 
        private const int Iterations = 10000; 

        private readonly IUserRL _userRL;
        private readonly ILogger<UserRL> _logger;
        private readonly JwtServices _jwtServices;
        
        public UserBL(IUserRL userRL, ILogger<UserRL> logger, JwtServices jwtServices)
        {
            _userRL = userRL;
            _logger = logger;
            _jwtServices = jwtServices;
        }
        public string Login(LoginUserModel userLoginModel)
        {
            try
            {
                var data = _userRL.Login(userLoginModel);

                if (data != null && VerifyPassword(userLoginModel.Password,data.Password))
                {
                    var token = _jwtServices.GenerateToken(data);
                    return token;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("Data Not found");
                throw;
            }
           
            
        }

        public UserEntity Registration(RegistrationUserModel registrationUserModel)
        {
            try
            {
                var existingUser = _userRL.GetEmail(registrationUserModel.Email);
                if (existingUser != null)
                {
                    throw new Exception("User already Registered!");
                }

               
                var newUser = new UserEntity
                {
                    FirstName = registrationUserModel.FirstName,
                    LastName = registrationUserModel.LastName,
                    Email = registrationUserModel.Email,
                    Password = PasswordHashing(registrationUserModel.Password), 
                    PhoneNumber = registrationUserModel.PhoneNumber,
                };

                return _userRL.Registration(newUser);
            }
            catch (Exception ex)
            {
                throw;
            }


        }

        public string PasswordHashing(string userPass)
        {
            try
            {
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
                var pbkdf2 = new Rfc2898DeriveBytes(userPass, salt, Iterations);
                byte[] hash = pbkdf2.GetBytes(HashSize);
                byte[] hashByte = new byte[SaltSize + HashSize];
                Array.Copy(salt, 0, hashByte, 0, SaltSize);
                Array.Copy(hash, 0, hashByte, SaltSize, HashSize);
                string hashedPassword = Convert.ToBase64String(hashByte);
                return hashedPassword;
            }
            catch (Exception ex)
            {
                throw new Exception("Error hashing password.", ex);
            }
        }

       

        public bool VerifyPassword(string userPass, string storedHashPass)
        {
            try
            {
                byte[] hashByte = Convert.FromBase64String(storedHashPass);
                byte[] salt = new byte[SaltSize];
                Array.Copy(hashByte, 0, salt, 0, SaltSize);
                var pbkdf2 = new Rfc2898DeriveBytes(userPass, salt, Iterations);
                byte[] hash = pbkdf2.GetBytes(HashSize);

                // Compare stored hash with computed hash
                for (int i = 0; i < HashSize; i++)
                {
                    if (hashByte[i + SaltSize] != hash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error verifying password.", ex);
            }
        }

        public UserEntity ForgetPassword(string password)
        {
            throw new NotImplementedException();
        }
    }
}
