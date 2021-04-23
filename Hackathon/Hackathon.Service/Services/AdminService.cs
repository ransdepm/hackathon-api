using Hackathon.Service.Services.Interface;
using Hackathon.Service.Configuration;
using Microsoft.Extensions.Options;
using Hackathon.Data;
using System.Data;

namespace Hackathon.Service.Services
{
    public class AdminService : IAdminService
    {

        private readonly AppSettings _appSettings;
        public AdminService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string GetMessage()
        {
            System.Console.WriteLine($"appsetttings connections string: {_appSettings.ConnectionString}");
            using DataAccess d = new DataAccess(_appSettings.ConnectionString);
            DataSet ds = d.GetMessages();

            if (ds.Tables[0].Rows.Count == 0)
                return null;

            DataRow r = ds.Tables[0].Rows[0];
            return r["Name"].ToString();
        }


    }

}
