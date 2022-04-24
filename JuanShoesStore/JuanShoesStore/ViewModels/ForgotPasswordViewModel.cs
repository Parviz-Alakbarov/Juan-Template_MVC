using System.ComponentModel.DataAnnotations;

namespace JuanShoesStore.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

}
