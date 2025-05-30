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

        [HttpPost]
        public ActionResult<Korisnik> Create([FromBody] Korisnik noviKorisnik)
        {
            if (string.IsNullOrWhiteSpace(noviKorisnik.Ime) ||
                string.IsNullOrWhiteSpace(noviKorisnik.Prezime) ||
                string.IsNullOrWhiteSpace(noviKorisnik.Username))
            {
                return BadRequest();
            }

            return Ok(userDbRepository.Create(noviKorisnik));
        }

        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update([FromBody] Korisnik k, int id)
        {
            if (string.IsNullOrWhiteSpace(k.Ime) ||
                string.IsNullOrWhiteSpace(k.Prezime) ||
                string.IsNullOrWhiteSpace(k.Username))
            {
                return BadRequest();
            }

            k.Id = id;
            userDbRepository.Update(k);

            return Ok(userDbRepository.GetById(id));
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            userDbRepository.Delete(id);
            return NoContent();
        }
    }
}
