namespace RPM.Services.Common
{
    using System.Threading.Tasks;
    using RPM.Services.Common.Models.Request;

    public interface IRequestService
    {
        Task<bool> CreateRequestAsync(RequestCreateServiceModel model);
    }
}
