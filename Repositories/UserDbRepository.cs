using drustvena_mreza.Model;
using Microsoft.Data.Sqlite;

namespace drustvena_mreza.Repositories
{
    public class UserDbRepository
    {
        private readonly string connectionString;
        

        public UserDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<User> GetPaged(int page, int pageSize)
        {
            List<User> listaKorisnika = new List<User>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Users LIMIT @page OFFSET @offset";
                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@page", pageSize);
                command.Parameters.AddWithValue("@offset", pageSize * (page - 1));

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string userName = reader["Username"].ToString();
                    string name = reader["Name"].ToString();
                    string prezime = reader["Surname"].ToString();
                    DateOnly datumRodjenja = DateOnly.Parse(reader["Birthday"].ToString());

                    listaKorisnika.Add(new User(id, userName, name, prezime, datumRodjenja));
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska pri konekciji ili neispravni SQL upit: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greska u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }

            return listaKorisnika;
        }

        public User GetById(int id)
        {
            int idKorisnika = -1;
            string username = "";
            string name = "";
            string surname = "";
            DateOnly birthyday = DateOnly.FromDateTime(DateTime.Now);

            try
            {
                string query = "SELECT * FROM Users WHERE Id = @Id";

                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using SqliteDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    return null;
                }
                while (reader.Read())
                {
                    idKorisnika = Convert.ToInt32(reader["Id"]);
                    username = reader["Username"].ToString();
                    name = reader["Name"].ToString();
                    surname = reader["Surname"].ToString();
                    birthyday = DateOnly.Parse(reader["Birthday"].ToString());
                }
                return new User(idKorisnika, username, name, surname, birthyday);
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska pri konekciji ili neispravni SQL upit: {ex.Message}");
                throw;
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greska u konverziji podataka iz baze: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
        }

        public User Create(User k)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"INSERT INTO users (Username, Name, Surname, Birthday)
                               VALUES (@username, @name, @surname, @birthday);
                               SELECT LAST_INSERT_ROWID();";

                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@username", k.Username);
                command.Parameters.AddWithValue("@name", k.Ime);
                command.Parameters.AddWithValue("@surname", k.Prezime);
                command.Parameters.AddWithValue("@birthday", k.DatumRodjenja.ToString());

                int idUbacenog = Convert.ToInt32(command.ExecuteScalar());

                return k;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska pri konekciji ili neispravni SQL upit: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
        }

        public User Update(User k)
        {
            try
            {
                string query = @"
                            UPDATE users
                            SET
                                Username = @username, 
                                Name = @name, 
                                Surname = @surname, 
                                Birthday = @birthday
                            WHERE Id = @id";

                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@username", k.Username);
                command.Parameters.AddWithValue("@name", k.Ime);
                command.Parameters.AddWithValue("@surname", k.Prezime);
                command.Parameters.AddWithValue("@birthday", k.DatumRodjenja.ToString());
                command.Parameters.AddWithValue("@id", k.Id);

                return command.ExecuteNonQuery() > 0 ? k : null;
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska pri konekciji ili neispravni SQL upit: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                string query = "DELETE FROM users WHERE Id = @id";

                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);

                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected > 0;

            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska pri konekciji ili neispravni SQL upit: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
        }

        public int CountAll()
        {
            try
            {
                string query = "SELECT COUNT(*) FROM Users";

                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                using SqliteCommand command = new SqliteCommand(query, connection);

                return Convert.ToInt32(command.ExecuteScalar());
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska pri konekciji ili neispravni SQL upit: {ex.Message}");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
                throw;
            }
        }
    }
}
