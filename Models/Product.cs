using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductAdmin.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product category is required.")]
        [Display(Name = "Product Category")]
        public string ProductCategory { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product freshness is required.")]
        [Display(Name = "Product Freshness")]
        public string ProductFreshness { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product image is required.")]
        public string ImagePath { get; set; } = string.Empty;

        [Required(ErrorMessage = "Additional description is required.")]
        [Display(Name = "Additional Description")]
        public string AdditionalDescription { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product price is required.")]
        [Range(0.01, 999999999, ErrorMessage = "Enter a valid product price.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Product Price")]
        public decimal ProductPrice { get; set; }

        [Required(ErrorMessage = "MRP is required.")]
        [Range(0.01, 999999999, ErrorMessage = "Enter a valid MRP.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MRP { get; set; }

        [Required(ErrorMessage = "Comments are required.")]
        public string Comments { get; set; } = string.Empty;

        [Required(ErrorMessage = "Manager email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email address.")]
        [Display(Name = "Manager Email")]
        public string ManagerEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "Manager phone number is required.")]
        [RegularExpression(@"^\+?[0-9]{10,15}$",
            ErrorMessage = "Phone number must contain 10 to 15 digits.")]
        [Display(Name = "Manager Phone Number")]
        public string ManagerPhoneNumber { get; set; } = string.Empty;
    }
}