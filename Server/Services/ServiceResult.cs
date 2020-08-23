using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CheckNote.Server.Services
{
    public class ServiceResult<T> : ObjectResult
    {
        public new T Value
        {
            get => (T)base.Value;
            protected set => base.Value = value;
        }

        public new int StatusCode
        {
            get => (int)base.StatusCode;
            protected set => base.StatusCode = value;
        }

        public string Message { get; set; }

        public ServiceResult() : base(null)
        { }

        protected ServiceResult<T> Assign(int statusCode, T value, string message)
        {
            StatusCode = statusCode;
            Message = message;
            Value = value;
            return this;
        }

        public ServiceResult(int statusCode, T value = default, string message = null) : base(value)
            => Assign(statusCode, value, message);

        public ServiceResult<T> Ok(T value = default, string message = null)
            => Assign(StatusCodes.Status200OK, value, message);

        public ServiceResult<T> NotFound(string message = null)
            => Assign(StatusCodes.Status404NotFound, default, message);

        public ServiceResult<T> BadRequest(string message = null)
            => Assign(StatusCodes.Status400BadRequest, default, message);

        public ServiceResult<T> Conflict(string message = null)
            => Assign(StatusCodes.Status409Conflict, default, message);

        public ServiceResult<T> Unauthorized(string message = null)
            => Assign(StatusCodes.Status401Unauthorized, default, message);
    }

    public class ServiceResult<TEntity, TModel> : ServiceResult<TModel>
        where TEntity : ICheckNoteModel<TModel>
        where TModel : class
    {
        public TEntity Entity { get; private set; }

        public ServiceResult()
        { }

        public ServiceResult(int statusCode, TEntity value = default, string message = null)
            : base(statusCode, default, message)
        {
            Entity = value;
        }

        public ServiceResult<TEntity, TModel> Ok(TEntity value = default, string message = null)
        {
            Ok(null, message);
            Entity = value;
            return this;
        }
        public new ServiceResult<TEntity, TModel> NotFound(string message = null)
        {
            base.NotFound(message);
            return this;
        }
        public new ServiceResult<TEntity, TModel> Conflict(string message = null)
        {
            base.Conflict(message);
            return this;
        }
        public new ServiceResult<TEntity, TModel> BadRequest(string message = null)
        {
            base.BadRequest(message);
            return this;
        }
        public new ServiceResult<TEntity, TModel> Unauthorized(string message = null)
        {
            base.Unauthorized(message);
            return this;
        }
        public ServiceResult<TEntity, TModel> Sanitize()
        {
            if (Entity != null) Value = Entity.Sanitize();

            return this;
        }
    }

    public class ServiceResult : ServiceResult<object>
    {
        protected ServiceResult SetOk(string message)
        {
            base.Ok(message);
            return this;
        }
        protected ServiceResult SetNotFound(string message)
        {
            base.NotFound(message);
            return this;
        }
        protected ServiceResult SetBadRequest(string message)
        {
            base.BadRequest(message);
            return this;
        }
        protected ServiceResult SetConflict(string message)
        {
            base.Conflict(message);
            return this;
        }
        protected ServiceResult SetUnauthorized(string message)
        {
            base.Unauthorized(message);
            return this;
        }

        public static ServiceResult Ok(string message = null)
            => new ServiceResult().SetOk(message);

        public new static ServiceResult NotFound(string message = null)
            => new ServiceResult().SetNotFound(message);

        public new static ServiceResult BadRequest(string message = null)
            => new ServiceResult().SetBadRequest(message);

        public new static ServiceResult Conflict(string message = null)
            => new ServiceResult().SetConflict(message);

        public new static ServiceResult Unauthorized(string message = null)
            => new ServiceResult().SetUnauthorized(message);
    }

    public class OkServiceResult : ServiceResult
    {
        public OkServiceResult(string message = null) => SetOk(message);
    }

    public class NotFoundServiceResult : ServiceResult
    {
        public NotFoundServiceResult(string message = null) => SetNotFound(message);
    }
    public class BadRequestServiceResult : ServiceResult
    {
        public BadRequestServiceResult(string message = null) => SetBadRequest(message);
    }
    public class ConflictServiceResult : ServiceResult
    {
        public ConflictServiceResult(string message = null) => SetConflict(message);
    }
    public class UnauthorizedServiceResult : ServiceResult
    {
        public UnauthorizedServiceResult(string message = null) => SetUnauthorized(message);
    }

    public class ListServiceResult<TEntity, TModel> : ServiceResult<List<TModel>>
        where TEntity : ICheckNoteModel<TModel>
    {
        public List<TEntity> Entities { get; private set; }

        public ListServiceResult()
        { }

        public ListServiceResult(int statusCode, List<TEntity> value = default, string message = null)
            : base(statusCode, default, message)
        {
            Entities = value;
        }

        public ListServiceResult<TEntity, TModel> Ok(List<TEntity> value = default, string message = null)
        {
            base.Ok(null, message);
            Entities = value;
            return this;
        }
        public new ListServiceResult<TEntity, TModel> NotFound(string message = null)
        {
            base.NotFound(message);
            return this;
        }
        public new ListServiceResult<TEntity, TModel> Conflict(string message = null)
        {
            base.Conflict(message);
            return this;
        }
        public new ListServiceResult<TEntity, TModel> BadRequest(string message = null)
        {
            base.BadRequest(message);
            return this;
        }
        public new ListServiceResult<TEntity, TModel> Unauthorized(string message = null)
        {
            base.Unauthorized(message);
            return this;
        }
        public ListServiceResult<TEntity, TModel> Sanitize()
        {
            Value = Entities?.Select(e => e.Sanitize()).ToList();

            return this;
        }
    }
}
