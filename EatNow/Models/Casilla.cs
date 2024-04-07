using System.ComponentModel.DataAnnotations;

namespace EatNow.Models
{
    public class Casilla
    {
        [Required]
        public int IdCasilla { get; set; }

        [Required]
        public int X { get; set; }

        [Required]
        public int Y { get; set; }

        public int NumeroMesa { get; set; }

        public bool EsMesa {  get; set; }
        public bool EstaOcupada { get; set; }

        [Required]
        public int RIdRestaurante { get; set; }
    }
}
