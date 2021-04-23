using Hackathon.Entities;
using System;
using System.Collections.Generic;

namespace Hackathon.Service.Services.Interface
{
    public interface IGameService
    {
        public List<BaseballGame> GetGames();
    }
}
