using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckNote.Server.Services
{
    public class ServiceResult<T> : ObjectResult
    {
        public new T Value { get; private set; }
        public new int StatusCode { get; private set; }
        public string Message { get; private set; }

        public ServiceResult(): base(null)
        { }

        public ServiceResult(int statusCode, T value = default, string message = null) : base(value)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ServiceResult<T> Ok(T value = default, string message = null) 
            => new ServiceResult<T>(StatusCodes.Status200OK, value, message);

        public ServiceResult<T> NotFound(string message = null)
            => new ServiceResult<T>(StatusCodes.Status404NotFound, default, message);

        public ServiceResult<T> Conflict(string message = null)
            => new ServiceResult<T>(StatusCodes.Status409Conflict, default, message);

        public ServiceResult<T> BadRequest(string message = null)
            => new ServiceResult<T>(StatusCodes.Status400BadRequest, default, message);

        public ServiceResult<T> Unauthorized(string message = null)
            => new ServiceResult<T>(StatusCodes.Status401Unauthorized, default, message);
    }

    public class ServiceResult : ServiceResult<object>
    {
        public ServiceResult Ok(string message = null) => base.Ok(message) as ServiceResult;
        public new ServiceResult NotFound(string message = null) => base.NotFound(message) as ServiceResult;
        public new ServiceResult Conflict(string message = null) => base.Conflict(message) as ServiceResult;
        public new ServiceResult BadRequest(string message = null) => base.BadRequest(message) as ServiceResult;
        public new ServiceResult Unauthorized(string message = null) => base.Unauthorized(message) as ServiceResult;
    }

    public class ServiceResult<TEntity, TModel> : ServiceResult<TEntity>
        where TEntity : ICheckNoteModel<TModel> // swap the rquirement so the model is implied here
        where TModel : class
    {
        public new ServiceResult<TEntity, TModel> Ok(TEntity value, string message = null) 
            => base.Ok(value, message) as ServiceResult<TEntity, TModel>;
        public new ServiceResult<TEntity, TModel> NotFound(string message = null) 
            => base.NotFound(message) as ServiceResult<TEntity, TModel>;
        public new ServiceResult<TEntity, TModel> Conflict(string message = null)
            => base.NotFound(message) as ServiceResult<TEntity, TModel>;
        public new ServiceResult<TEntity, TModel> BadRequest(string message = null)
            => base.NotFound(message) as ServiceResult<TEntity, TModel>;
        public new ServiceResult<TEntity, TModel> Unauthorized(string message = null)
            => base.NotFound(message) as ServiceResult<TEntity, TModel>;

        public TModel Sanitize()
        {
            if (Value != null) return Value.Sanitize();

            return null;
        }
    }
}
