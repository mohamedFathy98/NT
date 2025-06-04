using System.ComponentModel.DataAnnotations;

namespace OrderTask.Models
{
    public class ForgetPassword
    {
        [EmailAddress]
        public string Email { get; set; }
        
    }
}
