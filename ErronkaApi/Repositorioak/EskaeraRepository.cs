using Api.DTOak;
using Api.Modeloak;
using ErronkaApi.DTOak;
using ErronkaApi.Modeloak;
using ErronkaApi.NHibernate;
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

        public ErantzunaDTO<string> SortuEskaera(EskaeraSortuDTO dto)
        {
            using var session = _sessionFactory.OpenSession();
            using var tx = session.BeginTransaction();
            try
            {
                Console.WriteLine($"Mahaia jasota: {dto.MahaiaId}");
                var mahaia = session.Get<Mahaia>(dto.MahaiaId);

                if (mahaia == null)
                    return new ErantzunaDTO<string>
                    {
                        Code = 400,
                        Message = "Mahaia ez da aurkitu",
                        Datuak = new List<string>()
                    };

                mahaia.egoera = "okupatuta";
                session.Update(mahaia);

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
                    return new ErantzunaDTO<string>
                    {
                        Code = 400,
                        Message = "Stock gabe dauden produktuak daude",
                        Datuak = produktuakStockGabe
                    };
                }

                var eskaera = new Eskaera
                {
                    erabiltzaileId = dto.ErabiltzaileId,
                    komensalak = dto.Komensalak,
                    egoera = "irekita",
                    sortzeData = DateTime.Now,
                    mahaia_id = dto.MahaiaId
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

                return new ErantzunaDTO<string>
                {
                    Code = 200,
                    Message = "Eskaera ongi sortu da",
                    Datuak = new List<string>()
                };
            }
            catch (Exception ex)
            {
                tx.Rollback();
                return new ErantzunaDTO<string>
                {
                    Code = 500,
                    Message = "Errore bat egon da: " + ex.Message,
                    Datuak = new List<string>()
                };
            }
        }

        public ErantzunaDTO<EskaeraDTO> LortuEskaerak(int erabiltzaileId)
        {
            using var session = _sessionFactory.OpenSession();
            try
            {
                var eskaerak = session.Query<Eskaera>()
                    .Where(e => e.erabiltzaileId == erabiltzaileId)
                    .OrderByDescending(e => e.sortzeData)
                    .ToList();

                var dtoak = eskaerak.Select(e => new EskaeraDTO
                {
                    Id = e.id,
                    Izena = $"Eskaera #{e.id} ({e.sortzeData:dd/MM/yyyy HH:mm})",
                    MahaiaId = e.mahaia_id,
                    Data = e.sortzeData.ToString("yyyy-MM-dd HH:mm")
                }).ToList();

                return new ErantzunaDTO<EskaeraDTO>
                {
                    Code = 200,
                    Message = "Eskaerak lortu dira",
                    Datuak = dtoak
                };
            }
            catch (Exception ex)
            {
                return new ErantzunaDTO<EskaeraDTO>
                {
                    Code = 500,
                    Message = "Errore bat egon da: " + ex.Message,
                    Datuak = new List<EskaeraDTO>()
                };
            }
        }

        public ErantzunaDTO<EskaeraProduktuaDTO> LortuEskaeraProduktuak(int eskaeraId)
        {
            using var session = _sessionFactory.OpenSession();
            try
            {
                var eskaera = session.Get<Eskaera>(eskaeraId);
                if (eskaera == null)
                {
                    return new ErantzunaDTO<EskaeraProduktuaDTO>
                    {
                        Code = 404,
                        Message = "Eskaera ez da aurkitu",
                        Datuak = new List<EskaeraProduktuaDTO>()
                    };
                }

                var dtoak = eskaera.EskaeraProduktuak.Select(ep => new EskaeraProduktuaDTO
                {
                    ProduktuaId = ep.Produktua.id,
                    ProduktuaIzena = ep.Produktua.izena,
                    PrezioUnitarioa = ep.PrezioUnitarioa
                }).ToList();

                return new ErantzunaDTO<EskaeraProduktuaDTO>
                {
                    Code = 200,
                    Message = "Produktuak lortu dira",
                    Datuak = dtoak
                };
            }
            catch (Exception ex)
            {
                return new ErantzunaDTO<EskaeraProduktuaDTO>
                {
                    Code = 500,
                    Message = "Errore bat egon da: " + ex.Message,
                    Datuak = new List<EskaeraProduktuaDTO>()
                };
            }
        }

        public ErantzunaDTO<int> LortuMahaiKapazitatea(int mahaiaId)
        {
            using var session = _sessionFactory.OpenSession();
            try
            {
                var mahaia = session.Get<Mahaia>(mahaiaId);

                if (mahaia == null)
                {
                    return new ErantzunaDTO<int>
                    {
                        Code = 404,
                        Message = "Mahaia ez da aurkitu",
                        Datuak = new List<int>()
                    };
                }

                return new ErantzunaDTO<int>
                {
                    Code = 200,
                    Message = "Mahaia lortu da arrakastaz",
                    Datuak = new List<int> { mahaia.kapazitatea }
                };
            }
            catch (Exception ex)
            {
                return new ErantzunaDTO<int>
                {
                    Code = 500,
                    Message = "Errore bat egon da: " + ex.Message,
                    Datuak = new List<int>()
                };
            }
        }

        public ErantzunaDTO<string> EzabatuEskaera(int eskaeraId)
        {
            using var session = _sessionFactory.OpenSession();
            using var tx = session.BeginTransaction();
            try
            {
                var eskaera = session.Get<Eskaera>(eskaeraId);
                if (eskaera == null)
                {
                    return new ErantzunaDTO<string>
                    {
                        Code = 404,
                        Message = "Eskaera ez da aurkitu",
                        Datuak = new List<string>()
                    };
                }

                if (eskaera.EskaeraMahaiak.Any())
                {
                    foreach (var em in eskaera.EskaeraMahaiak)
                    {
                        em.Mahaia.egoera = "libre";
                        session.Update(em.Mahaia);
                        session.Delete(em);
                    }
                }

                if (eskaera.EskaeraProduktuak.Any())
                {
                    foreach (var ep in eskaera.EskaeraProduktuak)
                    {
                        session.Delete(ep);
                    }
                }

                session.Delete(eskaera);

                tx.Commit();

                return new ErantzunaDTO<string>
                {
                    Code = 200,
                    Message = "Eskaera ezabatu da arrakastaz",
                    Datuak = new List<string>()
                };
            }
            catch (Exception ex)
            {
                tx.Rollback();
                return new ErantzunaDTO<string>
                {
                    Code = 500,
                    Message = "Errore bat egon da: " + ex.Message,
                    Datuak = new List<string>()
                };
            }
        }
    }
}
