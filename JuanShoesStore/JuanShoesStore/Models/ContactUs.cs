using System.ComponentModel.DataAnnotations;

namespace JuanShoesStore.Models
{
    public class ContactUs
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 15)]
        public string PhoneNumber { get; set; }
        [Required]
        [StringLength(maximumLength: 25)]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 40)]
        public string Subject { get; set; }
        [Required]
        [StringLength(maximumLength: 500)]
        public string Message { get; set; }
    }
}
