using drustvena_mreza.Model;
using drustvena_mreza.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.ObjectPool;

namespace drustvena_mreza.Controllers
{
    [Route("api/groups/{groupId}/users")]
    [ApiController]
    public class GroupUserController : ControllerBase
    {
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


    }

}
