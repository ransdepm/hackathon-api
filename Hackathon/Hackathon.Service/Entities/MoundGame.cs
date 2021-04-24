using Hackathon.Service.Entities;
using System;
using System.Collections.Generic;

namespace Hackathon.Entities
{
    public class MoundGame
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public List<MoundGameResult> MoundGameResults { get; set; }
        public BaseballGame BaseballGame { get; set; }
    }
}
