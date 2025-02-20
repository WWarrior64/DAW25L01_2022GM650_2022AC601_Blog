using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace L01_2022GM650_2022AC601.Models
{
    public class usuarios
    {
        [Key]
        public int usuarioId { get; set; }
        public int? rolId { get; set; }

        [Required]
        [MaxLength(50)]
        public string nombreUsuario { get; set; }

        [Required]
        [MaxLength(50)]
        public string clave { get; set; }

        [Required]
        [MaxLength(100)]
        public string nombre { get; set; }

        [Required]
        [MaxLength(100)]
        public string apellido { get; set; }
    }
}
