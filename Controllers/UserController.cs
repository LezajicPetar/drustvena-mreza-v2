using drustvena_mreza.Model;
using drustvena_mreza.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Data.Sqlite;

namespace drustvena_mreza.Controllers
{
    [Route("api/korisnici")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserDbRepository userDbRepository;

        public UserController(IConfiguration configuration)
        {
            userDbRepository = new UserDbRepository(configuration);
        }

        [HttpGet]
        public ActionResult GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 2)
        {
            try
            {
                if(page < 1 || pageSize < 1)
                {
                    return BadRequest("Stranica i velicina stranice moraju biti veci od 1.");
                }
                Object result = new
                {
                    Data = userDbRepository.GetPaged(page, pageSize),
                    TotalCount = userDbRepository.CountAll()
                };
                return Ok(result);
            }
            catch(Exception ex)
            {
                return Problem($"Dogodila se greska pri dobavljanju korisnika! {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {
            try
            {
                if (userDbRepository.GetById(id) == null)
                {
                    return NotFound($"Book with {id} not found.");
                }
                return Ok(userDbRepository.GetById(id));
            }
            catch ( Exception ex )
            {
                return Problem($"Dogodio se problem kod pretrage. {ex.Message}");
            }
        }

        [HttpPost]
        public ActionResult<Korisnik> Create([FromBody] Korisnik noviKorisnik)
        {
            try
            {
                if (noviKorisnik == null ||
                    string.IsNullOrWhiteSpace(noviKorisnik.Ime) ||
                    string.IsNullOrWhiteSpace(noviKorisnik.Prezime) ||
                    string.IsNullOrWhiteSpace(noviKorisnik.Username))
                {
                    return BadRequest();
                }
                return Ok(userDbRepository.Create(noviKorisnik));
            }
            catch (Exception ex)
            {
                return Problem($"Dogodila se greska prilikom kreiranja korisnika. {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update([FromBody] Korisnik k, int id)
        {
            try
            {
                if (
                 string.IsNullOrWhiteSpace(k.Ime) ||
                 string.IsNullOrWhiteSpace(k.Prezime) || 
                 string.IsNullOrWhiteSpace(k.Username))
                {
                    return BadRequest("Invalid data!");
                }
                else if(userDbRepository.Update(k) == null)
                {
                    return NotFound("Korisnik nije pronadjen, doslo je do greske.");
                }

                k.Id = id;
                return Ok(k);
            }
            catch(Exception ex)
            {
                return Problem($"Dogodila se greska prilikom azuriranja korisnika. {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                return userDbRepository.Delete(id) ? NoContent() : NotFound();
            }
            catch(Exception ex)
            {
                return Problem($"Dogodila se greskom prilikom brisanja. {ex.Message}");
            }
        }
    }
}
