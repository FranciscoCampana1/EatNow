using EatNow.Models;
using System.Data.SqlClient;

namespace EatNow.DAL
{
    public class CasillaDAL
    {
        public readonly string connectionString;

        public CasillaDAL(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Casilla GetCasillaByID(int id)
        {
            Casilla casilla = new Casilla();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT IdCasilla, X, Y, NumeroMesa, EsMesa, RIdRestaurante " +
                               "FROM Casilla WHERE IdCasilla = @id;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            casilla = new Casilla
                            {
                                IdCasilla = Convert.ToInt32(reader["IdCasilla"]),
                                X = Convert.ToInt32(reader["X"]),
                                Y = Convert.ToInt32(reader["Y"]),
                                NumeroMesa = (reader["NumeroMesa"] != DBNull.Value) ? Convert.ToInt32(reader["NumeroMesa"]) : 1,
                                EsMesa = Convert.ToBoolean(reader["EsMesa"]),
                                RIdRestaurante = Convert.ToInt32(reader["RIdRestaurante"])
                            };
                        }
                    }
                }
            }

            return casilla;
        }

        public int TransactionInsertCasillas(List<Casilla> casillas)
        {
            int affectedRows = 0;

            using (SqlConnection connection = new(connectionString))
            {
                connection.Open();

                // Start a local transaction.
                SqlTransaction transaction = connection.BeginTransaction();            

                try
                {
                    // Bucle para insertar todas las casillas
                    foreach(Casilla casilla in casillas)
                    {
                        string query = "INSERT INTO Casilla (X, Y, EsMesa, EstaOcupada, RIdRestaurante) " +
                               "VALUES (@X, @Y, @EsMesa, 0, @IdRestaurante)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Transaction = transaction;

                            command.Parameters.AddWithValue("@X", casilla.X);
                            command.Parameters.AddWithValue("@Y", casilla.Y);
                            command.Parameters.AddWithValue("@EsMesa", casilla.EsMesa);
                            command.Parameters.AddWithValue("@IdRestaurante", casilla.RIdRestaurante);

                            affectedRows += command.ExecuteNonQuery();
                        }
                    }

                    // Commit the transaction.
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // Handle the exception if the transaction fails to commit.
                    Console.WriteLine(ex.Message);

                    try
                    {
                        // Attempt to roll back the transaction.
                        transaction.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        // Throws an InvalidOperationException if the connection
                        // is closed or the transaction has already been rolled
                        // back on the server.
                        Console.WriteLine(exRollback.Message);
                    }

                    affectedRows = -1;
                }
            }

            return affectedRows;
        }

        public List<Casilla> GetCasillasByRestaurantId(int idRestaurant)
        {
            List<Casilla> casillas = new List<Casilla>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT IdCasilla, X, Y, NumeroMesa, EsMesa, RIdRestaurante " +
                               "FROM Casilla WHERE RIdRestaurante = @IdRestaurante";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@IdRestaurante", idRestaurant);

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Casilla casilla = new Casilla
                            {
                                IdCasilla = Convert.ToInt32(reader["IdCasilla"]),
                                X = Convert.ToInt32(reader["X"]),
                                Y = Convert.ToInt32(reader["Y"]),
                                NumeroMesa = (reader["NumeroMesa"] != DBNull.Value) ? Convert.ToInt32(reader["NumeroMesa"]) : 1,
                                EsMesa = Convert.ToBoolean(reader["EsMesa"]),
                                RIdRestaurante = Convert.ToInt32(reader["RIdRestaurante"])
                            };
                            casillas.Add(casilla);
                        }
                    }
                }
            }

            return casillas;
        }

        Casilla GetCasillaByCoord(List<Casilla> casillas, int X, int Y)
        {
            try
            {
                return casillas.First(c => c.X == X && c.Y == Y);
            }
            catch
            {
                return null;
            }
            
        }

        public void TransaccionUpdateCasillas(List<Casilla> casillasNew, List<Casilla> casillasOld)
        {
            using (SqlConnection connection = new(connectionString))
            {
                connection.Open();

                // Start a local transaction.
                SqlTransaction transaction = connection.BeginTransaction();

                try
                {
                    for(int x= 0; x < 15; x++)
                    {
                        for(int y= 0; y < 15; y++)
                        {
                            Casilla casillaNew = GetCasillaByCoord(casillasNew, x, y);
                            Casilla casillaOld = GetCasillaByCoord(casillasOld, x, y);

                            string query = "";

                            if (casillaNew != null && casillaOld == null)
                            {
                                query = "INSERT INTO Casilla (X, Y, EsMesa, EstaOcupada, RIdRestaurante) VALUES (@X, @Y, @EsMesa, 0, @IdRestaurante)";
                                Console.WriteLine($"Insert X:{casillaNew.X} Y:{casillaNew.Y}");

                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    command.Transaction = transaction;

                                    command.Parameters.AddWithValue("@X", casillaNew.X);
                                    command.Parameters.AddWithValue("@Y", casillaNew.Y);
                                    command.Parameters.AddWithValue("@EsMesa", casillaNew.EsMesa);
                                    command.Parameters.AddWithValue("@IdRestaurante", casillaNew.RIdRestaurante);

                                    command.ExecuteNonQuery();
                                }
                            }
                            else if (casillaNew != null && casillaOld != null)
                            {
                                query = "UPDATE Casilla SET EsMesa = @EsMesa WHERE X = @X AND Y = @Y AND RIdRestaurante = @IdRestaurante";
                                Console.WriteLine($"Update X:{casillaNew.X} Y:{casillaNew.Y}");

                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    command.Transaction = transaction;

                                    command.Parameters.AddWithValue("@X", casillaNew.X);
                                    command.Parameters.AddWithValue("@Y", casillaNew.Y);
                                    command.Parameters.AddWithValue("@EsMesa", casillaNew.EsMesa);
                                    command.Parameters.AddWithValue("@IdRestaurante", casillaNew.RIdRestaurante);

                                    command.ExecuteNonQuery();
                                }
                            }
                            else if (casillaNew == null && casillaOld != null)
                            {
                                query = "DELETE FROM Casilla WHERE X = @X AND Y = @Y AND RIdRestaurante = @IdRestaurante";
                                Console.WriteLine($"Delete X:{casillaOld.X} Y:{casillaOld.Y}");

                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    command.Transaction = transaction;

                                    command.Parameters.AddWithValue("@X", casillaOld.X);
                                    command.Parameters.AddWithValue("@Y", casillaOld.Y);
                                    command.Parameters.AddWithValue("@IdRestaurante", casillaOld.RIdRestaurante);

                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    // Commit the transaction.
                    transaction.Commit();
                    Console.WriteLine("Añadido Todo");
                }
                catch (Exception ex)
                {
                    // Handle the exception if the transaction fails to commit.
                    Console.WriteLine(ex.Message);

                    try
                    {
                        // Attempt to roll back the transaction.
                        transaction.Rollback();
                    }
                    catch (Exception exRollback)
                    {
                        // Throws an InvalidOperationException if the connection
                        // is closed or the transaction has already been rolled
                        // back on the server.
                        Console.WriteLine(exRollback.Message);
                    }
                }
            }
        }
    }
}
