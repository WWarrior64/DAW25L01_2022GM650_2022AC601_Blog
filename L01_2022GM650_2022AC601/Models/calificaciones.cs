using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace L01_2022GM650_2022AC601.Models
{
    public class calificaciones
    {
        [Key]
        public int calificacionId { get; set; }

        public int? publicacionId { get; set; }
       
        public int? usuarioId { get; set; }
      
        public int calificacion { get; set; }
    }
}
