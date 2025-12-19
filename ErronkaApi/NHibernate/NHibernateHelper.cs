using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using ErronkaApi.Mapeoak;
using NH = NHibernate;
using TPVBarra.Mapeoak;

namespace ErronkaApi.NHibernate
{
    public class NHibernateHelper
    {
        private static NH.ISessionFactory _sessionFactory;

        public static NH.ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                    InitializeSessionFactory();
                return _sessionFactory;
            }
        }
        private static void InitializeSessionFactory()
        {
            _sessionFactory = Fluently.Configure()
                .Database(
                    MySQLConfiguration.Standard
                        .ConnectionString(cs => cs
                            .Server("192.168.2.100") // 192.168.115.161  localhost
                            .Database("tpv") // tpv  erronka
                            .Username("admin") // admin  root   
                            .Password("Taldea4") // Taldea4  1MG2024
                        )
                )
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<ErabiltzaileaMap>();
                    m.FluentMappings.AddFromAssemblyOf<ProduktuaMap>();
                    m.FluentMappings.AddFromAssemblyOf<EskaeraMap>();
                    m.FluentMappings.AddFromAssemblyOf<MahaiaMap>();
                    m.FluentMappings.AddFromAssemblyOf<EskaeraMahaiakMap>();
                    m.FluentMappings.AddFromAssemblyOf<RolaMap>();
                })
                .ExposeConfiguration(cfg => { cfg.SetProperty("current_session_context_class", "thread_static"); }) // Ez dezan sortu taula exekuzio bakoitzeko
                .BuildSessionFactory();
        }
        public static NH.ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
