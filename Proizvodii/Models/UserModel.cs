using Proizvodii.Attribute;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Proizvodii.Models
{
    public class UserModel
    {
        [Required(ErrorMessage = "UserId je obavezan")]
        public int UserId { get; set; }

        [DisplayName("Ime")]
        [Required(ErrorMessage = "Ime je obavezno")]
        public string FirstName { get; set; } = "";

        [DisplayName("Prezime")]
        [Required(ErrorMessage = "Prezime je obavezno")]
        public string LastName { get; set; } = "";

        [DisplayName("Korisnicko ime")]
        [Required(ErrorMessage = "Korisnicko ime je obavezno")]
        public string Username { get; set; } = "";

        [DisplayName("Lozinka")]
        [DataType("Password")]
        [RequiredIf("UserId", "0", ErrorMessage = "Lozinka je obavezan")]
        public string? Password { get; set; } = "";

        [DisplayName("Mejl")]
        [Required(ErrorMessage = "Mejl je obavezan")]
        [EmailAddress(ErrorMessage = "Format mejla nije ispravan")]
        public string Email { get; set; } = "";

        [DisplayName("Aktivan")]
        public bool Active { get; set; }

        [RequiredRoles(ErrorMessage = "Korisnik mora da ima minimum jednu ulogu")]
        public List<RoleModel> Roles { get; set; } = new List<RoleModel>();

        [DisplayName("Potvrda lozine")]
        [DataType("Password")]
        [Compare("Password", ErrorMessage = "Lozinka i potvrda lozinke se ne poklapaju.")]
        public string? ConfirmPassword { get; set; } = "";
    }

    public class RoleModel
    {
        public int RoleId { get; set; }
        public bool Selected { get; set; }
        public string RoleName { get; set; } = "";
    }
}

