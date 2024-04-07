using EatNow.Models;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace EatNow.DAL
{
    public class PlatoDAL
    {
        public readonly string connectionString;

        public PlatoDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Plato> GetAllDishesFromRestaurant(int idRestaurant)
        {
            List<Plato> platos = new List<Plato>();
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT IdPlato, Nombre, Precio, RIdRestaurante, URLImagen " +
                               " FROM Plato WHERE RIdRestaurante = @IdRestaurante";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdRestaurante", idRestaurant);
                    
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Plato plato = new Plato
                            {
                                IdPlato = int.Parse(reader["IdPlato"].ToString()),
                                Nombre = reader["Nombre"].ToString(),
                                Precio = double.Parse(reader["Precio"].ToString()),
                                RIdRestaurante = int.Parse(reader["RIdRestaurante"].ToString()),
                                URLImagen = (reader["URLImagen"] != DBNull.Value) ? reader["URLImagen"].ToString() : null,
                            };
                            platos.Add(plato);
                        }
                    }
                }
            }

            return platos;
        }

        public int InsertDish(Plato plato)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Plato (Nombre, Precio, RIdRestaurante, URLImagen) " +
                               "VALUES (@Nombre, @Precio, @RIdRestaurante, @URLImagen)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", plato.Nombre);
                    command.Parameters.AddWithValue("@Precio", plato.Precio);
                    command.Parameters.AddWithValue("@RIdRestaurante", plato.RIdRestaurante);
                    command.Parameters.AddWithValue("@URLImagen", (plato.URLImagen == null) ? DBNull.Value : plato.URLImagen);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public int DeleteDish(int idPlato)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Plato WHERE IdPlato = @IdPlato";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdPlato", idPlato);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}
