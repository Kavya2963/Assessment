using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CurtainStoreManagement.Models
{
    public class Curtain
    {
        [Key]
        public int CurtainID { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public int StockQuantity { get; set; } = 0;

        [MaxLength(50)]
        public string? Material { get; set; }

        [MaxLength(30)]
        public string? Color { get; set; }
    }
}
