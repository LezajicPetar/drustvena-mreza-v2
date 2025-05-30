using drustvena_mreza.Model;
using drustvena_mreza.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace drustvena_mreza.Controllers
{
    [Route("api/korisnici")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserDbRepository userDbRepository;

        public UserController()
        {
            userDbRepository = new UserDbRepository();
        }

        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            return Ok(userDbRepository.GetAllFromDatabase());
        }

        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {
            return Ok(userDbRepository.GetById(id));
        }

        private int DodeliNoviId(List<int> identifikatori)
        {
            int maxId = 0;

            foreach (int i in identifikatori)
            {
                if (i > maxId)
                {
                    maxId = i;
                }
            }

            return maxId + 1;
        }
    }
}
