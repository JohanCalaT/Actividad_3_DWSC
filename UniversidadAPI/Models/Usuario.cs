using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversidadAPI.Models
{
    [Table("sampleusers")]
    public class Usuario
    {
        [Key]
        [StringLength(50)]
        [Column("username")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Column("dni")]
        public string Dni { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Column("surnames")]
        public string Surnames { get; set; } = string.Empty;

        [Required]
        [Range(0, 150)]
        [Column("age")]
        public int Age { get; set; }
    }
}
