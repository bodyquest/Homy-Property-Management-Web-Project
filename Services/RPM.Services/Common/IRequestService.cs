namespace RPM.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using RPM.Data.Models;
    using RPM.Services.Common.Models.Request;

    public interface IRequestService
    {
        Task<RequestFormServiceModel> GetFormDataAsync(string id, User user);
    }
}
