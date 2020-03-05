namespace RPM.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class CloudImage
    {
        [Key]
        public int Id { get; set; }

        public string PicturePublicId { get; set; }

        public string PictureUrl { get; set; }

        public string PictureThumbnailUrl { get; set; }

        public long Length { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
