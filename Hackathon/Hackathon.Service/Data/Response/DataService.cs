
using Hackathon.Data;
using Hackathon.Service.Configuration;
using Hackathon.Service.Data.Interface;
using Microsoft.Extensions.Options;
using System;
using System.Data;
using Hackathon.Entities;

namespace Hackathon.Service.Data.Response
{
    public class DataService : IDataService
    {
        private readonly AppSettings _appSettings;
        public DataService(IOptions<AppSettings> settings)
        {
            _appSettings = settings.Value;
        }

    }
}
