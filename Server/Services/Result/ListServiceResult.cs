using CheckNote.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CheckNote.Server.Services
{
    public class ListServiceResult<TEntity, TModel> : ServiceResult<IList<TModel>>
        where TEntity : ICheckNoteEntity<TModel>
        where TModel : class
    {
        public IList<TEntity> Entities { get; private set; }
        public ListServiceResult<TEntity, TModel> SetValue(int statusCode, IList<TEntity> entities, string message = null)
        {
            Set(statusCode, message);
            Entities = entities;
            return this;
        }
        public ListServiceResult()
        { }
        public ListServiceResult(int statusCode, string message = null, IList<TEntity> entities = default) => SetValue(statusCode, entities, message);
        public new ActionResult Result()
        {
            if (Entities == null) return base.Result();

            return new ObjectResult(Entities)
            {
                StatusCode = StatusCode
            };
        }
        public ActionResult SanitizeResult()
        {
            if (Entities != null)
                SetValue(StatusCode, Entities.Select(e => e.Sanitize()).ToList(), Message);

            return base.Result();
        }
        public ListServiceResult<TEntity, TModel> Ok(IList<TEntity> entities = default)
        {
            base.Ok();
            return SetValue(StatusCode, entities);
        }
        public new ListServiceResult<TEntity, TModel> NotFound(string message = null)
        {
            base.NotFound(message);
            return this;
        }
        public new ListServiceResult<TEntity, TModel> BadRequest(string message = null)
        {
            base.BadRequest(message);
            return this;
        }
        public new ListServiceResult<TEntity, TModel> Conflict(string message = null)
        {
            base.Conflict(message);
            return this;
        }
        public new ListServiceResult<TEntity, TModel> Unauthorized(string message = null)
        {
            base.Unauthorized(message);
            return this;
        }
    }
}
