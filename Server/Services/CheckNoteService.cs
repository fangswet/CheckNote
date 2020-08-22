using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CheckNote.Server.Services
{
    public class ServiceResult<T> : ObjectResult
    {
        public new T Value { get; private set; }
        public new int StatusCode { get; private set; }
        public string Message { get; private set; }

        public ServiceResult(int statusCode, T value) : base(value)
        {
            StatusCode = statusCode;
        }

        public ServiceResult(int statusCode, string message) : base(default)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }

    public class ServiceResult : ServiceResult<object>
    {
        public ServiceResult(int statusCode, string message) : base(statusCode, message)
        { }
    }

    public class CheckNoteService
    {
        protected ServiceResult<T> Ok<T>(T value = default) 
            => new ServiceResult<T>(StatusCodes.Status200OK, value);
        protected ServiceResult Ok()
            => new ServiceResult(StatusCodes.Status200OK, null);

        protected ServiceResult<T> NotFound<T>(string message = null)
            => new ServiceResult<T>(StatusCodes.Status404NotFound, message);
        protected ServiceResult NotFound(string message = null)
            => new ServiceResult(StatusCodes.Status404NotFound, message);

        protected ServiceResult<T> Conflict<T>(string message = null)
            => new ServiceResult<T>(StatusCodes.Status409Conflict, message);
        protected ServiceResult Conflict(string message = null)
            => new ServiceResult(StatusCodes.Status409Conflict, message);

        protected ServiceResult<T> Unauthorized<T>(string message = null)
            => new ServiceResult<T>(StatusCodes.Status401Unauthorized, message);
        protected ServiceResult Unauthorized(string message = null)
            => new ServiceResult(StatusCodes.Status401Unauthorized, message);

        protected ServiceResult<T> BadRequest<T>(string message = null)
            => new ServiceResult<T>(StatusCodes.Status401Unauthorized, message);
        protected ServiceResult BadRequest(string message = null)
            => new ServiceResult(StatusCodes.Status401Unauthorized, message);
    }
}
