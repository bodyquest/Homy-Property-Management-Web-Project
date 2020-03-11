namespace RPM.Services.Common
{
    using System.Threading.Tasks;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public interface ICloudImageService
    {
        Task<ImageUploadResult> UploadImageAsync(IFormFile imageFile);

        string GetImageUrl(string imagePublicId);

        string GetThumbnailUrl(string imagePublicId);

        string GetProfilePic(string imagePublicId);

        Task DeleteImages(params string[] publicIds);
    }
}
