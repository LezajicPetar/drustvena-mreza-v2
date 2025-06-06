using drustvena_mreza.Model;
using drustvena_mreza.Models;
using Microsoft.Data.Sqlite;

namespace drustvena_mreza.Repositories
{
    public class PostDbRepository
    {
        private readonly string connectionString;

        public PostDbRepository(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionString:SQLiteConnection"];
        }

        public List<Post> GetAll()
        {
            try
            {
                List<Post> sviPostovi = new List<Post>();

                SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"SELECT p.Id AS postId, p.Content, p.Date,  u.Id AS UserId, u.Username, u.Name, u.Surname, u.Birthday
                             FROM Posts p
                             INNER JOIN Users u ON p.UserId = u.Id";

                SqliteCommand command = new SqliteCommand(query, connection);

                SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int userId = Convert.ToInt32(reader["UserId"]);
                    string username = reader["Username"].ToString();
                    string name = reader["Name"].ToString();
                    string surname = reader["Surname"].ToString();
                    DateOnly birthday = DateOnly.Parse(reader["Birthday"].ToString());

                    User user = new User(userId, username, name, surname, birthday);


                    int postId = Convert.ToInt32(reader["PostId"]);
                    string content = reader["Content"].ToString();
                    DateOnly date = DateOnly.Parse(reader["Date"].ToString());

                    Post post = new Post(postId, userId, user, content, date);

                    sviPostovi.Add(post);
                }

                return sviPostovi;
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
    }
}
