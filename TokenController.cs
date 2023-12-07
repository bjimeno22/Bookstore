using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Bookstore;
//using PizzaStore.Data;
//using PizzaStore.Entities;
//using PizzaStore.Helpers;
using Bookstore.Models.RequestModels;

namespace Bookstore
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly PasswordManager _passwordManager;

        public TokenController(ApplicationDbContext context, PasswordManager passwordManager)
        {
            _dbContext = context;
            _passwordManager = passwordManager;
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateRequestModel requestModel)
        {
            var user = await GetUserAsync(requestModel.Email);

            if (user == null)
            {
                return Forbid();
            }
            else if(!_passwordManager.VerifyPassword(requestModel.Password,user.PasswordHash,user.PasswordSalt))
            {
                return Forbid();
            }

            var token = await GenerateTokenAsync(user);

            return Ok(token);
        }

        private string? hashPassword(string password)
        {
            throw new NotImplementedException();
        }

        private async Task<User> GetUserAsync(string emailAddress)
        {
            var user = await _dbContext.Users.Where(u => u.Email.ToLower().Equals(emailAddress.ToLower())).Include(u => u.Role).SingleOrDefaultAsync();
            return user;
        }
        private async Task<string> GenerateTokenAsync(User user)
        {
            var secret = "MyVerySuperSecureSecretSharedKey";
            var secretKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));

            var issuer = "http://www.ecu.edu";
            var audience = "http://www.ecu.edu";

            var claims = new Dictionary<string, object>();
            claims.Add("rol", new string[] { user.Role.RoleName });
            claims.Add("email", user.Email);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddDays(30),
                NotBefore = DateTime.UtcNow,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(secretKey,
                                                            SecurityAlgorithms.HmacSha256Signature),
                Claims = claims
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);
            return await Task.FromResult(token);

        }
    }
}