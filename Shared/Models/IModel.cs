namespace CheckNote.Shared.Models
{
    public interface ICheckNoteModel
    { }

    public interface ICheckNoteModel<TModel> : ICheckNoteModel
    {
        public TModel Sanitize();
    }
}
