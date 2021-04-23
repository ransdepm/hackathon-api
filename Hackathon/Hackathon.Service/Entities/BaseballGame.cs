using Hackathon.Service.Entities;
using System;

namespace Hackathon.Entities
{
    public class BaseballGame
    {
        public int Id { get; set; }
        public string HomeTeam { get; set; }
        public string HomeTeamLogo { get; set; }
        public string AwayTeam { get; set; }
        public string AwayTeamLogo { get; set; }
        public DateTime? StartDate { get; set; }
        public string Status { get; set; }
    }
}
