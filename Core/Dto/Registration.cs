using System.ComponentModel.DataAnnotations;

namespace Bitly.Core.Dto
{
    public class Registration
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords don't match")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
