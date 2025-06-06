namespace drustvena_mreza.Model
{
    public class Group
    {
        public int Id { get; set; }
        public string Ime { get; set; }
        public DateOnly DatumOsnivanja { get; set; }
        public List<User> ListaKorisnika { get; set; }

        public Group(int id, string ime, DateOnly datumOsnivanja)
        {
            this.Id = id;
            this.Ime = ime;
            this.DatumOsnivanja = datumOsnivanja;
            ListaKorisnika = new List<User>();
        }

    }
}
