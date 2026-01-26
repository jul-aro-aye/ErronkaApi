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
    public IActionResult LortuEskaerak()
    {
        var erantzuna = _repo.LortuEskaerak();

        return StatusCode(erantzuna.Code, erantzuna);
    }

    [HttpGet("{eskaeraId}/produktuak")]
    public IActionResult LortuEskaeraProduktuak(int eskaeraId)
    {
        var erantzuna = _repo.LortuEskaeraProduktuak(eskaeraId);

        return StatusCode(erantzuna.Code, erantzuna);
    }

    [HttpDelete("{eskaeraId}")]
    public IActionResult EzabatuEskaera(int eskaeraId)
    {
        var erantzuna = _repo.EzabatuEskaera(eskaeraId);

        if (erantzuna.Code == 200)
        {
            return Ok(erantzuna);
        }
        else
        {
            return BadRequest(erantzuna);
        }
    }

    [HttpGet("mahaiak/{mahaiaId}/kapazitatea")]
    public IActionResult LortuMahaiKapasitatea(int mahaiaId)
    {
        var erantzuna = _repo.LortuMahaiKapazitatea(mahaiaId);

        if (erantzuna.Code == 200)
            return Ok(erantzuna);
        else if (erantzuna.Code == 404)
            return NotFound(erantzuna);
        else
            return StatusCode(500, erantzuna);
    }

    [HttpPut("{eskaeraId}")]
    public IActionResult EguneratuEskaera(
    int eskaeraId,
    [FromBody] List<EskaeraProduktuaEditatuDTO> produktuak)
    {
        if (produktuak == null || !produktuak.Any())
        {
            return BadRequest(new ErantzunaDTO<string>
            {
                Code = 400,
                Message = "Ez duzu produkturik bidali",
                Datuak = new List<string>()
            });
        }

        var erantzuna = _repo.EguneratuEskaera(eskaeraId, produktuak);

        if (erantzuna.Code == 200)
            return Ok(erantzuna);
        else if (erantzuna.Code == 404)
            return NotFound(erantzuna);
        else if (erantzuna.Code == 400)
            return BadRequest(erantzuna);
        else
            return StatusCode(500, erantzuna);
    }

    [HttpPut("{eskaeraId}/sukaldea-egoera")]
    public IActionResult EguneratuSukaldeaEgoera(int eskaeraId, [FromBody] EskaeraSukaldeaEgoeraDTO dto)
    {
        if (dto == null)
        {
            return BadRequest(new ErantzunaDTO<string>
            {
                Code = 400,
                Message = "Datuak behar dira",
                Datuak = new List<string>()
            });
        }

        var erantzuna = _repo.EguneratuSukaldeaEgoera(eskaeraId, dto.SukaldeaEgoera);

        if (erantzuna.Code == 200)
            return Ok(erantzuna);
        else if (erantzuna.Code == 404)
            return NotFound(erantzuna);
        else if (erantzuna.Code == 400)
            return BadRequest(erantzuna);
        else
            return StatusCode(500, erantzuna);
    }

}
