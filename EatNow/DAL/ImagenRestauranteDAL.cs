using EatNow.Models;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace EatNow.DAL
{
    public class ImagenRestauranteDAL
    {
        public readonly string connectionString;

        public ImagenRestauranteDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public List<Imagen> GetAllRestaurantImages(int idRestaurant)
        {
            List<Imagen> images = new List<Imagen>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT IdImagen, URL, RIdRestaurante " +
                               "FROM Imagen WHERE RIdRestaurante = @IdRestaurante";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdRestaurante", idRestaurant);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Imagen image = new Imagen
                            {
                                IdImagen = int.Parse(reader["IdImagen"].ToString()),
                                URL = reader["URL"].ToString(),
                                RIdRestaurante = int.Parse(reader["RIdRestaurante"].ToString())
                            };
                            images.Add(image);                            
                        }
                    }
                }
            }

            return images;
        }

        public int InsertRestaurantImage(Imagen image)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Imagen (URL, RIdRestaurante) " +
                               "VALUES (@URL, @RIdRestaurante)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@URL", image.URL);
                    command.Parameters.AddWithValue("@RIdRestaurante", image.RIdRestaurante);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public int DeleteImageFromRestaurant(int idImage)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Imagen WHERE IdImagen = @IdImagen";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdImagen", idImage);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}
