using System.ComponentModel.DataAnnotations;

namespace EatNow.Models
{
    public class Empleado
    {
        [Required]
        public int IdEmpleado { get; set; }

        [Required]
        [StringLength(9)]
        public string DNI { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(100)]
        public string CorreoElectronico { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        public int RIdRestaurante { get; set; }
    }
}
