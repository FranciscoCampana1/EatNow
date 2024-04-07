using System.ComponentModel.DataAnnotations;

namespace EatNow.Models
{
    public class Cliente
    {
        [Required]
        public int IdCliente { get; set; }

        [Required]
        [StringLength(50)]
        public string CorreoElectronico { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; }

        [Required]
        [StringLength(11)]
        public string Telefono { get; set; }

        [StringLength(200)]
        public string? URLFoto { get; set; }
    }
}
