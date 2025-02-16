using Authentication.Application.DTOs;
using Authentication.Application.Interfaces;
using Authentication.Domain.Entities;
using Authentication.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Infrastructure.Repositories
{
    public class UserRepository(AuthenticationDbContext context, IConfiguration configuration) : IUser
    {

        private async Task<User> GetUserByEmail(string Email)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Email!.Equals(Email));
            return user is not null ? user! : null!;
        }
        public async Task<GetUserDTO> GetUserAsync(Guid userId)
        {
            var user = await context.Users.FindAsync(userId);
            return user is not null ? new GetUserDTO
            {
                UserId = user.UserId,
                Name = user.UserName,
                Email = user.Email,
                Address = user.Address,
                MobileNumber = user.MobileNumber,
                Role = user.Role
            } : null!;
        }

        public async Task<Response> LoginUser(LoginDTO loginDTO)
        {
            var getUser = await GetUserByEmail(loginDTO.Email!);
            if (getUser is null)
            {
                return new Response { Status = false, Message = "Invalid credentials" };
            }

            if (!BCrypt.Net.BCrypt.Verify(loginDTO.Password, getUser.Password))
            {
                return new Response { Status = false, Message = "Invalid credentials" };
            }

            string token = GenerateToken(getUser);
            return new Response { Status = true, Message = token };
        }

        public async Task<Response> RegisterUser(UserDTO userDTO)
        {
            var getUser = await GetUserByEmail(userDTO.Email!);
            if (getUser is not null)
            {
                return new Response { Status = false, Message = "User already exists" };
            }

            var result = context.Users.Add(new User()
            {
                UserId = Guid.NewGuid(),
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                Address = userDTO.Address,
                MobileNumber = userDTO.MobileNumber,
                Role = userDTO.Role,
                Password = BCrypt.Net.BCrypt.HashPassword(userDTO.Password)
            });
            await context.SaveChangesAsync();
            return result.Entity.UserId != Guid.Empty ? new Response { Status = true, Message = "User created successfully" } : 
                new Response { Status = false, Message = "Invalid data provided" };
        }

        private string GenerateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(configuration.GetSection("Authentication:Key").Value!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!.ToString()),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            if(!string.IsNullOrEmpty(user.Role) || !Equals("string", user.Role))
                claims.Add(new Claim(ClaimTypes.Role, user.Role!));

            var token = new JwtSecurityToken(
                issuer: configuration.GetSection("Authentication:Issuer").Value,
                audience: configuration.GetSection("Authentication:Audience").Value,
                claims: claims,
                expires: null,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
