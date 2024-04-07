using EatNow.Models;
using System.Data.SqlClient;

namespace EatNow.DAL
{
    public class ClienteDAL
    {
        public readonly string connectionString;

        public ClienteDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Cliente GetClientByEmailPassword(string email, string password)
        {
            Cliente cliente = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT IdCliente, CorreoElectronico, Password, Nombre, Apellidos, Telefono, URLFoto " +
                               "FROM Cliente WHERE CorreoElectronico = @Email AND Password = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = new Cliente
                            {
                                IdCliente = int.Parse(reader["IdCliente"].ToString()),
                                CorreoElectronico = reader["CorreoElectronico"].ToString(),
                                Password = reader["Password"].ToString(),
                                Nombre = reader["Nombre"].ToString(),
                                Apellidos = (reader["Apellidos"] != DBNull.Value) ? reader["Apellidos"].ToString() : null,
                                Telefono = reader["Telefono"].ToString(),
                                URLFoto = (reader["URLFoto"] != DBNull.Value) ? reader["URLFoto"].ToString() : null
                            };
                        }
                    }
                }
            }

            return cliente;
        }

        public int InsertClient(Cliente cliente)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Cliente (CorreoElectronico, Password, Nombre, Telefono) " +
                               "VALUES (@CorreoElectronico, @Password, @Nombre, @Telefono)";
                               
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CorreoElectronico", cliente.CorreoElectronico);
                    command.Parameters.AddWithValue("@Password", cliente.Password);
                    command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    command.Parameters.AddWithValue("@Telefono", cliente.Telefono);

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }

        public Cliente GetClientById(int idCliente)
        {
            Cliente cliente = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT IdCliente, CorreoElectronico, Password, Nombre, Apellidos, Telefono, URLFoto " +
                               "FROM Cliente WHERE IdCliente = @IdCliente";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdCliente", idCliente);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = new Cliente
                            {
                                IdCliente = int.Parse(reader["IdCliente"].ToString()),
                                CorreoElectronico = reader["CorreoElectronico"].ToString(),
                                Password = reader["Password"].ToString(),
                                Nombre = reader["Nombre"].ToString(),
                                Apellidos = (reader["Apellidos"] != DBNull.Value) ? reader["Apellidos"].ToString() : null,
                                Telefono = reader["Telefono"].ToString(),
                                URLFoto = (reader["URLFoto"] != DBNull.Value) ? reader["URLFoto"].ToString() : null
                            };
                        }
                    }
                }
            }

            return cliente;
        }

        public int UpdateClient(Cliente cliente)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Cliente " + 
                               "SET CorreoElectronico = @Email, Password = @Password, Nombre = @Nombre, " + 
                               "Apellidos = @Apellidos, Telefono = @Telefono, URLFoto = @URLFoto " +
                               "WHERE IdCliente = @IdCliente";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdCliente", cliente.IdCliente);
                    command.Parameters.AddWithValue("@Email", cliente.CorreoElectronico);
                    command.Parameters.AddWithValue("@Password", cliente.Password);
                    command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    command.Parameters.AddWithValue("@Apellidos", (cliente.Apellidos == null) ? DBNull.Value : cliente.Apellidos);
                    command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                    command.Parameters.AddWithValue("@URLFoto", (cliente.URLFoto == null) ? DBNull.Value : cliente.URLFoto);

                    connection.Open();

                    return command.ExecuteNonQuery();
                }
            }
        }

        public String GetClientImage(int idCliente)
        {
            Cliente cliente = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT URLFoto " +
                               "FROM Cliente WHERE IdCliente = @IdCliente";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdCliente", idCliente);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = new Cliente
                            {
                                
                                URLFoto = (reader["URLFoto"] != DBNull.Value) ? reader["URLFoto"].ToString() : null
                            };
                        }
                    }
                }
            }

            return cliente.URLFoto;
        }
    }
}
