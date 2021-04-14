using Hackathon.Service.Configuration;
using Hackathon.Service.Data.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace Hackathon.Service.Data.Response
{
    public class Response : IResponse
    {
        public ObjectResult Respond<T>(DataWrapper<Result<T>> result, ILogger logger)
        {
            if (result == null)
            {
                const string errorMessage = "Result Object not defined";

                var error = new ErrorModel(ErrorCode.NoContent, errorMessage, errorMessage);

                logger.Log(LogLevel.Error, JsonConvert.SerializeObject(error));

                return new BadRequestObjectResult(error);
            }

            if (result.Data.IsSuccess)
            {
                return new OkObjectResult(result);
            }

            if (result.Data.IsException)
            {
                logger.Log(LogLevel.Error, JsonConvert.SerializeObject(result));
            }

            if (!result.Data.IsSuccess)
            {
                logger.Log(LogLevel.Error, JsonConvert.SerializeObject(result));
            }

            return new BadRequestObjectResult(result);
        }

        public ObjectResult HandleError<T>(DataWrapper<Result<T>> result, ILogger logger)
        {
            return Respond(new DataWrapper<Result<IActionResult>>
                (Result<IActionResult>.CreateException(new Exception(result.Data.Exception))), logger);
        }
    }
}
