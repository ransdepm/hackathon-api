using Hackathon.Entities;
using Hackathon.Service.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackathon.Service.Services.Interface
{
    public interface IGameService
    {
        public List<BaseballGame> GetGames();
        public MoundGame GetMoundGame(int gameId);
        public void LockMoundResult(int id);
        public MoundGameResult StoreMoundResult(int id, MoundResultModel model);
        public MoundGameResult GetMoundResultById(int moundResultId);

        public UserMoundGameSubmission StoreUserMoundResult(int id, Guid userId, UserResultModel model);
        public List<UserMoundGameResult> GetMoundGameResultsForUser(int moundGameId, Guid id);
        public List<MoundGameTotals> GetAllMoundGameResults(int moundGameId);
        public Task<List<BaseballGame>> UpdateGames();
    }
}
