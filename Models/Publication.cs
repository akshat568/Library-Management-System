using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public enum PublicationType
    {
        Newspaper = 0,
        Magazine = 1
    }

    public class Publication
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Publisher is required.")]
        [StringLength(100)]
        public string Publisher { get; set; } = string.Empty;

        [Required(ErrorMessage = "Published date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Published Date")]
        public DateTime PublishedDate { get; set; }

        [Required]
        public PublicationType Type { get; set; }

        [Display(Name = "Available")]
        public bool IsAvailable { get; set; } = true;
    }
}