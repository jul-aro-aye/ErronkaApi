using Microsoft.AspNetCore.Mvc;
using ErronkaApi.Repositorioak;
using ErronkaApi.DTOak;
using System.Linq;
namespace ErronkaApi.Kontrollerrak
{
    [ApiController]
    [Route("api/[controller]")]
    public class KategoriaKontrollerra : ControllerBase
    {
        private readonly KategoriaRepository _repo;
        public KategoriaKontrollerra(KategoriaRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var kategoriak = _repo.GetAll()
                                    .Select(k => new KategoriaDTO
                                    {
                                        id = k.id,
                                        izena = k.izena
                                    })
                                    .ToList();
            return Ok(kategoriak);
        }
    }
}
