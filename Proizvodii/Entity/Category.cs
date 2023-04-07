using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proizvodii.Entity
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "CategoryId je obavezan")]
        public int CategoryId { get; set; }

        [DisplayName("Naziv kategorije")]
        [Required(ErrorMessage = "Naziv je obavezan")]
        [MaxLength(200)]
        public string Name { get; set; } = "";

        [DisplayName("Šifra kategorije")]
        [Required(ErrorMessage = "Sifra je obavezan")]
        [MaxLength(20, ErrorMessage = "Maksimalno 20 karaktera za sifru")]
        public string Code { get; set; } = "";

        [DisplayName("Aktivna")]
        public bool Active { get; set; }

        public IEnumerable<Product> Products { get; set; } = new List<Product>();
    }
}
