using Hackathon.Entities;
using Hackathon.Service.Entities;
using Hackathon.Service.Models;
using System;
using System.Security.Claims;

namespace Hackathon.Service.Services.Interface
{
    public interface IUserService
    {
        public User LoginAdminUser(AdminUserLoginModel loginModel);
        public User GetCurrentAdminUser(ClaimsIdentity claimsIdentity);
        public User GetAdminUserById(Guid userId);
        public User AuthenticateAdmin(string email, string password);
        public bool ActiveUserExists(string email);
        public User CreateAdminUser(string email, string password);
        public User GetActiveAdminUserByEmail(string email);
    }
}
