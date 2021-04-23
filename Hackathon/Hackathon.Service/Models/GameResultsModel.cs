using System.ComponentModel.DataAnnotations;

namespace Hackathon.Service.Models
{
    public class GameResultsModel
    {
        [Required]
        public string Username { get; set; }
    }
}
