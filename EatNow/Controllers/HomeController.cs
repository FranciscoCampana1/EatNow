using EatNow.DAL;
using EatNow.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace EatNow.Controllers
{
    public class HomeController : Controller
    {
        private readonly RestauranteDAL restauranteDAL;
        private readonly ClienteDAL clienteDAL;
        private readonly EmpleadoDAL empleadoDAL;
        private readonly ReservaDAL reservaClienteDAL;

        public HomeController()
        {
            restauranteDAL = new RestauranteDAL(Conexion.CadenaBBDD);
            clienteDAL = new ClienteDAL(Conexion.CadenaBBDD);
            empleadoDAL = new EmpleadoDAL(Conexion.CadenaBBDD);
            reservaClienteDAL = new ReservaDAL(Conexion.CadenaBBDD);
        }

        public IActionResult Login()
        {
            // Si ha iniciado sesión le redirigimos a la pantalla de lista de restaurantes
            /*if (Request.Cookies["IdCliente"] != null)
                return RedirectToAction("index");
            // Si ha iniciado sesión como empleado le redirigimos a la vista principal de empleado
            else if (Request.Cookies["IdEmpleado"] != null)
                // TODO: Cambiar la ruta a la que se dirige
                return RedirectToAction("ListReservasRestaurante", "Restaurante");
            else*/
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetCliente(Cliente cliente)
        {
            cliente = clienteDAL.GetClientByEmailPassword(cliente.CorreoElectronico, cliente.Password);

            if (cliente == null)
            {
                TempData["ErrorLoginMessage"] = "Tu correo electrónico y/o contrseña son incorrectos";
                return RedirectToAction("login");
            }
            else
            {
                // Guardamos una cookie con el IdCliente
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(1);

                Response.Cookies.Append("IdCliente", cliente.IdCliente.ToString());
                ViewBag.IdCliente = cliente.IdCliente;

                //return RedirectToAction("index");
                return Redirect("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Cliente cliente)
        {
            int affectedRows = clienteDAL.InsertClient(cliente);

            // Si devuelve -1 ha habido un error
            if (affectedRows == -1)
                TempData["ErrorCreatingClient"] = "Ha habido un error al crear tu cuenta, vuelve a intentarlo";
            else if (affectedRows == 1)
                TempData["ClientCreatedMessage"] = "Cuenta creada correctamente!";
                
            return RedirectToAction("login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult GetEmpleado(string email, string password)
        {
            Empleado empleado = empleadoDAL.GetEmployeeByEmailPassword(email, password);
            
            if (empleado == null)
            {
                TempData["ErrorLoginMessage"] = "Tu correo electrónico y/o contrseña son incorrectos";
                return RedirectToAction("login");
            }
            else
            {
                // Guardamos una cookie con el IdEmpleado
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(1);

                Response.Cookies.Append("IdEmpleado", empleado.IdEmpleado.ToString());

                // TODO: Cambiar la ruta a la que se dirige
                return RedirectToAction("Index", "Empleado");
            }
        }

        public IActionResult Index()
        {
            List<Restaurante> listRestaurants = new List<Restaurante>();
            listRestaurants = restauranteDAL.GetAllRestaurants();

            if (Request.Cookies["IdEmpleado"] != null)
            {
                return RedirectToAction("Index", "Empleado");
            }

            else if (Request.Cookies["IdCliente"] != null)
            {
                ViewBag.IdCliente = Request.Cookies["IdCliente"];
                ViewBag.ImageCliente = clienteDAL.GetClientImage(int.Parse(Request.Cookies["IdCliente"]));

                List<Reserva> reservas = reservaClienteDAL.LastFiveReservation(int.Parse(Request.Cookies["IdCliente"]));
                ViewBag.Reserva = reservas;
            }
            return View(listRestaurants);
        }

        public IActionResult ReservasUsuario()
        {
            return Redirect("../cliente/ListReservasUsuario");
        }

        public IActionResult Perfil()
        {
            return Redirect("../cliente/InfoUsuario");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CerrarSesion()
        {
            if (Request.Cookies["IdCliente"] != null)
            {
                Response.Cookies.Delete("IdCliente");
                Response.Cookies.Delete("IdEmpleado");
                return Redirect("Index");
            }
            else if (Request.Cookies["IdEmpleado"] != null)
            {
                Response.Cookies.Delete("IdEmpleado");
                Response.Cookies.Delete("IdCliente");
                return RedirectToAction("Index","Empleado");
                
            }
            else
            {
                return Redirect("Index");
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string time, string date, string direccion, string nombre)
        {
            List<Restaurante> listRestaurants = new List<Restaurante>();
            listRestaurants = restauranteDAL.GetRestaurantsByFilter(time, direccion, nombre);

            if (Request.Cookies["IdCliente"] != null)
            {
                ViewBag.IdCliente = Request.Cookies["IdCliente"];
                List<Reserva> reservas = reservaClienteDAL.LastFiveReservation(int.Parse(Request.Cookies["IdCliente"]));
                ViewBag.Reserva = reservas;
            }
            else
                RedirectToAction("Index");

            return View(listRestaurants);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BotonHome()
        {
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
