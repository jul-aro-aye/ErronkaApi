using GastuakApi.Modeloak;
using NHibernate;

namespace GastuakApi.Repositorioak
{ 
    public class FamiliaRepository
    {
        private readonly NHibernate.ISession _session;

        public FamiliaRepository(ISessionFactory sessionFactory)
        {
            _session = sessionFactory.GetCurrentSession();
        }

        public void Add(Familia familia)
        {
            using var tx = _session.BeginTransaction();

            _session.Save(familia);

            tx.Commit();
        }

        public Familia? Get(int id, bool eager = false)
        {
            var query = _session.Query<Familia>()
                .Where(x => x.Id == id);   

            var familia = query.SingleOrDefault();
            return familia;

        }

        public IList<Familia> GetAll()
        {
            return _session.Query<Familia>().ToList();
        }

        public void Update(Familia familia)
        {
            using var tx = _session.BeginTransaction();

            _session.Update(familia);

            tx.Commit();
        }

        public void Delete(Familia familia)
        {
            using var tx = _session.BeginTransaction();

            _session.Delete(familia);

            tx.Commit();
        }

    }

}
