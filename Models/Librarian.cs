using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Models
{
    public class Librarian
    {
        public int LibrarianId { get; set; }

        [Required(ErrorMessage = "Librarian name is required.")]
        [StringLength(100)]
        [Display(Name = "Librarian Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Age is required.")]
        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Please enter a valid phone number.")]
        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; } = string.Empty;
    }
}