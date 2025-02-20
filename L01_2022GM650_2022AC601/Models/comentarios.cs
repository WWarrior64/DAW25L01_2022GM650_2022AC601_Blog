using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace L01_2022GM650_2022AC601.Models
{
    public class comentarios
    {
        [Key]
        public int comentarioId { get; set; }

        public int? publicacionId { get; set; }

        public string comentario { get; set; }

        public int? usuarioId { get; set; }
 
    }
}
