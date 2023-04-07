using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proizvodii.Entity
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required(ErrorMessage = "ProductId je obavezan")]
        public int ProductId { get; set; }

        [DisplayName("Naziv")]
        [Required(ErrorMessage = "Naziv je obavezan")]
        [MaxLength(200)]
        public string Name { get; set; } = "";

        [DisplayName("Šifra")]
        [Required(ErrorMessage = "Sifra je obavezan")]
        [MaxLength(20, ErrorMessage = "Maksimalno 20 karaktera za sifru")]
        public string Code { get; set; } = "";

        [DisplayName("Cena")]
        [Required(ErrorMessage = "Cena je obavezna")]
        [Range(0, Double.PositiveInfinity, ErrorMessage = "Cena ne sme da bude manja od 0")]
        public decimal Price { get; set; }

        [DisplayName("Opis")]
        public string Description { get; set; } = "";

        [DisplayName("Kategorija")]
        [Required(ErrorMessage = "Kategorija je obavezna")]
        public Category? Category { get; set; } = new Category();

        [DisplayName("Slika")]
        public string? ImageName { get; set; }

        [DisplayName("Aktivan")]
        public bool Active { get; set; }

        [NotMapped]
        public string ImagePath => "/images/" + ImageName;

        [NotMapped]
        public IFormFile? NewImage { set; get; }
    }
}
