using ErronkaApi.DTOak;
using ErronkaApi.Repositorioak;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class MahaiakController : ControllerBase
{
    private readonly MahaiaRepository _mahaiaService;

    public MahaiakController(MahaiaRepository mahaiaService)
    {
        _mahaiaService = mahaiaService;
    }

    [HttpGet("libre")]
    public async Task<ActionResult<ErantzunaDTO<MahaiaDTO>>> LortuMahaiLibre()
    {
        var erantzuna = await _mahaiaService.LortuMahaiLibreAsync();

        if (erantzuna.Code != 200)
            return StatusCode(erantzuna.Code, erantzuna);

        return Ok(erantzuna);
    }

}