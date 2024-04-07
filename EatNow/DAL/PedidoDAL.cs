using EatNow.Models;
using System.Data.SqlClient;

namespace EatNow.DAL
{
    public class PedidoDAL
    {
        public readonly string connectionString;

        public PedidoDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public int InsertOrder(Pedido pedido)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Plato (RIdReserva, RIdPlato) " +
                               "VALUES (@RIdReserva, @RIdPlato)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@RIdReserva", pedido.RIdReserva);
                    command.Parameters.AddWithValue("@RIdPlato", pedido.RIdPlato);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public List<Pedido> GetOrderOfBooking(int idReserva)
        {
            List<Pedido> pedidos = new List<Pedido>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string query = "SELECT * FROM Pedido WHERE RIdReserva = @IdReserva";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdReserva", idReserva);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Pedido pedido = new Pedido
                            {
                                IdPedido = int.Parse(reader["IdPedido"].ToString()),
                                RIdReserva = int.Parse(reader["RIdReserva"].ToString()),
                                RIdPlato = int.Parse(reader["RIdPlato"].ToString()),
                            };
                            pedidos.Add(pedido);
                        }
                    }
                }
            }

            return pedidos;
        }
    }
}
