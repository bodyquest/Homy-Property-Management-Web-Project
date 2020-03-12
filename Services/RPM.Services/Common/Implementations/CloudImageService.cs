namespace RPM.Services.Common.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using RPM.Data;

    using static RPM.Common.GlobalConstants;

    public class CloudImageService : ICloudImageService
    {
        private readonly Cloudinary cloudUtility;
        private readonly ApplicationDbContext context;

        public CloudImageService(Cloudinary cloudUtility, ApplicationDbContext context)
        {
            this.context = context;
            this.cloudUtility = cloudUtility;
        }

        public async Task<ImageUploadResult> UploadImageAsync(IFormFile imageFile)
        {
            byte[] destinationData;

            using (var ms = new MemoryStream())
            {
                await imageFile.CopyToAsync(ms);
                destinationData = ms.ToArray();
            }

            var fileName = imageFile.FileName;
            //int index = fileName.LastIndexOf('.');
            //var trueFileName = index == -1 ? fileName : fileName.Substring(0, index);

            using (var ms = new MemoryStream(destinationData))
            {
                ImageUploadParams uploadParams = new ImageUploadParams
                {
                    Folder = CloudFolder,
                    File = new FileDescription(fileName, ms),
                    PublicId = $"{PubPrefix}_{Guid.NewGuid().ToString().Substring(0, 5)}",
                    Transformation = new Transformation().Crop(Limit).Width(ImgWidth).Height(ImgHeight),
                };

                var uploadResult = await this.cloudUtility.UploadAsync(uploadParams);

                return uploadResult;
            }
        }

        public string GetImageUrl(string imagePublicId)
        {
            string imageUrl = this.cloudUtility
                                      .Api
                                      .UrlImgUp
                                      .BuildUrl(string.Format(GetImageUrlFormat, imagePublicId));

            return imageUrl;
        }

        public string GetThumbnailUrl(string imagePublicId)
        {
            var imageUrl = this.cloudUtility
                                 .Api
                                 .UrlImgUp
                                 .Transform(new Transformation()
                                     .Height(200)
                                     .Width(200)
                                     .Crop(Thumb))
                                 .BuildUrl(string.Format(GetImageUrlFormat, imagePublicId));

            return imageUrl;
        }

        public string GetProfilePic (string imagePublicId)
        {
            var imageUrl = this.cloudUtility
                               .Api
                               .UrlImgUp
                               .Transform(new Transformation()
                                   .Radius(RadiusMax).Chain()
                                   .Height(200)
                                   .Width(200)
                                   .Gravity(GravityFaces)
                                   .Crop(Fill))
                                   .BuildUrl(string.Format(GetImageUrlFormat, imagePublicId));

            return imageUrl;
        }

        public async Task DeleteImages(params string[] publicIds)
        {
            var delParams = new DelResParams
            {
                PublicIds = publicIds.ToList(),
                Invalidate = true,
            };

            await this.cloudUtility.DeleteResourcesAsync(delParams);
        }
    }
}
