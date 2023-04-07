using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proizvodii.Entity
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoleId { get; set; }

        public string Name { get; set; } = "";

        public bool Active { get; set; }

        public IEnumerable<UserRole> UserRole { get; set; } = new List<UserRole>();
    }
}
