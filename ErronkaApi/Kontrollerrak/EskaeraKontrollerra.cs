using Api.DTOak;
using Api.Modeloak;
using Microsoft.AspNetCore.Mvc;
using ErronkaApi.Repositorioak;
using ErronkaApi.NHibernate;
using System.Linq;

[ApiController]
[Route("api/eskaerak")]
public class EskaeraKontrollerra : ControllerBase
{
    private readonly EskaeraRepository _repo;

    public EskaeraKontrollerra()
    {
        _repo = new EskaeraRepository(NHibernateHelper.SessionFactory);
    }

    [HttpPost]
    public IActionResult SortuEskaera([FromBody] EskaeraSortuDTO dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var eskaera = _repo.SortuEskaera(dto);

        return Ok(new
        {
            eskaera.id,
            eskaera.komensalak,
            eskaera.egoera,
            eskaera.sortzeData,
            Produktuak = eskaera.EskaeraProduktuak.Select(p => new
            {
                p.Produktua.id,
                p.Kantitatea,
                p.PrezioUnitarioa,
                p.Guztira
            })
        });

    }
}
