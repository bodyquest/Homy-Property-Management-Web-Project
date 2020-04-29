namespace RPM.Services.Tests.Seed
{
    using RPM.Data.Models;

    public class ImageCreator
    {
        private const string DefaultUrl = "https://res.cloudinary.com/dotnetdari/image/upload/v1587205946/Homy/house_1_ovyvnj.jpg";
        private static int id;

        public static CloudImage CreateForModel()
        {
            return new CloudImage
            {
                Id = ++id,
                PictureUrl = DefaultUrl,
            };
        }
    }
}
