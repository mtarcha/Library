namespace Library.Domain
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Create();
    }
}