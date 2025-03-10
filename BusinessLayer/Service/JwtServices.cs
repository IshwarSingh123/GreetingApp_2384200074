﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Entity;

namespace BusinessLayer.Service
{
    public class JwtServices
    {
        private readonly IConfiguration _configuration;

        public JwtServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(UserEntity userEntity)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, userEntity.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //new Claim(ClaimTypes.Name, userEntity.Name),
            new Claim(ClaimTypes.Email, userEntity.Email),
            new Claim(ClaimTypes.Sid,userEntity.Id.ToString())

};


            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryMinutes"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateResetToken(string userEmail)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]); // Secret key from config
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Email, userEmail),
                new Claim("ResetToken", Guid.NewGuid().ToString()) // Unique token claim
            }),
                Expires = DateTime.UtcNow.AddMinutes(15), // Token valid for 15 minutes
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token); // Return generated token
        }
    }
}
