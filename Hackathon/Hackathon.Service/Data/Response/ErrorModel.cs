using System;

namespace Hackathon.Service.Data.Response
{
    public class ErrorModel
    {
        public ErrorModel(ErrorCode errorCode, string title, string detail)
        {
            Id = Guid.NewGuid().ToString();
            ErrorCode = errorCode;
            Title = title;
            Detail = detail;
        }
        public string Id { get; set; }
        public ErrorCode ErrorCode { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
    }

    public enum ErrorCode
    {
        BadRequest = 400,
        NotFound = 404,
        Forbidden = 403,
        NoContent = 204,
        InternalError = 500,
    }
}
