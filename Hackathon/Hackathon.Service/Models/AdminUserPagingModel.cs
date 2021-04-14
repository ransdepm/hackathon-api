using System.ComponentModel.DataAnnotations;

namespace Hackathon.Service.Models
{
    public class AdminUserPagingModel
    {
        public int PageSize { get; set; } = 250;
        public int PageNumber { get; set; } = 0;
        public string Search { get; set; }
        public string SortColumn { get; set; } = "email";
        public bool Ascending { get; set; } = true;
    }
}
