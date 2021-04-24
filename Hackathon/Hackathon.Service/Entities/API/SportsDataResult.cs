using System;

namespace Hackathon.Entities.API
{
    public class SportsDataResult
    {
        public int GameID { get; set; }
        public DateTime DateTime { get; set; }
        public string Status { get; set; }
        public string AwayTeam { get; set; }
        public string HomeTeam { get; set; }
        public int AwayTeamRuns { get; set; }
        public int HomeTeamRuns { get; set; }
        public int Inning { get; set; }
        public string InningHalf { get; set; }
    }

   
}