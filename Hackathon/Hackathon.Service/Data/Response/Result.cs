using System;
using System.Collections.Generic;

namespace Hackathon.Service.Data.Response
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public bool IsException { get; set; }

        public IEnumerable<ErrorModel> Errors { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public T Value { get; set; }

        public static Result<T> CreateSuccess(T value)
        {
            var resultSuccess = new Result<T>
            {
                IsSuccess = true,
                Value = value
            };

            return resultSuccess;
        }

        public static Result<T> CreateError(IEnumerable<ErrorModel> errorList)
        {
            var resultError = new Result<T>
            {
                IsSuccess = false,
                Errors = errorList
            };

            return resultError;
        }

        public static Result<T> CreateErrorMessage(string errorMessage)
        {
            var resultError = new Result<T>
            {
                IsSuccess = false,
                Message = errorMessage
            };

            return resultError;
        }

        public static Result<T> CreateError(ErrorModel error)
        {
            var resultError = new Result<T>
            {
                IsSuccess = false,
                Errors = new List<ErrorModel>
                {
                    error
                }
            };

            return resultError;
        }

        public static Result<T> CreateException(Exception exception)
        {
            var resultException = new Result<T>
            {
                IsSuccess = false,
                IsException = true,
                Exception = exception.Message
            };

            return resultException;
        }

    }
}
