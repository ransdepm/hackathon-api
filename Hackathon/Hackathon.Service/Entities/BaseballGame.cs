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
        public DateTime StartDate { get; set; }
        public string Status { get; set; }
        public int? AwayTeamRuns { get; set; }
        public int? HomeTeamRuns { get; set; }
        public int? Inning { get; set; }
        public string InningHalf { get; set; }
        public int ExternalGameId { get; set; }
    }
}
