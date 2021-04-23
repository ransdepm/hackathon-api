using Hackathon.Service.Entities;
using System;

namespace Hackathon.Entities
{
    public class MoundGameResult
    {
        public int Id { get; set; }
        public string MoundResult { get; set; }
        public string Status { get; set; }
        public int? MoundGameId { get; set; }
    }
}