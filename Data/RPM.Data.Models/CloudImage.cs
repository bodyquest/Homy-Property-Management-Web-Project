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
    }
}
