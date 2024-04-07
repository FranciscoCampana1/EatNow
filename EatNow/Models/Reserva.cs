using System.ComponentModel.DataAnnotations;

namespace EatNow.Models
{
    public class Reserva
    {
        [Required]
        public int IdReserva { get; set; }

        [Required]
        public DateTime Inicio { get; set; }

        [Required]
        public DateTime Fin { get; set; }

        [Required]
        public int RIdCliente { get; set; }

        public int RIdEstadoReserva { get; set; }

        [Required]
        public int RIdCasilla { get; set; }
        
        // Atributos propios de esta clase, no están en la BD
        public int NumeroMesa { get; set; }

        public string NombreCliente { get; set; }

        public string ApellidoCliente { get; set; }

        public string NombreRestaurante { get; set; }

        public string EstadoReservaNombre { get; set; }

        public int RIdRestaurante { get; set; }

    }
}
