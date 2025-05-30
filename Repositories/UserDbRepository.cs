using drustvena_mreza.Model;
using Microsoft.Data.Sqlite;

namespace drustvena_mreza.Repositories
{
    public class UserDbRepository
    {
        private const string connectionString = "Data Source = database/database.db";

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

                while (reader.Read())
                {
                    idKorisnika = Convert.ToInt32(reader["Id"]);
                    userName = reader["Username"].ToString();
                    name = reader["Name"].ToString();
                    surname = reader["Surname"].ToString();
                    birthday = DateOnly.Parse(reader["Birthday"].ToString());
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
    }
}
