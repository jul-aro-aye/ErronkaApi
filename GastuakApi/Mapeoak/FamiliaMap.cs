using FluentNHibernate.Mapping;
using GastuakApi.Modeloak;

namespace GastuakApi.Mapeoak
{
    public class FamiliaMap : ClassMap<Familia>
    {
        public FamiliaMap()
        {
            Table("familiak");

            Id(x => x.Id).Column("id").GeneratedBy.Identity();

            Map(x => x.Izena).Column("izena");

            HasManyToMany(x => x.Erabiltzaileak)
                .Table("erabiltzailea_familia")
                .ParentKeyColumn("familia_id")
                .ChildKeyColumn("erabiltzailea_id")
                .Inverse()
                .LazyLoad()
                .Cascade.All();


        }
    }
}
