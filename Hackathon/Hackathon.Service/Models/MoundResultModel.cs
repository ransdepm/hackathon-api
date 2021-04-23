using System.ComponentModel.DataAnnotations;

namespace Hackathon.Service.Models
{
    public class MoundResultModel
    {
        [Required]
        public string Result { get; set; }
    }
}
