using System.ComponentModel.DataAnnotations;

namespace TheDAF.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "You need to enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
