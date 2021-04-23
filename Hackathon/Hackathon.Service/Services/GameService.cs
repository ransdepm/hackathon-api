using Hackathon.Service.Services.Interface;
using Hackathon.Service.Configuration;
using Microsoft.Extensions.Options;
using Hackathon.Data;
using System.Data;
using System.Collections.Generic;
using Hackathon.Entities;
using System;
using Hackathon.Service.Utilities;
using Hackathon.Service.Models;

namespace Hackathon.Service.Services
{
    public class GameService : IGameService
    {

        private readonly AppSettings _appSettings;
        public GameService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public List<BaseballGame> GetGames()
        {
            using DataAccess d = new DataAccess(_appSettings.ConnectionString);
            DataSet ds = d.GetBaseballGames();

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            var games = new List<BaseballGame>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                var game = new BaseballGame
                {
                    Id = Convert.ToInt32(r["Id"]),
                    HomeTeam = r["HomeTeam"].ToString(),
                    HomeTeamLogo = r["HomeTeamLogo"].ToString(),
                    AwayTeam = r["AwayTeam"].ToString(),
                    AwayTeamLogo = r["AwayTeamLogo"].ToString(),
                    Status = r["Status"].ToString(),
                    StartDate = DatabaseUtility.ReadDateTimeUTC(r, "StartDate")
                };
                games.Add(game);

            }
            return games;
        }


        public MoundGame GetMoundGame(int baseballGameId)
        {
            DataSet ds;
            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                ds = d.GetMoundGameByBaseballGameId(baseballGameId);
            }

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow r = ds.Tables[0].Rows[0];
            var moundGameId = Convert.ToInt32(r["Id"]);

            var game = new MoundGame
            {
                Id = Convert.ToInt32(r["Id"]),
                Status = r["Status"].ToString(),
                MoundGameResults = GetMoundResults(moundGameId)
            };

            return game;
        }

        public MoundGameResult GetMoundResultById(int moundResultId)
        {
            DataSet ds;
            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                ds = d.GetMoundResultById(moundResultId);
            }

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow r = ds.Tables[0].Rows[0];

            var result = new MoundGameResult
            {
                Id = Convert.ToInt32(r["Id"]),
                Status = r["Status"].ToString(),
                MoundResult = r["MoundResult"].ToString(),
                MoundGameId = Convert.ToInt32(r["MoundGameId"])
            };

            return result;
        }

        public List<MoundGameResult> GetMoundResults(int moundGameId)
        {
            DataSet ds;
            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                ds = d.GetMoundGameResults(moundGameId);
            }

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            var results = new List<MoundGameResult>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                var result = new MoundGameResult
                {
                    Id = Convert.ToInt32(r["Id"]),
                    Status = r["Status"].ToString(),
                    MoundResult = r["MoundResult"].ToString()
                };
                results.Add(result);

            }
            return results;

        }

        public void LockMoundResult(int id)
        {
            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                d.LockMoundRound(id);
            }

        }

        public MoundGameResult StoreMoundResult(int id, MoundResultModel model)
        {
            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                d.AddMoundResult(id, model.Result);
            }

            MoundGameResult moundResult = GetMoundResultById(id);

            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                d.StartNextMoundRound(moundResult.MoundGameId.Value);
            }

            return moundResult;
        }

        public UserMoundGameSubmission StoreUserMoundResult(int id, Guid userId, UserResultModel model)
        {
            var existingResult = GetUserMoundResult(id, userId);
            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                if (existingResult == null)
                {
                    d.AddUserMoundResult(id, userId, model.Submission);
                }
                else
                {
                    d.UpdateUserMoundResult(id, userId, model.Submission);
                }
            }

            return GetUserMoundResult(id, userId);
        }

        public List<UserMoundGameResult> GetMoundGameResultsForUser(int moundGameId, Guid userId)
        {
            DataSet ds;
            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                ds = d.GetUserResultsByMoundGame(moundGameId, userId);
            }

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            var results = new List<UserMoundGameResult>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                var submission = r["Submission"].ToString();
                var moundResult = r["MoundResult"].ToString();
                var result = new UserMoundGameResult
                {
                    MoundResultId = Convert.ToInt32(r["Id"]),
                    Score = (submission == moundResult) ? 1 : 0
                };
                results.Add(result);

            }
            return results;
        }

        public List<MoundGameTotals> GetAllMoundGameResults(int moundGameId)
        {
            DataSet ds;
            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                ds = d.GetAllResultsByMoundGame(moundGameId);
            }

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            var results = new List<MoundGameTotals>();
            foreach (DataRow r in ds.Tables[0].Rows)
            {
                var result = new MoundGameTotals
                {
                    Username =r["Name"].ToString(),
                    Score = Convert.ToInt32(r["Score"])
                };
                results.Add(result);

            }
            return results;

        }

        public UserMoundGameSubmission GetUserMoundResult(int id, Guid userId)
        {
            DataSet ds;
            using (DataAccess d = new DataAccess(_appSettings.ConnectionString))
            {
                ds = d.GetUserMoundResultByIds(id, userId);
            }

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow r = ds.Tables[0].Rows[0];

            var result = new UserMoundGameSubmission
            {
                Id = Convert.ToInt32(r["Id"]),
                Submission = r["Submission"].ToString()
            };

            return result;
        }
    }

}
