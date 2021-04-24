using System;
using Hackathon.Service.Configuration;
using Microsoft.Extensions.Options;
using Hackathon.Service.Repositories.Interface;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Generic;
using Hackathon.Entities;
using Hackathon.Entities.API;

namespace Hackathon.Service.Repositories
{
    public class SportsDataApiRepository : ISportsDataApiRepository
    {
        private readonly AppSettings _appSettings;


        public SportsDataApiRepository(
            IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            JsonConvert.DefaultSettings = () =>
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        private HttpClient GetClient()
        {
            HttpClient _client = new HttpClient();
            _client.BaseAddress = new Uri("https://fly.sportsdata.io/v3/mlb/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _appSettings.SportsDataApiKey);
            return _client;
        }

        public async Task<List<BaseballGame>> GetTodaysGames()
        {
            var date = DateTime.Now.ToString("yyyy-MMM-dd");
            HttpResponseMessage response = await GetClient().GetAsync($"scores/json/GamesByDate/{date}");

            if (response.IsSuccessStatusCode)
            {
                string responseJsonString = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<List<SportsDataResult>>(responseJsonString);
                return Convert(result);
            }

            return null;
        }

        public List<BaseballGame> Convert(List<SportsDataResult> results)
        {
            var games = new List<BaseballGame>();
            if (results == null)
                return null;

            foreach (var a in results)
            {
                var game = new BaseballGame
                {
                    //Id = ,
                    HomeTeam = a.HomeTeam,
                    HomeTeamLogo = GetLogo(a.HomeTeam),
                    AwayTeam = a.AwayTeam,
                    AwayTeamLogo = GetLogo(a.AwayTeam),
                    StartDate = ConvertTime(a.DateTime),
                    Status = a.Status,
                    AwayTeamRuns = a.AwayTeamRuns,
                    HomeTeamRuns = a.HomeTeamRuns,
                    Inning = a.Inning,
                    InningHalf = a.InningHalf,
                    ExternalGameId = a.GameID
                };
                games.Add(game);
            }

            return games;
        }

        private DateTime ConvertTime(DateTime time)
        {
            string easternZoneId = "Eastern Standard Time";

            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(easternZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(time, easternZone);
        }


        private string GetLogo(string team)
        {
            string logo = "https://storage.cloud.google.com/mlb-logo-hackathon/";
            switch(team)
            {
                case "ARI":
                    logo += "Diamondbacks-logo.png";
                    break;
                case "ATL":
                    logo += "Braves-logo.png";
                    break;
                case "BAL":
                    logo += "Orioles-logo.png";
                    break;
                case "BOS":
                    logo += "Redsox-logo.png";
                    break;
                case "CHC":
                    logo += "Cubs-logo.png";
                    break;
                case "CWS":
                    logo += "WhiteSox-logo.png";
                    break;
                case "CIN":
                    logo += "Reds-logo.png";
                    break;
                case "CLE":
                    logo += "Indians-logo.png";
                    break;
                case "COL":
                    logo += "Rockies-logo.png";
                    break;
                case "DET":
                    logo += "Tigers-logo.png";
                    break;
                case "HOU":
                    logo += "Astros-logo.png";
                    break;
                case "KC":
                    logo += "Royals-logo.png";
                    break;
                case "LAA":
                    logo += "Angels-logo.png";
                    break;
                case "LAD":
                    logo += "Dodgers-logo.png";
                    break;
                case "MIA":
                    logo += "Marlins-logo.png";
                    break;
                case "MIL":
                    logo += "Brewers-logo.png";
                    break;
                case "MIN":
                    logo += "Twins-logo.png";
                    break;
                case "NYM":
                    logo += "Mets-logo.png";
                    break;
                case "NYY":
                    logo += "Yankees-logo.png";
                    break;
                case "OAK":
                    logo += "Oakland-logo.png";
                    break;
                case "PHI":
                    logo += "Phillies-logo.png";
                    break;
                case "PIT":
                    logo += "Pirates-logo.png";
                    break;
                case "SD":
                    logo += "Padres-logo.png";
                    break;
                case "SEA":
                    logo += "Mariners-logo.png";
                    break;
                case "SF":
                    logo += "Giants-logo.png";
                    break;
                case "STL":
                    logo += "Cardinals-logo.png";
                    break;
                case "TB":
                    logo += "Rays-logo.png";
                    break;
                case "TEX":
                    logo += "Rangers-logo.png";
                    break;
                case "TOR":
                    logo += "BlueJays-logo.png";
                    break;
                case "WSH":
                    logo += "Nationals-logo.png";
                    break;
                default:
                    logo += "mlb-logo.png";
                    break;
            }

            return logo;
        }

    }
}
