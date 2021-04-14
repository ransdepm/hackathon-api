using Hackathon.Service.Data.Response;
using Hackathon.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hackathon.Service.Configuration
{
    public class Helper
    {
        public static ErrorModel BuildError(ErrorCode errorCode, string title, string detailedError)
        {
            return new ErrorModel(errorCode, title, detailedError);
        }
    }
}
