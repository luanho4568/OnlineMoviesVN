
namespace OnlineMoviesVN.DAL.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IContactUsRequestRepository ContactUsRequest { get; }
        Task SaveAsync();
    }
}
