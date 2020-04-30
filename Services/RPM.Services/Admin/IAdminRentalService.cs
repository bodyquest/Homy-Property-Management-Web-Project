namespace RPM.Services.Admin
{
    using System.Threading.Tasks;

    public interface IAdminRentalService
    {
        Task<int> GetRentalsCount();
    }
}
