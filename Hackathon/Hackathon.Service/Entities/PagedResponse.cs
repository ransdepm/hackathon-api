using System.Collections.Generic;

namespace Hackathon.Service.Entities
{
    public class PagedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public long TotalPages { get; set; }
        public long TotalItems { get; set; }
        public long Hits { get; set; }
        public List<T> Results { get; set; }
    }
}