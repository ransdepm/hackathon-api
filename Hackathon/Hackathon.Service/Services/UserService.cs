using Hackathon.Service.Models;
using Hackathon.Service.Services.Interface;
using System;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Hackathon.Service.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Hackathon.Entities;
using System.Data;
using Hackathon.Data;
using System.Security.Cryptography;
using Hackathon.Service.Utilities;

namespace Hackathon.Service.Services
{
    public class UserService : IUserService
    {

        private readonly AppSettings _appSettings;
        private readonly IWebHostEnvironment _hostEnvironment;
        public UserService(IOptions<AppSettings> appSettings, IWebHostEnvironment hostEnvironment)
        {
            _appSettings = appSettings.Value;
            _hostEnvironment = hostEnvironment;
        }

        public AdminUser LoginAdminUser(AdminUserLoginModel loginModel)
        {
            DataSet ds;
            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                ds = d.GetActiveAdminUserByEmail(loginModel.Email);
            }

            if (ds.Tables[0].Rows.Count != 1)
            {
                return null;
            }

            var r = ds.Tables[0].Rows[0];
            AdminUser user = ValidateAdminUser(r, loginModel);

            return user;
        }

        public AdminUser GetCurrentAdminUser(ClaimsIdentity claimsIdentity)
        {
            var user = new AdminUser();

            if (claimsIdentity != null)
            {
                var claimId = claimsIdentity.FindFirst("Id");

                if (claimId == null)
                    return null;

                var id = Guid.Parse(claimId.Value);
                user = GetAdminUserById(id);
            }
            return user;
        }

        public AdminUser GetAdminUserById(Guid userId)
        {
            DataSet ds;
            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                ds = d.GetAdminUserById(userId);
            }

            if (ds.Tables[0].Rows?.Count != 1)
            {
                return null;
            }

            var user = ParseAdminUser(ds.Tables[0].Rows[0]);

            return user;
        }


        public AdminUser AuthenticateAdmin(string email, string password)
        {
            var model = new AdminUserLoginModel
            {
                Email = email,
                Password = password
            };
            var user = LoginAdminUser(model);

            return user;
        }


        public bool ActiveAdminUserExists(string email)
        {
            DataSet ds;
            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                ds = d.GetActiveAdminUserByEmail(email);
            }

            return (ds.Tables[0].Rows.Count > 0);
        }

        public AdminUser GetActiveAdminUserByEmail(string email)
        {
            DataSet ds;
            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                ds = d.GetActiveAdminUserByEmail(email);
            }

            if (ds.Tables[0].Rows.Count != 1)
            {
                return null;
            }

            var r = ds.Tables[0].Rows[0];
            var user = ParseAdminUser(r);

            user.Token = GenerateAdminToken(user.Id, 1);

            return user;
        }

        public AdminUser CreateAdminUser(string email, string password)
        {
            var user = new AdminUser
            {
                Email = email,
            };

            var salt = Guid.NewGuid();
            var passwordHash = GenerateHash(password, salt.ToString());

            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                user.Id = d.CreateAdminUser(email, passwordHash, salt);
            }

            user.Token = GenerateAdminToken(user.Id);

            return user;
        }

        public bool ActiveUserExists(string name)
        {
            DataSet ds;
            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                ds = d.GetActiveUserByName(name);
            }

            return (ds.Tables[0].Rows.Count > 0);
        }

        public User CreateUser(string name)
        {
            var user = new User
            {
                Name = name,
            };

            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                user.Id = d.CreateUser(name);
            }

            return user;
        }


        private AdminUser ParseAdminUser(DataRow r)
        {
            var user = new AdminUser
            {
                Id = Guid.Parse(r["Id"].ToString()),
                Email = r["Email"].ToString(),
                CreatedDate = DatabaseUtility.ReadDateTimeUTC(r, "CreatedDate"),
                ActivatedDate = DatabaseUtility.ReadDateTimeUTC(r, "ActivatedDate")
            };
            return user;
        }


        private AdminUser ValidateAdminUser(DataRow r, AdminUserLoginModel loginModel)
        {
            AdminUser user = ParseAdminUser(r);
            var storedPasswordHash = Encoding.Default.GetString((byte[])r["PasswordHash"]);
            var storedSalt = r["Salt"].ToString();

            if (CheckPassword(loginModel.Password, storedPasswordHash, storedSalt))
            {
                user.Token = GenerateAdminToken(user.Id);
                return user;
            }
            else
            {
                return null;
            }
        }

        private bool CheckPassword(string plainTextInput, string hashedInput, string salt)
        {
            string newHashedPin = GenerateHash(plainTextInput, salt);
            return newHashedPin.Equals(hashedInput);
        }

        private string GenerateHash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input + salt);
            SHA256Managed sHA256ManagedString = new SHA256Managed();
            byte[] hash = sHA256ManagedString.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private string GenerateAdminToken(Guid id, int expiration = 90)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, "DashboardAdmin"),
                    new Claim("DashboardAccess", "true"),
                    new Claim("Id", id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(expiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            string returnToken = tokenHandler.WriteToken(token);

            return returnToken;
        }
    }
}
