using System.ComponentModel.DataAnnotations;

namespace Hackathon.Service.Models
{
    public class UserResultModel
    {
        [Required]
        public string Submission { get; set; }

        [Required]
        public string Username { get; set; }
    }
}
