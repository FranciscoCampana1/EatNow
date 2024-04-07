using System.ComponentModel.DataAnnotations;

namespace EatNow.Models
{
    public class Pedido
    {
        [Required]
        public int IdPedido { get; set; }

        [Required]
        public int RIdReserva { get; set; }

        [Required]
        public int RIdPlato { get; set; }
    }
}
