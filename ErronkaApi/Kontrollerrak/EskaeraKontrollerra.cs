using Api.DTOak;
using Api.Modeloak;
using Microsoft.AspNetCore.Mvc;
using ErronkaApi.Repositorioak;
using ErronkaApi.NHibernate;
using System.Linq;
using ErronkaApi.DTOak;

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

        var erantzuna = _repo.SortuEskaera(dto);

        if (erantzuna.Code == 200)
        {
            return Ok(erantzuna);
            
        }
        else {
            return BadRequest(erantzuna);
        }
    }

    [HttpGet]
    public IActionResult LortuEskaerak([FromQuery] int erabiltzaileId)
    {
        var erantzuna = _repo.LortuEskaerak(erabiltzaileId);

        return StatusCode(erantzuna.Code, erantzuna);
    }

    [HttpGet("{eskaeraId}/produktuak")]
    public IActionResult LortuEskaeraProduktuak(int eskaeraId)
    {
        var erantzuna = _repo.LortuEskaeraProduktuak(eskaeraId);

        return StatusCode(erantzuna.Code, erantzuna);
    }
}
