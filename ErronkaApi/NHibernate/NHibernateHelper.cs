using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using ErronkaApi.Mapeoak;
using NH = NHibernate;

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
                            .Server("localhost") // 192.168.115.161  localhost
                            .Database("tpv") // tpv
                            .Username("root") // admin root
                            .Password("1MG2024") // Taldea4 1MG2024
                        )
                )
                .Mappings(m =>
                {
                    m.FluentMappings.AddFromAssemblyOf<ErabiltzaileaMap>();
                })
                .ExposeConfiguration(cfg =>
                {
                    cfg.SetProperty("current_session_context_class", "call"); // Ez dezan sortu taula exekuzio bakoitzeko
                })
                .BuildSessionFactory();
        }
        public static NH.ISession OpenSession()
        {
            return SessionFactory.OpenSession();
        }
    }
}
