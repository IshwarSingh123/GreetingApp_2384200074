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
        private readonly IEmailService _emailService;
        
        public UserBL(IUserRL userRL, ILogger<UserRL> logger, JwtServices jwtServices,IEmailService emailService)
        {
            _userRL = userRL;
            _logger = logger;
            _jwtServices = jwtServices;
            _emailService = emailService;
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
                _logger.LogError(ex.Message);
                throw;
            }
           
            
        }

        public async Task<UserEntity> Registration(RegistrationUserModel registrationUserModel)
        {
            try
            {
                var existingUser = await _userRL.GetEmailAsync(registrationUserModel.Email);
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

                return await _userRL.Registration(newUser);
            }
            catch (Exception )
            {
                throw;
            }


        }

        public string PasswordHashing(string userPass)
        {
            try
            {
              

                byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
                using var pbkdf2 = new Rfc2898DeriveBytes(userPass, salt, Iterations, HashAlgorithmName.SHA256);
                byte[] hash = pbkdf2.GetBytes(HashSize);

                byte[] hashByte = new byte[SaltSize + HashSize];
                Buffer.BlockCopy(salt, 0, hashByte, 0, SaltSize);
                Buffer.BlockCopy(hash, 0, hashByte, SaltSize, HashSize);

                return Convert.ToBase64String(hashByte);

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


        public async Task<bool> ForgetPasswordAsync(ForgetPasswordModel forgetPasswordModel)
        {
            if (string.IsNullOrWhiteSpace(forgetPasswordModel?.Email))
            {
                throw new ArgumentException("Email cannot be null or empty.", nameof(forgetPasswordModel.Email));
            }

            var user = await _userRL.GetEmailAsync(forgetPasswordModel.Email);
            if (user == null)
            {
                return false;
            }

            // Generate JWT token for password reset
            string token = _jwtServices.GenerateToken(user);
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new InvalidOperationException("Generated token is null or empty.");
            }

            return await _emailService.SendEmailAsync(forgetPasswordModel.Email, "Reset Password", token);
        }




        public async Task<bool> ResetPasswordAsync(ResetPasswordModel resetPasswordModel)
        {
            string email = _jwtServices.ValidateToken(resetPasswordModel.Token);
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            var user = await _userRL.GetEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            // Hash the new password
            user.Password = PasswordHashing(resetPasswordModel.NewPassword);

            return await _userRL.UpdatePasswordAsync(user);
        }
    }
}
