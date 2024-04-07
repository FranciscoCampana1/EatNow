using EatNow.DAL;
using EatNow.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EatNow.Controllers
{
    public class EmpleadoController : Controller
    {
        private readonly RestauranteDAL restauranteDAL;
        private readonly EmpleadoDAL empleadoDAL;
        private readonly PlatoDAL platoDAL;
        private readonly CasillaDAL casillaDAL;

        public EmpleadoController()
        {
            restauranteDAL = new RestauranteDAL(Conexion.CadenaBBDD);
            empleadoDAL = new EmpleadoDAL(Conexion.CadenaBBDD);
            platoDAL = new PlatoDAL(Conexion.CadenaBBDD);
            casillaDAL = new CasillaDAL(Conexion.CadenaBBDD);
        }

        // GET: EmpleadoController
        public IActionResult Index()
        {
            if (Request.Cookies["IdEmpleado"] != null)
            {
                ViewBag.IdEmpleado = Request.Cookies["IdEmpleado"];
                Empleado empleado = empleadoDAL.GetEmployeeById(int.Parse(ViewBag.IdEmpleado));
                ViewBag.NombreRestaurante = restauranteDAL.GetRestaurantById(empleado.RIdRestaurante).Nombre;
                return View();
            }
            else
            {
                return RedirectToAction("Login","Home");
            }
        }

        public IActionResult ListaReservasRestaurante()
        {
            if (Request.Cookies["IdEmpleado"] != null)
            {
                ViewBag.IdEmpleado = Request.Cookies["IdEmpleado"];
            }
            return RedirectToAction("ListReservasRestaurante", "Restaurante");
        }

        public IActionResult DatosRestaurante()
        {
            if (Request.Cookies["IdEmpleado"] != null)
            {
                ViewBag.IdEmpleado = Request.Cookies["IdEmpleado"];
            }
            return RedirectToAction("InfoRestaurante", "Restaurante");
        }

        public IActionResult MenuRestaurante()
        {
            if (Request.Cookies["IdEmpleado"] != null)
            {
                ViewBag.IdEmpleado = Request.Cookies["IdEmpleado"];

                int idEmpleado = int.Parse(Request.Cookies["IdEmpleado"]);
                Empleado empleado = empleadoDAL.GetEmployeeById(idEmpleado);
                Restaurante restaurante = restauranteDAL.GetRestaurantById(empleado.RIdRestaurante);
                List<Plato> platos = platoDAL.GetAllDishesFromRestaurant(restaurante.IdRestaurante);

                ViewBag.IdRestaurante = restaurante.IdRestaurante;
                ViewBag.NombreRestaurante = restauranteDAL.GetRestaurantById(empleado.RIdRestaurante).Nombre;

                return View(platos);
            }

            else
            {
                return RedirectToAction("Login", "Home");
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AñadirPlato(string name, string precio, string URL, int IdRestaurante)
        {

            Plato plato = new Plato { Nombre = name, Precio = double.Parse(precio), URLImagen = URL, RIdRestaurante = IdRestaurante };
            int affectedRows = platoDAL.InsertDish(plato);

            return RedirectToAction("MenuRestaurante");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarPlato(string IdPlato)
        {
            int affectedRows = platoDAL.DeleteDish(int.Parse(IdPlato));

            return RedirectToAction("MenuRestaurante");
        }

        public IActionResult ListEmpleadosRestaurante()
        {
            if (Request.Cookies["IdEmpleado"] != null)
            {
                ViewBag.IdEmpleado = Request.Cookies["IdEmpleado"];

                int idEmpleado = int.Parse(Request.Cookies["IdEmpleado"]);
                Empleado empleado = empleadoDAL.GetEmployeeById(idEmpleado);

                List<Empleado> empleados = empleadoDAL.GetAllEmployeesExcept(empleado.RIdRestaurante, idEmpleado);

                ViewBag.IdRestaurante = empleado.RIdRestaurante;
                ViewBag.NombreRestaurante = restauranteDAL.GetRestaurantById(empleado.RIdRestaurante).Nombre;

                return View(empleados);
            }

            else
            {
                return RedirectToAction("Login", "Home");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]           
        public IActionResult AddEmpleado(string idRestaurante, string dni, string nombre, string apellidos, string email, string password)
        {
            if (Request.Cookies["IdEmpleado"] != null)
            {
                ViewBag.IdEmpleado = Request.Cookies["IdEmpleado"];
            }
            Empleado empleado = new Empleado
            {
                IdEmpleado = int.Parse(idRestaurante),
                DNI = dni,
                Nombre = nombre,
                Apellidos = apellidos,
                CorreoElectronico = email,
                Password = password,
                RIdRestaurante = int.Parse(idRestaurante)
            };
            empleadoDAL.InsertEmployee(empleado);

            return RedirectToAction("ListEmpleadosRestaurante");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarEmpleado(string idEmpleado)
        {
            if (Request.Cookies["IdEmpleado"] != null)
            {
                ViewBag.IdEmpleado = Request.Cookies["IdEmpleado"];
            }
            empleadoDAL.DeleteEmployee(int.Parse(idEmpleado));
            return RedirectToAction("ListEmpleadosRestaurante");
        }

        public IActionResult mapaRestauranteEmpleado()
        {
            if (Request.Cookies["IdEmpleado"] != null)
            {
                ViewBag.IdEmpleado = Request.Cookies["IdEmpleado"];
                Empleado empleado = empleadoDAL.GetEmployeeById(int.Parse(ViewBag.IdEmpleado));
                ViewBag.NombreRestaurante = restauranteDAL.GetRestaurantById(empleado.RIdRestaurante).Nombre;
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        [HttpGet]
        public IActionResult GetCasillas()
        {
            int idEmpleado = int.Parse(Request.Cookies["IdEmpleado"]);
            Empleado empleado = empleadoDAL.GetEmployeeById(idEmpleado);

            Casilla[] casillas = casillaDAL.GetCasillasByRestaurantId(empleado.RIdRestaurante).ToArray();

            string casillasJson = JsonConvert.SerializeObject(casillas);

            return Json(casillasJson);
        }

        [HttpPost]
        public IActionResult SaveCasillas(string casillasJson)
        {
            Casilla[] casillasArray = JsonConvert.DeserializeObject<Casilla[]>(casillasJson);

            int idEmpleado = int.Parse(Request.Cookies["IdEmpleado"]);
            Empleado empleado = empleadoDAL.GetEmployeeById(idEmpleado);

            foreach (Casilla c in casillasArray)
            {
                //Console.WriteLine($"X:{c.X} Y:{c.Y} EsMesa:{c.EsMesa}");
                c.RIdRestaurante = empleado.RIdRestaurante;
            }

            if (casillaDAL.GetCasillasByRestaurantId(empleado.RIdRestaurante).Count == 0)
            {
                int affected = casillaDAL.TransactionInsertCasillas(casillasArray.ToList());

                if(affected == 0)
                    TempData["ErrorMapMessage"] = "Ha habido un error al intentar insertar las casillas";
                else
                    TempData["MapCreatedMessage"] = "Todas las casillas guardadas correctamente";
            }
            else
            {
                casillaDAL.TransaccionUpdateCasillas(casillasArray.ToList(), casillaDAL.GetCasillasByRestaurantId(empleado.RIdRestaurante));
                TempData["MapCreatedMessage"] = "Todas las casillas guardadas correctamente";
            }

            //return RedirectToAction("mapaRestauranteEmpleado", "Empleado");
            return Json(new { success = true });
        }

        public IActionResult InfoUsuario()
        {
            if (Request.Cookies["IdEmpleado"] != null)
            {
                ViewBag.IdEmpleado = Request.Cookies["IdEmpleado"];
                Empleado empleado = empleadoDAL.GetEmployeeById(int.Parse(ViewBag.IdEmpleado));

                ViewBag.NombreRestaurante = restauranteDAL.GetRestaurantById(empleado.RIdRestaurante).Nombre;

                return View(empleado);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
    }
}
