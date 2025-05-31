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

        public List<Korisnik> GetAllFromDatabase()
        {
            List<Korisnik> listaKorisnika = new List<Korisnik>();

            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = "SELECT * FROM Users";
                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string userName = reader["Username"].ToString();
                    string name = reader["Name"].ToString();
                    string prezime = reader["Surname"].ToString();
                    DateOnly datumRodjenja = DateOnly.Parse(reader["Birthday"].ToString());

                    listaKorisnika.Add(new Korisnik(id, userName, name, prezime, datumRodjenja));
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska pri konekciji ili neispravni SQL upit: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greska u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }

            return listaKorisnika;
        }

        public Korisnik GetById(int id)
        {
            int idKorisnika = -1;
            string userName = "";
            string name = "";
            string surname = "";
            DateOnly birthday = DateOnly.FromDateTime(DateTime.Now);

            try
            {
                string query = "SELECT * FROM Users WHERE Id = @Id";

                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                using SqliteCommand command = new SqliteCommand(query, connection);
                command.Parameters.AddWithValue("@Id", id);

                using SqliteDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    while (reader.Read())
                    {
                        idKorisnika = Convert.ToInt32(reader["Id"]);
                        userName = reader["Username"].ToString();
                        name = reader["Name"].ToString();
                        surname = reader["Surname"].ToString();
                        birthday = DateOnly.Parse(reader["Birthday"].ToString());
                    }

                }
                else
                {
                    return null;
                }

            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska pri konekciji ili neispravni SQL upit: {ex.Message}");
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Greska u konverziji podataka iz baze: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }

            return new Korisnik(idKorisnika, userName, name, surname, birthday);
        }

        public Korisnik Create(Korisnik k)
        {
            int idUbacenog = -1;

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

                idUbacenog = Convert.ToInt32(command.ExecuteScalar());
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska pri konekciji ili neispravni SQL upit: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }

            return GetById(idUbacenog);
        }

        public Korisnik Update(Korisnik k)
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

                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska pri konekciji ili neispravni SQL upit: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }

            return k;
        }

        public void Delete(int id)
        {
            try
            {
                string query = "DELETE FROM users WHERE Id = @id";

                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@id", id);

                command.ExecuteNonQuery();
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska pri konekciji ili neispravni SQL upit: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Konekcija nije otvorena ili je otvorena više puta: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }
        }
    }
}
