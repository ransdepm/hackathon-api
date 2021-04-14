using Hackathon.Service.Data.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Hackathon.Service.Data.Interface
{
    public interface IResponse
    {
        ObjectResult Respond<T>(DataWrapper<Result<T>> result, ILogger logger);
        ObjectResult HandleError<T>(DataWrapper<Result<T>> result, ILogger logger);
    }
}
