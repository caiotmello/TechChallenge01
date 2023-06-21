using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace ImageUploaderWebMVC.Models
{
    public class Image
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
        public Uri? Url { get; set; }

        public string? BlobName { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

    }
}
