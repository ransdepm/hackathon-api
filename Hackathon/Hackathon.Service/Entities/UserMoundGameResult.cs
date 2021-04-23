using Hackathon.Service.Entities;
using System;

namespace Hackathon.Entities
{
    public class UserMoundGameResult
    {
        public int MoundResultId { get; set; }
        public string MoundResult { get; set; }
        public string UserSubmission { get; set; }
        public int Score { get; set; }
    }
}