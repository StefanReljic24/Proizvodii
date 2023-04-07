using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Proizvodii.Models
{
    public class LoginModel
    {
        [DisplayName("Korisnicko ime")]
        [Required(ErrorMessage = "Korisnicko ime je obavezno")]
        public string Username { get; set; } = "";


        [DisplayName("Lozinka")]
        [DataType("Password")]
        [Required(ErrorMessage = "Lozinka je obavezno")]
        public string Password { get; set; } = "";

        [DisplayName("Zapamti me")]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; } = "";
    }
}
