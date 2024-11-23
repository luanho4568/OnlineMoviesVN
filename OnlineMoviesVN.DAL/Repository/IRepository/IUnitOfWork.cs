
namespace OnlineMoviesVN.DAL.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IProvinceRepository Province { get; }
        IDistrictRepository District { get; }
        IWardRepository Ward { get; }
        IUserRepository User { get; }
        IContactUsRequestRepository ContactUsRequest { get; }
        IUserActivityLogRepository UserActivityLog { get; }
        Task SaveAsync();
    }
}
