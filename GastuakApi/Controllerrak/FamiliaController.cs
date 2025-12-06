using GastuakApi.Repositorioak;
using GastuakApi.Modeloak;
using GastuakApi.DTOak;
using Microsoft.AspNetCore.Mvc;

namespace GastuakApi.Controllerrak
{
    [ApiController]
    [Route("api/[controller]")]
    public class FamiliaController: ControllerBase
    {
        private readonly FamiliaRepository _familiaRepo;

        public FamiliaController(
            FamiliaRepository familiaRepo)
        {
            _familiaRepo = familiaRepo;
        }
        

        // GET api/familia
        [HttpGet]
        public IActionResult GetAll(bool eager = false)
        {
            var familiak = _familiaRepo.GetAll();

            // Creamos la lista de DTOs
            var familiakDto = new List<FamiliaDto>();

            foreach (var familia in familiak)
            {
                if (eager)
                {
                    familiakDto.Add(new FamiliaDto
                    {
                        Id = familia.Id,
                        Izena = familia.Izena,
                        Erabiltzaileak = familia.Erabiltzaileak
                            .Select(e => new ErabiltzaileaDto
                            {
                                Id = e.Id,
                                Izena = e.Izena,
                                Abizena = e.Abizena
                            })
                            .ToList()
                    });
                }
                else
                {
                    // LAZY: no incluir usuarios
                    familiakDto.Add(new FamiliaDto
                    {
                        Id = familia.Id,
                        Izena = familia.Izena
                    });
                }
            }

            return Ok(familiakDto);
        }


        // POST api/familia
        [HttpPost]
        public IActionResult SortuFamilia([FromBody] FamiliaSortuDTO dto)
        {
            // Familia sortu
            var familia = new Familia
            {
                Izena = dto.Izena
            };

            _familiaRepo.Add(familia);

            return Ok(new
            {
                mezua = "Familia sortuta",
                familiaId = familia.Id
            });
        }

        // GET api/familia/{id}
        [HttpGet("{id}")]
        public IActionResult Get(int id, bool eager = false)
        {
            var familia = _familiaRepo.Get(id, eager);
            var familiaDto = new FamiliaDto { };

            if (familia == null)
                return NotFound(new { mezua = "Familia ez da existitzen" });
            if (eager) {
                familiaDto = new FamiliaDto
                {
                    Id = familia.Id,
                    Izena = familia.Izena,
                    Erabiltzaileak = familia.Erabiltzaileak
                    .Select(e => new ErabiltzaileaDto
                    {
                        Id = e.Id,
                        Izena = e.Izena,
                        Abizena = e.Abizena
                    })
                .ToList()
                };
            }else
            {
                familiaDto = new FamiliaDto
                {
                    Id = familia.Id,
                    Izena = familia.Izena
                };

            }
  

            return Ok(familiaDto);
        }

        // PUT api/familia/{id}
        [HttpPut("{id}")]
        public IActionResult EguneratuFamilia(int id, [FromBody] FamiliaSortuDTO dto)
        {
            var familia = _familiaRepo.Get(id);
            if (familia == null)
                return NotFound(new { mezua = "Familia ez da existitzen" });

            familia.Izena = dto.Izena;

            _familiaRepo.Update(familia);

            return Ok(new { mezua = "Familia eguneratuta" });
        }

        // PATCH api/familia/{id}
        [HttpPatch("{id}")]
        public IActionResult EguneratuZatia(int id, [FromBody] FamiliaPatchDTO dto)
        {
            var familia = _familiaRepo.Get(id);
            if (familia == null)
                return NotFound(new { mezua = "Familia ez da existitzen" });

            if (!string.IsNullOrWhiteSpace(dto.Izena))
                familia.Izena = dto.Izena;

            _familiaRepo.Update(familia);

            return Ok(new { mezua = "Familia zati batean eguneratuta" });
        }

        // DELETE api/familia/{id}
        [HttpDelete("{id}")]
        public IActionResult EzabatuFamilia(int id)
        {
            var familia = _familiaRepo.Get(id);
            if (familia == null)
                return NotFound(new { mezua = "Familia ez da existitzen" });

            _familiaRepo.Delete(familia);

            return Ok(new { mezua = "Familia ezabatua" });
        }


    }

    public class FamiliaSortuDTO
    {
        public string Izena { get; set; }
        public List<int> ErabiltzaileIds { get; set; } = new();
    }


    public class FamiliaPatchDTO
    {
        public string? Izena { get; set; }
    }
}
