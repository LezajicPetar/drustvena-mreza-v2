using drustvena_mreza.Model;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace drustvena_mreza.Models
{
    public class Post
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string Content { get; set; }
        public DateOnly Date { get; set; }

        public Post(int id, int userId, User user, string content, DateOnly date)
        {
            this.Id = id;
            this.UserId = userId;
            this.User = user;
            this.Content = content;
            this.Date = date;
        }
    }
}
