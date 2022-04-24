using System.ComponentModel.DataAnnotations;

namespace JuanShoesStore.Areas.Manage.ViewModels
{
    public class AdminAddRoleViewModel
    {
        [Required]
        [StringLength(maximumLength: 30)]
        public string UserName { get; set;}
        [Required]
        [StringLength(maximumLength: 100)]
        public string Email { get; set;}
        [Required]
        [StringLength(maximumLength: 50)]
        public string FullName { get; set; }
        [Required]
        [StringLength(maximumLength: 30)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [StringLength(maximumLength: 30)]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; }


    }
}
