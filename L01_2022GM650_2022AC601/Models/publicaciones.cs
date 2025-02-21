﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace L01_2022GM650_2022AC601.Models
{
    public class publicaciones
    {
        [Key]
        public int publicacionId { get; set; }
        public required string titulo { get; set; }
        public required string descripcion { get; set; }
        public int? usuarioId { get; set; }
    }
}
