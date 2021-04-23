using Hackathon.Entities;
using Hackathon.Service.Entities;
using Hackathon.Service.Models;
using System;
using System.Security.Claims;

namespace Hackathon.Service.Services.Interface
{
    public interface IUserService
    {
        public AdminUser LoginAdminUser(AdminUserLoginModel loginModel);
        public AdminUser GetCurrentAdminUser(ClaimsIdentity claimsIdentity);
        public AdminUser GetAdminUserById(Guid userId);
        public AdminUser AuthenticateAdmin(string email, string password);
        public bool ActiveAdminUserExists(string email);
        public AdminUser CreateAdminUser(string email, string password);
        public AdminUser GetActiveAdminUserByEmail(string email);

        public bool ActiveUserExists(string name);
        public User CreateUser(string name);
    }
}
