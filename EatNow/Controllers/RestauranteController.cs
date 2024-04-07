using EatNow.DAL;
using EatNow.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;
using Newtonsoft.Json;
using System.Linq;

namespace EatNow.Controllers
{
    public class RestauranteController : Controller
    {
        private readonly RestauranteDAL restauranteDAL;
        private readonly ClienteDAL clienteDAL;
        private readonly EmpleadoDAL empleadoDAL;
        private readonly ImagenRestauranteDAL imagenRestauranteDAL;
        private readonly ReservaDAL reservaDAL;
        private readonly PlatoDAL platoDAL;
        private readonly CasillaDAL casillaDAL;

        public RestauranteController()
        {
            restauranteDAL = new RestauranteDAL(Conexion.CadenaBBDD);
            clienteDAL = new ClienteDAL(Conexion.CadenaBBDD);
            empleadoDAL = new EmpleadoDAL(Conexion.CadenaBBDD);
            imagenRestauranteDAL = new ImagenRestauranteDAL(Conexion.CadenaBBDD);
            reservaDAL = new ReservaDAL(Conexion.CadenaBBDD);
            platoDAL = new PlatoDAL(Conexion.CadenaBBDD);
            casillaDAL = new CasillaDAL(Conexion.CadenaBBDD);
        }

        // GET: RestauranteController
        public IActionResult Index()
        {

            if (Request.Cookies["IdCliente"] != null)
            {
                ViewBag.IdCliente = Request.Cookies["IdCliente"];
                ViewBag.ImageCliente = clienteDAL.GetClientImage(int.Parse(Request.Cookies["IdCliente"]));
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int idRestaurante)
        {
            Restaurante restaurante = restauranteDAL.GetRestaurantById(idRestaurante);

            if (restaurante == null)
            {
                List<Restaurante> listRestaurants = new List<Restaurante>();
                listRestaurants = restauranteDAL.GetAllRestaurants();

                TempData["ErrorLoginClientMessage"] = "El restaurante no existe";
                return RedirectToAction("Login", "Home");
            }
            else
            {

                if (Request.Cookies["IdCliente"] != null)
                {
                    ViewBag.IdCliente = Request.Cookies["IdCliente"];
                    ViewBag.ImageCliente = clienteDAL.GetClientImage(int.Parse(Request.Cookies["IdCliente"]));
                }

                List<Imagen> images = imagenRestauranteDAL.GetAllRestaurantImages(restaurante.IdRestaurante);
                ViewBag.Images = images;

                List<Plato> platos = platoDAL.GetAllDishesFromRestaurant(restaurante.IdRestaurante);
                ViewBag.Platos = platos;

                return View(restaurante);
            }
        }

        public IActionResult MapaRestaurante(int idRestaurante)
        {
            Restaurante restaurante = restauranteDAL.GetRestaurantById(idRestaurante);

            if (Request.Cookies["IdCliente"] != null)
            {
                ViewBag.IdCliente = Request.Cookies["IdCliente"];
                ViewBag.ImageCliente = clienteDAL.GetClientImage(int.Parse(Request.Cookies["IdCliente"]));
                return View(restaurante);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        
        public IActionResult EstablecerHoraMapa(int idRestaurante, int Hora)
        {
            
            MapaRestaurante(idRestaurante, Hora);

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult MapaRestaurante(int idRestaurante, int Hora)
        {
            Restaurante restaurante = restauranteDAL.GetRestaurantById(idRestaurante);
            Console.WriteLine($"{idRestaurante}-{Hora}");

            if (Request.Cookies["IdCliente"] != null)
            {
                ViewBag.IdCliente = Request.Cookies["IdCliente"];
                ViewBag.ImageCliente = clienteDAL.GetClientImage(int.Parse(Request.Cookies["IdCliente"]));
                ViewBag.HoraInicio = Hora;
                return View(restaurante);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public IActionResult GetCasillas(int idRestaurante)
        {
            Casilla[] casillas = casillaDAL.GetCasillasByRestaurantId(idRestaurante).ToArray();

            string casillasJson = JsonConvert.SerializeObject(casillas);

            return Json(casillasJson);
        }

        public IActionResult ListReservasRestaurante()
        {
            int idRestaurant = 0;
            if (Request.Cookies["IdEmpleado"] != null)
            {
                ViewBag.IdEmpleado = Request.Cookies["IdEmpleado"];
                Empleado emp = empleadoDAL.GetEmployeeById(int.Parse(ViewBag.IdEmpleado));
                ViewBag.NombreRestaurante = restauranteDAL.GetRestaurantById(emp.RIdRestaurante).Nombre;
                idRestaurant = emp.RIdRestaurante;

                List<Reserva> reservas = reservaDAL.GetAllReservasRestauranteId(idRestaurant);

                if (reservas == null)
                {
                    List<Reserva> listaReservas = new List<Reserva>();
                    listaReservas = reservaDAL.GetAllReservasRestauranteId(idRestaurant);

                    TempData["ErrorLoginClientMessage"] = "No tienes reservas";
                    return View(listaReservas);
                }
                else
                {
                    // Guardamos el objeto cliente en el ViewBag
                    //ViewBag.ReservasRestauranteById = reservas;
                    return View(reservas);
                }

            }


            else{
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelBooking(int idReserva)
        {
            reservaDAL.CancelBooking(idReserva);
            return RedirectToAction("ListReservasRestaurante");
        }

        public IActionResult CompleteBooking(int idReserva)
        {
            reservaDAL.CompleteBooking(idReserva);
            return RedirectToAction("ListReservasRestaurante");
        }

        public IActionResult ConfirmacionReserva(string Hora, string Fecha, int IdCliente, int IdCasilla)
        {
            // Convertir la cadena de hora a TimeSpan
            TimeSpan hora = TimeSpan.Parse(Hora);

            // Convertir la cadena de fecha a DateTime
            DateTime fecha = DateTime.ParseExact(Fecha, "yyyy-MM-dd", null);

            // Combinar fecha y hora en un solo DateTime
            DateTime fechaInicio = fecha.Add(hora);
            DateTime fechaFin = fechaInicio.AddHours(3);

            Cliente cliente = clienteDAL.GetClientById(IdCliente);
            Reserva reserva = new Reserva { Inicio = fechaInicio, Fin = fechaFin, RIdCasilla = IdCasilla, RIdCliente = IdCliente, 
                NombreCliente = cliente.Nombre, ApellidoCliente = cliente.Apellidos };

            Casilla casilla = casillaDAL.GetCasillaByID(IdCasilla);
            Restaurante restaurante = restauranteDAL.GetRestaurantById(casilla.RIdRestaurante);

            List<Plato> platos = platoDAL.GetAllDishesFromRestaurant(restaurante.IdRestaurante);
            ViewBag.Platos = platos;

            if (Request.Cookies["IdCliente"] != null)
            {
                ViewBag.IdCliente = Request.Cookies["IdCliente"];
                ViewBag.ImageCliente = clienteDAL.GetClientImage(int.Parse(Request.Cookies["IdCliente"]));
            }

            return View(reserva);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearReserva(Reserva reserva)
        {
            reservaDAL.InsertBooking(reserva);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult InfoRestaurante()
        {
            if (Request.Cookies["IdEmpleado"] != null)
            {
                int idEmpleado = int.Parse(Request.Cookies["IdEmpleado"]);
                Empleado empleado = empleadoDAL.GetEmployeeById(idEmpleado);
                ViewBag.IdEmpleado = Request.Cookies["IdEmpleado"];
                ViewBag.NombreRestaurante = restauranteDAL.GetRestaurantById(empleado.RIdRestaurante).Nombre;

                Restaurante restaurante = restauranteDAL.GetRestaurantById(empleado.RIdRestaurante);

                List<Imagen> images = imagenRestauranteDAL.GetAllRestaurantImages(restaurante.IdRestaurante);
                ViewBag.Images = images;

                return View(restaurante);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateRestaurante(Restaurante restaurante)
        {
            int affectedRows = restauranteDAL.UpdateRestaurant(restaurante);

            if (affectedRows != -1)
                TempData["ClientUpdatedMessage"] = "Datos actualizados correctamente!";
            else
                TempData["ErrorUpdatingMessage"] = "Ha habido un error al actualizar los datos";

            return RedirectToAction("InfoRestaurante");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddFotoRestaurante(string idRestaurante, string urlImage)
        {
            Imagen image = new Imagen { RIdRestaurante = int.Parse(idRestaurante), URL = urlImage };
            int affectedRows = imagenRestauranteDAL.InsertRestaurantImage(image);
            return RedirectToAction("InfoRestaurante");
        }

        public IActionResult EliminarFotoRestaurante(string idImage)
        {

            int affectedRows = imagenRestauranteDAL.DeleteImageFromRestaurant(int.Parse(idImage));
            return RedirectToAction("InfoRestaurante");
        }
    }
}
