using FluentNHibernate.Mapping;
using GastuakApi.Modeloak;

namespace GastuakApi.Mapeoak
{
    public class ErabiltzaileaMap : ClassMap<Erabiltzailea>
    {
        public ErabiltzaileaMap()
        {
            Table("erabiltzaileak");

            Id(x => x.Id).Column("id").GeneratedBy.Identity();

            Map(x => x.Izena).Column("izena");

            Map(x => x.Abizena).Column("abizena");

            Map(x => x.SarreraRekurrentea).Column("sarrera_rekurrentea");

            Map(x => x.KontuNagusia_id).Column("kontu_nagusia_id");
            
            HasManyToMany(x => x.Familiak)
                .Table("erabiltzailea_familia")
                .ParentKeyColumn("erabiltzailea_id")
                .ChildKeyColumn("familia_id")
                .LazyLoad()
                .Cascade.All();

        }
    }
}
