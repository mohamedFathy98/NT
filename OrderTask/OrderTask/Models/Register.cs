using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OrderTask.Models
{
    public class Register
    {
        [Required(ErrorMessage = "First Name is Required")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is Required")]
        public string LastName { get; set; }
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(Password) , ErrorMessage="Password And Comnfirm Password Don't Match")]
        public string ConfirmPassword { get; set; }
        public bool IsAgree { get; set; }
    }
}
