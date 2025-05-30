namespace drustvena_mreza.Model
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public DateOnly DatumRodjenja { get; set; }

        public Korisnik(int id, string userName, string ime, string prezime, DateOnly datumRodjenja)
        {
            this.Id = id;
            this.Username = userName;
            this.Ime = ime;
            this.Prezime = prezime;
            this.DatumRodjenja = datumRodjenja;
        }
    }
}
