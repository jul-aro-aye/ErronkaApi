using System.Text.Json.Serialization;

namespace GastuakApi.Modeloak
{
    public class Familia
    {
        public virtual int Id { get; set; }
        public virtual string Izena { get; set; }
        public virtual IList<Erabiltzailea> Erabiltzaileak { get; set; } = new List<Erabiltzailea>();

        public Familia()
        {
        }

        public Familia(string izena) { 
            this.Izena = izena;
        }
    }
}
