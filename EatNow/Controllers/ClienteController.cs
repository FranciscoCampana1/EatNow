using EatNow.DAL;
using EatNow.Models;
using Microsoft.AspNetCore.Mvc;

namespace EatNow.Controllers
{
    public class ClienteController : Controller
    {
        private readonly ClienteDAL clienteDAL;
        private readonly ReservaDAL reservaDAL;

        public ClienteController()
        {
            clienteDAL = new ClienteDAL(Conexion.CadenaBBDD);
            reservaDAL = new ReservaDAL(Conexion.CadenaBBDD);
        }

        public IActionResult InfoUsuario()
        {


            if (Request.Cookies["IdCliente"] != null)
            {
                int idCliente = int.Parse(Request.Cookies["IdCliente"]);
                Cliente cliente = clienteDAL.GetClientById(idCliente);

                ViewBag.IdCliente = Request.Cookies["IdCliente"];
                ViewBag.ImageCliente = clienteDAL.GetClientImage(int.Parse(Request.Cookies["IdCliente"]));
                return View(cliente);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateClient(Cliente cliente)
        {
            cliente.IdCliente = int.Parse(Request.Cookies["IdCliente"]);
            int affectedRows = clienteDAL.UpdateClient(cliente);

            if (affectedRows != -1)
                TempData["ClientUpdatedMessage"] = "Datos actualizados correctamente!";
            else
                TempData["ErrorUpdatingMessage"] = "Ha habido un error al actualizar los datos";

            return RedirectToAction("InfoUsuario");
        }



        public IActionResult ListReservasUsuario()
        {
            if (Request.Cookies["IdCliente"] != null)
            {
                int idCliente = int.Parse(Request.Cookies["IdCliente"]);
                Cliente cliente = clienteDAL.GetClientById(idCliente);

                List<Reserva> reservas = reservaDAL.ListReservasUsuario(idCliente);

                if (reservas.Count == 0)
                    TempData["NoBookingsMessage"] = "No tienes reservas";

                ViewBag.IdCliente = Request.Cookies["IdCliente"];
                ViewBag.ImageCliente = clienteDAL.GetClientImage(int.Parse(Request.Cookies["IdCliente"]));

                return View(reservas);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelBooking(int idReserva)
        {
            reservaDAL.CancelBooking(idReserva);
            return RedirectToAction("ListReservasUsuario");
        }
    }
}
