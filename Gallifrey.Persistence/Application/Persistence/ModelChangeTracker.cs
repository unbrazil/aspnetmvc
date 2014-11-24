namespace Gallifrey.Persistence.Application.Persistence
{
    public sealed class ModelChangeTracker<TModel>
    {
        public TModel Old { set; get; }

        public TModel New { set; get; }
    }
}