using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using GastuakApi.Mapeoak;
using GastuakApi.Modeloak;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace GastuakApi
{
    public class NHibernateHelper
    {
        private static ISessionFactory _sessionFactory;

        public static ISessionFactory SessionFactory =>
            _sessionFactory ??= CreateSessionFactory();

        private static ISessionFactory CreateSessionFactory()
        {
            var config = Fluently.Configure()
                .Database(MySQLConfiguration.Standard
                    .ConnectionString("Server=localhost;Port=3306;Database=erronka;Uid=root;Pwd=1MG2024;"))
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<FamiliaMap>();
                })
                .ExposeConfiguration(cfg =>
                {
                    cfg.SetProperty("current_session_context_class", "async_local");
                })
                .BuildConfiguration();

            dbBirSortu(config);
            //dbEguneratu(config);

            return config.BuildSessionFactory();
        }

        public static void dbEguneratu(NHibernate.Cfg.Configuration config) {
            //Eguneratu
        
            SchemaUpdate schemaUpdate = new SchemaUpdate(config);
            schemaUpdate.Execute(false, true);
            
        }

        public static void dbBirSortu(NHibernate.Cfg.Configuration config) {
            //Eguneratu
            var schemaExport = new SchemaExport(config);
            schemaExport.Drop(true, true);    // Ezabatu
            schemaExport.Create(true, true);  // Sortu
            datuakSortu(config);
        }

        public static void datuakSortu(NHibernate.Cfg.Configuration config)
        {
            // HEMEN: Datu hasierakoak txertatu
            using (var session = config.BuildSessionFactory().OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Save(new Familia { Izena = "Tolosa" });
                session.Save(new Familia { Izena = "Sebastian" });
                session.Save(new Familia { Izena = "Toledo" });

                transaction.Commit();
            }
        }
    }
}
