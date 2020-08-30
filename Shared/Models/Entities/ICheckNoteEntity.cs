namespace CheckNote.Shared.Models
{
    public interface ICheckNoteEntity
    { }

    public interface ICheckNoteEntity<TModel> : ICheckNoteEntity
    {
        public TModel Sanitize();
    }

    public interface ICheckNoteEntity<TEntity, TModel> : ICheckNoteEntity<TModel>
    {
        public TEntity Qualify();
    }
}