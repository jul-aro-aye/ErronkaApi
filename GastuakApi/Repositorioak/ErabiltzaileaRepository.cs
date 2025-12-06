using GastuakApi.Modeloak;
using NHibernate;

namespace GastuakApi.Repositorioak
{ 
    public class ErabiltzaileaRepository
    {
        private readonly NHibernate.ISession _session;

        public ErabiltzaileaRepository(ISessionFactory sessionFactory)
        {
            _session = sessionFactory.GetCurrentSession();
        }

        public void Add(Erabiltzailea erabiltzailea)
        {
            using var tx = _session.BeginTransaction();

            _session.Save(erabiltzailea);

            tx.Commit();
        }

        public Erabiltzailea? Get(int id, bool eager = false)
        {
            var query = _session.Query<Erabiltzailea>()
                .Where(x => x.Id == id);

            var erabiltzailea = query.SingleOrDefault();
            return erabiltzailea;

        }

        public IList<Erabiltzailea> GetAll()
        {
            return _session.Query<Erabiltzailea>().ToList();
        }

        public void Update(Erabiltzailea erabiltzailea)
        {
            using var tx = _session.BeginTransaction();

            _session.Update(erabiltzailea);

            tx.Commit();
        }

        public void Delete(Erabiltzailea erabiltzailea)
        {
            using var tx = _session.BeginTransaction();

            _session.Delete(erabiltzailea);

            tx.Commit();
        }

    }

}
