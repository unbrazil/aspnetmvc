namespace Gallifrey.Persistence.Application.Persistence
{
    public interface IIdentity<T>
    {
        T Id { get; set; }
    }
}