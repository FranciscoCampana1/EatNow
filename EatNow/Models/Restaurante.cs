using System.ComponentModel.DataAnnotations;

namespace EatNow.Models
{
    public class Restaurante
    {
        [Required]
        public int IdRestaurante { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Direccion { get; set; }

        [Required]
        [StringLength(11)]
        public string Telefono { get; set; }

        [StringLength(200)]
        public string? Web { get; set; }

        [StringLength(200)]
        public string? Descripcion { get; set; }

        [Required]
        public string HoraApertura { get; set; }

        [Required]
        public string HoraCierre { get; set; }

        // Aquí se guardará la URL de la primera imagen del restaurante (No existe este campo en la base de datos)
        public string URLImagen { get; set; }
    }
}
