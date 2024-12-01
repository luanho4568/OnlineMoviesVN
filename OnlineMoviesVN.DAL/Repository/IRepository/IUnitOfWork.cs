
namespace OnlineMoviesVN.DAL.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IContactUsRequestRepository ContactUsRequest { get; }
        IMovieRepository Movie { get; }
        Task SaveAsync();
    }
}
