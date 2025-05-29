using drustvena_mreza.Model;
using drustvena_mreza.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace drustvena_mreza.Controllers
{
    [Route("api/korisnici")]
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private KorisnikRepository korisnikRepository = new KorisnikRepository();

        [HttpGet]
        public ActionResult<List<Korisnik>> GetAll()
        {
            //List<Korisnik> listaKorisnika = KorisnikRepository.Data.Values.ToList();
            //return Ok(listaKorisnika

            return Ok(GetAllFromDatabase());
        }

        [HttpGet("{id}")]
        public ActionResult<Korisnik> GetById(int id)
        {
            if (!KorisnikRepository.Data.ContainsKey(id))
            {
                return NotFound();
            }

            return Ok(KorisnikRepository.Data[id]);
        }

        [HttpPost]
        public ActionResult<Korisnik> Create([FromBody] Korisnik noviKorisnik)
        {
            if (string.IsNullOrWhiteSpace(noviKorisnik.Ime) ||
                string.IsNullOrWhiteSpace(noviKorisnik.Prezime) ||
                string.IsNullOrWhiteSpace(noviKorisnik.UserName))
            {
                return BadRequest();
            }

            noviKorisnik.Id = DodeliNoviId(KorisnikRepository.Data.Keys.ToList());
            KorisnikRepository.Data[noviKorisnik.Id] = noviKorisnik;
            korisnikRepository.Save();

            return Ok(noviKorisnik);
        }
        [HttpPut("{id}")]
        public ActionResult<Korisnik> Update([FromBody] Korisnik k, int id)
        {
            if (string.IsNullOrWhiteSpace(k.Ime) ||
                string.IsNullOrWhiteSpace(k.Prezime) ||
                string.IsNullOrWhiteSpace(k.UserName))
            {
                return BadRequest();
            }

            if (!KorisnikRepository.Data.ContainsKey(id))
            {
                return NotFound();
            }

            Korisnik korisnik = KorisnikRepository.Data[id];
            korisnik.Ime = k.Ime;
            korisnik.Prezime = k.Prezime;
            korisnik.UserName = k.UserName;
            korisnik.DatumRodjenja = k.DatumRodjenja;

            korisnikRepository.Save();

            return Ok(korisnik);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            if (!KorisnikRepository.Data.ContainsKey(id))
            {
                return NotFound();
            }

            KorisnikRepository.Data.Remove(id);
            korisnikRepository.Save();

            return NoContent();
        }

        private List<Korisnik> GetAllFromDatabase()
        {
            List<Korisnik> listaKorisnika = new List<Korisnik>();

            try
            {
                using SqliteConnection connection = new SqliteConnection("Data Source = database/database.db");
                connection.Open();

                string query = "SELECT * FROM Users";
                using SqliteCommand command = new SqliteCommand(query, connection);

                using SqliteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int id = Convert.ToInt32(reader["Id"]);
                    string userName = reader["Username"].ToString();
                    string name = reader["Name"].ToString();
                    string prezime = reader["Surname"].ToString();
                    DateOnly datumRodjenja = DateOnly.Parse(reader["Birthday"].ToString());

                    listaKorisnika.Add(new Korisnik(id, userName, name, prezime, datumRodjenja));
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Greska pri konekciji ili neispravni SQL upit: {ex.Message}");
            }
            catch(FormatException ex)
            {
                Console.WriteLine($"Greska u konverziji podataka iz baze: {ex.Message}");
            }
            catch(InvalidOperationException ex)
            {
                Console.WriteLine($"Greška u konverziji podataka iz baze: {ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Neočekivana greška: {ex.Message}");
            }

            return listaKorisnika;
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
