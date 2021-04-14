using System.ComponentModel.DataAnnotations;

namespace Hackathon.Service.Models
{
    public class AdminUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string OrganizationIdentifier { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
