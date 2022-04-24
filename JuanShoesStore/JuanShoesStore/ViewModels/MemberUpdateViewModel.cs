using System.ComponentModel.DataAnnotations;

namespace JuanShoesStore.ViewModels
{
    public class MemberUpdateViewModel
    {
        [Required]
        [StringLength(maximumLength: 30)]
        public string UserName { get; set; }
        [Required]
        [StringLength(maximumLength: 50)]
        public string FullName { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string Email { get; set; }
        [StringLength(maximumLength: 15)]
        public string Phone { get; set; }  
        [StringLength(maximumLength:50)]
        public string Country { get; set; }
        [StringLength(maximumLength: 30)]
        public string City { get; set; }
        [StringLength(maximumLength: 150)]
        public string Address { get; set; }

        [StringLength(maximumLength: 30)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [StringLength(maximumLength: 30)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [StringLength(maximumLength: 30)]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string ConfirmPassword { get; set; }
    }
}
