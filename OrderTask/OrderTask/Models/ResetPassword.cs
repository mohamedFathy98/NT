using System.ComponentModel.DataAnnotations;

namespace OrderTask.Models
{
    public class ResetPassword
    {
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password And Comnfirm Password Don't Match")]
        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
        public string Token { get; set; }
    }
}
