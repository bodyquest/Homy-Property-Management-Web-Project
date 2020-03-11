namespace RPM.Services.Common
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public interface IImageDbService
    {
        Task<int> WriteToDatabasebAsync(string imageUrl, string imagePublicId);

        string GetPublicId(int id);
    }
}
