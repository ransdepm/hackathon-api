using Hackathon.Service.Entities;
using System;

namespace Hackathon.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? Active { get; set; }
    }
}
