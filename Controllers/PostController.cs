using drustvena_mreza.Models;
using drustvena_mreza.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace drustvena_mreza.Controllers
{
    [Route("api")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private PostDbRepository postDbRepository;

        public PostController(IConfiguration configuration)
        {
            postDbRepository = new PostDbRepository(configuration);
        }

        [HttpGet("posts")]
        public IActionResult GetAll()
        {
            try
            {
                return Ok(postDbRepository.GetAll());
            }
            catch (Exception ex)
            {
                return Problem($"Dogodila se greska pri dobavljanju korisnika. {ex.Message}");
            }
        }

        [HttpPost("users/userId/posts")]
        public IActionResult Post([FromBody] Post post)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(post.Content) ||
                    string.IsNullOrWhiteSpace(post.Date.ToString()))
                {
                    return BadRequest("Sadrzaj i datum su obavezni.");
                }
                if (!postDbRepository.UserExists(post.UserId))
                {
                    return NotFound("Korisnik nije pronadjen.");
                }

                return Ok(postDbRepository.Create(post));
            }
            catch(Exception ex)
            {
                return Problem($"Dogodila se greska: {ex.Message}");
            }
        }

        [HttpDelete("posts/postId")]
        public IActionResult Delete(int postId)
        {
            try
            {
                return postDbRepository.Delete(postId) ? NoContent() : NotFound();
            }
            catch(Exception ex)
            {
                return Problem($"Dogodila se greska. {ex.Message}");
            }
        }
    }
}
