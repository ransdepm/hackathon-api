using Hackathon.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hackathon.Service.Repositories.Interface
{
    public interface ISportsDataApiRepository
    {
        public Task<List<BaseballGame>> GetTodaysGames();
    }
}