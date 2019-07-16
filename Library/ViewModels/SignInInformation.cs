using System.ComponentModel.DataAnnotations;

namespace Library.ViewModels
{
    public class SignInInformation
    {
        [Required]
        [RegularExpression(@"/d+")]
        public string PhoneNumber { get; set; }
       
        [Required]
        public string Password { get; set; }
    }
}