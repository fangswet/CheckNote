
using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CheckNote.Server.Services
{
    public class ServiceResult<T>
    {
        public T Value { get; protected set; }
        protected bool hasValue = false;
        public int StatusCode { get; protected set; }
        public string Message { get; protected set; }
        public ServiceResult<T> Set(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message;
            return this;
        }
        public ServiceResult<T> SetValue(int statusCode, T value, string message = null)
        {
            Value = value;
            hasValue = true;
            return Set(statusCode, message);
        }

        public ServiceResult()
        { }
        public ServiceResult(int statusCode, string message = null) => Set(statusCode, message);
        public ServiceResult(int statusCode, string message = null, T value = default) => SetValue(statusCode, value, message);
        public ActionResult Result()
        {
            if (!hasValue) return new StatusCodeResult(StatusCode);

            return new ObjectResult(Value)
            {
                StatusCode = StatusCode
            };
        }
        public ServiceResult<T> Ok() => Set(StatusCodes.Status200OK);
        public ServiceResult<T> Ok(T value)
        {
            Ok();
            return SetValue(StatusCode, value);
        }
        public ServiceResult<T> NotFound(string message = null) => Set(StatusCodes.Status404NotFound, message);
        public ServiceResult<T> BadRequest(string message = null) => Set(StatusCodes.Status400BadRequest, message);
        public ServiceResult<T> Conflict(string message = null) => Set(StatusCodes.Status409Conflict, message);
        public ServiceResult<T> Unauthorized(string message = null) => Set(StatusCodes.Status401Unauthorized, message);

        public bool IsOk { get => StatusCode == StatusCodes.Status200OK; }
        public bool IsNotFound { get => StatusCode == StatusCodes.Status404NotFound; }
    }

    public class ServiceResult<TEntity, TModel> : ServiceResult<TModel>
        where TEntity : ICheckNoteEntity<TModel>
        where TModel : class
    {
        public TEntity Entity { get; private set; }
        public ServiceResult<TEntity, TModel> SetValue(int statusCode, TEntity entity, string message = null)
        {
            Set(statusCode, message);
            Entity = entity;
            return this;
        }
        public ServiceResult()
        { }
        public ServiceResult(int statusCode, string message = null, TEntity entity = default) => SetValue(statusCode, entity, message);
        public new ActionResult Result()
        {
            if (Entity == null) return base.Result();
            
            return new ObjectResult(Entity)
            {
                StatusCode = StatusCode
            };
        }
        public ActionResult SanitizeResult()
        {
            if (Entity != null) 
                SetValue(StatusCode, Entity.Sanitize(), Message);

            return base.Result();
        }
        public ServiceResult<TEntity, TModel> Ok(TEntity entity = default)
        {
            base.Ok();
            return SetValue(StatusCode, entity);
        }
        public new ServiceResult<TEntity, TModel> NotFound(string message = null)
        {
            base.NotFound(message);
            return this;
        }
        public new ServiceResult<TEntity, TModel> BadRequest(string message = null)
        {
            base.BadRequest(message);
            return this;
        }
        public new ServiceResult<TEntity, TModel> Conflict(string message = null)
        {
            base.Conflict(message);
            return this;
        }
        public new ServiceResult<TEntity, TModel> Unauthorized(string message = null)
        {
            base.Unauthorized(message);
            return this;
        }
    }
    public class ServiceResult : ServiceResult<object>
    {
        private new object Value { get; set; }
        public ServiceResult()
        { }
        public ServiceResult(int statusCode, string message = null) : base(statusCode, message)
        { }
        public static OkServiceResult Ok(string message = null) => new OkServiceResult(message);
        public new static NotFoundServiceResult NotFound(string message = null) => new NotFoundServiceResult(message);
        public new static BadRequestServiceResult BadRequest(string message = null) => new BadRequestServiceResult(message);
        public new static ConflictServiceResult Conflict(string message = null) => new ConflictServiceResult(message);
        public new static UnauthorizedServiceResult Unauthorized(string message = null) => new UnauthorizedServiceResult(message);
    }

    public class OkServiceResult : ServiceResult
    {
        public OkServiceResult(string message = null) : base(StatusCodes.Status200OK, message)
        { }
    }
    public class NotFoundServiceResult : ServiceResult
    {
        public NotFoundServiceResult(string message = null) : base(StatusCodes.Status404NotFound, message)
        { }
    }
    public class BadRequestServiceResult : ServiceResult
    {
        public BadRequestServiceResult(string message = null) : base(StatusCodes.Status400BadRequest, message)
        { }
    }
    public class ConflictServiceResult : ServiceResult
    {
        public ConflictServiceResult(string message = null) : base(StatusCodes.Status409Conflict, message)
        { }
    }
    public class UnauthorizedServiceResult : ServiceResult
    {
        public UnauthorizedServiceResult(string message = null) : base(StatusCodes.Status401Unauthorized, message)
        { }
    }
}
