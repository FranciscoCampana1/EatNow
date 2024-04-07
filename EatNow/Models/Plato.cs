using System.ComponentModel.DataAnnotations;

namespace EatNow.Models
{
    public class Plato
    {
        [Required]
        public int IdPlato { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        public double Precio { get; set; }

        [Required]
        public int RIdRestaurante { get; set; }

        public string URLImagen { get; set; }
    }
}
