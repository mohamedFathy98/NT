using System.ComponentModel.DataAnnotations;

namespace OrderTask.Models
{
    public class Login 
    {
        [Required(ErrorMessage = "Password is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
