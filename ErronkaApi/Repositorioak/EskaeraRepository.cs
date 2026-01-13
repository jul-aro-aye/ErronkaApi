using Api.DTOak;
using Api.Modeloak;
using ErronkaApi.DTOak;
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

        public ErantzunaDTO SortuEskaera(EskaeraSortuDTO dto)
        {
            using var session = _sessionFactory.OpenSession();
            using var tx = session.BeginTransaction();
            try
            {
                var mahaia = session.Get<Mahaia>(1);

                if (mahaia == null)
                    return new ErantzunaDTO
                    {
                        Code = 400,
                        Message = "Mahaia ez da aurkitu",
                        ProduktuakStockGabe = new List<string>()
                    };

                var produktuakStockGabe = new List<string>();

                foreach (var p in dto.Produktuak)
                {
                    var produktua = session.Get<Produktua>(p.ProduktuaId, LockMode.Upgrade);
                    if (produktua.stock_aktuala < p.Kantitatea)
                    {
                        produktuakStockGabe.Add(produktua.izena);
                    }
                }
                if (produktuakStockGabe.Any())
                {
                    ErantzunaDTO erantzuna = new ErantzunaDTO
                    {
                        Code = 400,
                        Message = "Stock gabe dauden produktuak daude",
                        ProduktuakStockGabe = produktuakStockGabe
                    };
                    return erantzuna;
                }

                var eskaera = new Eskaera
                {
                    erabiltzaileId = dto.ErabiltzaileId,
                    komensalak = dto.Komensalak,
                    egoera = "irekita",
                    sortzeData = DateTime.Now,
                    mahaia_id = 1
                };

                var eskaeraMahaiak = new EskaeraMahaiak
                {
                    Eskaera = eskaera,
                    Mahaia = mahaia
                };
                    
                eskaera.EskaeraMahaiak.Add(eskaeraMahaiak);
                mahaia.EskaeraMahaiak.Add(eskaeraMahaiak);

                foreach (var p in dto.Produktuak)
                {   
                    var produktua = session.Get<Produktua>(p.ProduktuaId, LockMode.Upgrade);
                    produktua.stock_aktuala -= p.Kantitatea;
                    session.Update(produktua);

                    var ep = new EskaeraProduktuak
                    {
                        Eskaera = eskaera,
                        Produktua = produktua,
                        Kantitatea = p.Kantitatea,
                        PrezioUnitarioa = produktua.prezioa,
                        Guztira = produktua.prezioa * p.Kantitatea
                    };
                    eskaera.EskaeraProduktuak.Add(ep);
                }
                
                session.Save(eskaera);
                tx.Commit();

                IList<object> eskaerak = new List<object> { eskaera };

                return new ErantzunaDTO
                {
                    Code = 200,
                    Message = "Eskaera ongi sortu da",
                    ProduktuakStockGabe = new List<string>()
                };
            }
            catch (Exception ex)
            {
                tx.Rollback();
                return new ErantzunaDTO
                {
                    Code = 500,
                    Message = "Errore bat egon da: " + ex.Message,
                    ProduktuakStockGabe = new List<string>()
                };
            }
        }
    }
}
