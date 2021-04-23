using System.ComponentModel.DataAnnotations;

namespace Hackathon.Service.Models
{
    public class UserModel
    {
        [Required]
        public string Name { get; set; }
    }
}
