using System.ComponentModel.DataAnnotations;

namespace EatNow.Models
{
    public class Imagen
    {
        public int IdImagen { get; set; }
        public string URL { get; set; }
        public int RIdRestaurante { get; set; }
    }
}
