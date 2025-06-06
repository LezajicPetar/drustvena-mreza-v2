using drustvena_mreza.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace drustvena_mreza.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private PostDbRepository postDbRepository;

        public PostController(IConfiguration configuration)
        {
            postDbRepository = new PostDbRepository(configuration);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(postDbRepository.GetAll());
            }
            catch (Exception ex)
            {
                return Problem($"Dogodila se greska pri dobavljanju korisnika! {ex.Message}");
            }
        }
    }
}
