using Api.DTOak;
using Api.Modeloak;
using ErronkaApi.Modeloak;
using NHibernate;
using System;

namespace ErronkaApi.Repositorioak
{
    public class EskaeraRepository
    {
        private readonly ISessionFactory _sessionFactory;

        public EskaeraRepository(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public Eskaera SortuEskaera(EskaeraSortuDTO dto)
        {
            using var session = _sessionFactory.OpenSession();
            using var tx = session.BeginTransaction();

            var mahaia = session.Get<Mahaia>(1);

            var eskaera = new Eskaera
            {
                erabiltzaileId = dto.ErabiltzaileId,
                komensalak = dto.Komensalak,
                egoera = "irekita",
                sortzeData = DateTime.Now,
                mahaia_id = 1
            };

            if (mahaia != null)
            {
                var eskaeraMahaiak = new EskaeraMahaiak
                {
                    Eskaera = eskaera,
                    Mahaia = mahaia
                };
                eskaera.EskaeraMahaiak.Add(eskaeraMahaiak);
                mahaia.EskaeraMahaiak.Add(eskaeraMahaiak);
            }

            session.Save(eskaera);
            tx.Commit();

            return eskaera;
        }
    }
}
