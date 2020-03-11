namespace RPM.Services.Common.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using RPM.Data;
    using RPM.Data.Models;

    public class ImageDbService : IImageDbService
    {
        private readonly ApplicationDbContext context;

        public ImageDbService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> WriteToDatabasebAsync(string imageUrl, string imagePublicId)
        {
            var image = new CloudImage
            {
                PictureUrl = imageUrl,
                PicturePublicId = imagePublicId,
            };

            await this.context.CloudImages.AddAsync(image);

            await this.context.SaveChangesAsync();

            return image.Id;
        }

        public string GetPublicId(int id)
        {
            var image = this.context.CloudImages.FirstOrDefault(i => i.Id == id);
            var pubId = image.PicturePublicId;

            return pubId;
        }
    }
}
