using EatNow.Models;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;

namespace EatNow.DAL
{
    public class RestauranteDAL
    {
        private readonly string connectionString;

        public RestauranteDAL(string connectionString) {
            this.connectionString = connectionString;
        }
        
        public List<Restaurante> GetAllRestaurants()
        {
            List<Restaurante> restaurants = new List<Restaurante>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT IdRestaurante, Nombre, Direccion, Telefono, Web, Descripcion, " +
                               "CONVERT(VARCHAR(5), HoraApertura, 108) AS HoraApertura, " +
                               "CONVERT(VARCHAR(5), HoraCierre, 108) AS HoraCierre, " +
                               "URLImagen = (SELECT TOP 1 URL FROM Imagen WHERE RIdRestaurante = IdRestaurante) " + 
                               "FROM Restaurante";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Restaurante restaurant = new Restaurante
                            {
                                IdRestaurante = int.Parse(reader["IdRestaurante"].ToString()),
                                Nombre = reader["Nombre"].ToString(),
                                Direccion = reader["Direccion"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                Web = (reader["Web"] != DBNull.Value) ? reader["Web"].ToString() : null,
                                Descripcion = (reader["Descripcion"] != DBNull.Value) ? reader["Descripcion"].ToString() : null,
                                HoraApertura = reader["HoraApertura"].ToString(),
                                HoraCierre = reader["HoraCierre"].ToString(),
                                URLImagen = (reader["URLImagen"] != DBNull.Value) ? reader["URLImagen"].ToString() : null
                            };
                            restaurants.Add(restaurant);
                        }
                    }
                }
            }

            return restaurants;
        }

        public Restaurante GetRestaurantById(int idRestaurant)
        {
            Restaurante restaurant = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT IdRestaurante, Nombre, Direccion, Telefono, Web, Descripcion, " +
                               "CONVERT(VARCHAR(5), HoraApertura, 108) AS HoraApertura, " +
                               "CONVERT(VARCHAR(5), HoraCierre, 108) AS HoraCierre, " +
                               "URLImagen = (SELECT TOP 1 URL FROM Imagen WHERE RIdRestaurante = @IdRestaurante) " +
                               "FROM Restaurante WHERE IdRestaurante = @IdRestaurante";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdRestaurante", idRestaurant);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            restaurant = new Restaurante
                            {
                                IdRestaurante = int.Parse(reader["IdRestaurante"].ToString()),
                                Nombre = reader["Nombre"].ToString(),
                                Direccion = reader["Direccion"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                Web = (reader["Web"] != DBNull.Value) ? reader["Web"].ToString() : null,
                                Descripcion = (reader["Descripcion"] != DBNull.Value) ? reader["Descripcion"].ToString() : null,
                                HoraApertura = reader["HoraApertura"].ToString(),
                                HoraCierre = reader["HoraCierre"].ToString(),
                                URLImagen = (reader["URLImagen"] != DBNull.Value) ? reader["URLImagen"].ToString() : null
                            };
                        }
                    }
                }
            }

            return restaurant;
        }

        public int UpdateRestaurant(Restaurante restaurant)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Restaurante " +
                               "SET Nombre = @Nombre, Direccion = @Direccion, Telefono = @Telefono, " +
                               "Web = @Web, Descripcion = @Descripcion, HoraApertura = @HoraApertura, HoraCierre = @HoraCierre " +
                               "WHERE IdRestaurante = @IdRestaurante";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdRestaurante", restaurant.IdRestaurante);
                    command.Parameters.AddWithValue("@Nombre", restaurant.Nombre);
                    command.Parameters.AddWithValue("@Direccion", restaurant.Direccion);
                    command.Parameters.AddWithValue("@Telefono", restaurant.Telefono);
                    command.Parameters.AddWithValue("@Web", (restaurant.Web == null) ? DBNull.Value : restaurant.Web);
                    command.Parameters.AddWithValue("@Descripcion", (restaurant.Descripcion == null) ? DBNull.Value : restaurant.Descripcion);
                    command.Parameters.AddWithValue("@HoraApertura", restaurant.HoraApertura);
                    command.Parameters.AddWithValue("@HoraCierre", restaurant.HoraCierre);

                    connection.Open();

                    return command.ExecuteNonQuery();
                }
            }
        }

        public List<Restaurante> GetRestaurantsByFilter(string time, string direccion, string nombre)
        {
            List<Restaurante> restaurants = new List<Restaurante>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT IdRestaurante, Nombre, Direccion, Telefono, Web, Descripcion, " +
                               "CONVERT(VARCHAR(5), HoraApertura, 108) AS HoraApertura, " +
                               "CONVERT(VARCHAR(5), HoraCierre, 108) AS HoraCierre, " +
                               "URLImagen = (SELECT TOP 1 URL FROM Imagen WHERE RIdRestaurante = IdRestaurante) " +
                               "FROM Restaurante WHERE";

                query += $" Nombre LIKE '%{nombre}%' AND";
                query += $" Direccion LIKE '%{direccion}%' AND";
                query += $" HoraApertura >= '{time}'";


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Restaurante restaurant = new Restaurante
                            {
                                IdRestaurante = int.Parse(reader["IdRestaurante"].ToString()),
                                Nombre = reader["Nombre"].ToString(),
                                Direccion = reader["Direccion"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                Web = (reader["Web"] != DBNull.Value) ? reader["Web"].ToString() : null,
                                Descripcion = (reader["Descripcion"] != DBNull.Value) ? reader["Descripcion"].ToString() : null,
                                HoraApertura = reader["HoraApertura"].ToString(),
                                HoraCierre = reader["HoraCierre"].ToString(),
                                URLImagen = (reader["URLImagen"] != DBNull.Value) ? reader["URLImagen"].ToString() : null
                            };
                            restaurants.Add(restaurant);
                        }
                    }
                }
            }

            return restaurants;
        }
    }
}
