namespace GastuakApi.Modeloak
{
    public class Erabiltzailea
    {
        public virtual int Id { get; set; }
        public virtual string Izena { get; set; }
        public virtual string Abizena { get; set; }
        public virtual float SarreraRekurrentea { get; set; }
        public virtual int KontuNagusia_id { get; set; }
        public virtual IList<Familia> Familiak { get; set; } = new List<Familia>();

        public Erabiltzailea()
        {
        }

        public Erabiltzailea(string izena, string abizena, float sarrera) { 
            this.Izena = izena;
            this.Abizena = abizena;
            this.SarreraRekurrentea = sarrera;
        }
    }
}   
