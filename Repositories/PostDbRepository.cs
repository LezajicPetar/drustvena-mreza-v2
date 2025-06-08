using drustvena_mreza.Model;
using drustvena_mreza.Models;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
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

                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"SELECT p.Id AS postId, p.Content, p.Date,  u.Id AS UserId, u.Username, u.Name, u.Surname, u.Birthday
                             FROM Posts p
                             INNER JOIN Users u ON p.UserId = u.Id";

                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();

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

        public Post Create(Post post)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                string query = @"INSERT INTO Posts (UserId, Content, Date)
                             VALUES (@userId, @content, @date);
                             SELECT LAST_INSERT_ROWID();";


                using SqliteCommand command = new SqliteCommand(query, connection);

                command.Parameters.AddWithValue("@userId", post.UserId);
                command.Parameters.AddWithValue("@content", post.Content);
                command.Parameters.AddWithValue("@date", post.Date.ToString());

                int postId = Convert.ToInt32(command.ExecuteScalar());

                return post;
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

        public bool Delete(int postId)
        {
            try
            {
                using SqliteConnection connection = new SqliteConnection(connectionString);
                connection.Open();

                using SqliteCommand command = new SqliteCommand(@"DELETE FROM Posts WHERE Id = @postId", connection);
                command.Parameters.AddWithValue("@postId", postId);

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

        public bool UserExists(int userId)
        {
            using SqliteConnection connection = new SqliteConnection(connectionString);
            connection.Open();

            using SqliteCommand command = new SqliteCommand("SELECT * FROM Users Where Id=@userId", connection);
            command.Parameters.AddWithValue("@userId", userId);

            int result = Convert.ToInt32(command.ExecuteScalar());

            return result == 1;
        }
    }
}
