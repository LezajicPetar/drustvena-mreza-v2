using drustvena_mreza.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace drustvena_mreza.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<Group>> GetAll()
        {
            List<Group> groups = GetAllFromDatabase();
            return Ok(groups);
        }

        private List<Group> GetAllFromDatabase()
        {
            List<Group> result = new List<Group>();

            using (SqliteConnection connection = new SqliteConnection("Data Source=database/socialnetwork.db"))
            {
                connection.Open();
                string query = "SELECT Id, Name, CreationDate FROM Groups";

                using (SqliteCommand command = new SqliteCommand(query, connection))
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Group group = new Group(
      reader.GetInt32(0),
      reader.GetString(1),
      DateOnly.Parse(reader.GetString(2))
  );
                        result.Add(group);
                    }
                }
            }

            return result;
        }
    }
}


////KorisnikRepository korisnikRepository = new KorisnikRepository();
//GroupDbRepository grupaRepository = new GroupDbRepository();

//[HttpGet]
//public ActionResult<List<User>> GetUsersByGroup(int groupId)
//{
//    if (!GroupDbRepository.Data.ContainsKey(groupId))
//    {
//        return NotFound();
//    }

//    List<User> korisnici = new List<User>();
//    foreach (Group g in GroupDbRepository.Data.Values)
//    {
//        if (g.Id == groupId)
//        {
//            korisnici = g.ListaKorisnika;
//        }
//    }

//    return Ok(korisnici);
//}

