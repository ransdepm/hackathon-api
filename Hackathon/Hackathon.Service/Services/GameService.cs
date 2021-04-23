using Hackathon.Service.Services.Interface;
using Hackathon.Service.Configuration;
using Microsoft.Extensions.Options;
using Hackathon.Data;
using System.Data;
using System.Collections.Generic;
using Hackathon.Entities;
using System;
using Hackathon.Service.Utilities;

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
            foreach( DataRow r in ds.Tables[0].Rows)
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


    }

}
