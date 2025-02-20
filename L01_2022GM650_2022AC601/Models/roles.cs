using System.ComponentModel.DataAnnotations;

namespace L01_2022GM650_2022AC601.Models
{
    public class roles
    {
        
            [Key]
            public int rolId { get; set; }

            [Required]
            [MaxLength(100)]
            public string rol { get; set; }
        
    }

}
