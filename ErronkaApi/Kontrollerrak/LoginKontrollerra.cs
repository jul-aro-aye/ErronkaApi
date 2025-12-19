using ErronkaApi.DTOak;
using ErronkaApi.Modeloak;
using ErronkaApi.Repositorioak;
using Microsoft.AspNetCore.Mvc;

namespace ErronkaApi.Controlerrak
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginKontrollera : ControllerBase
    {
        private readonly ErabiltzaileaRepository _repo;

        public LoginKontrollera(ErabiltzaileaRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]

        public IActionResult Login([FromBody] LoginDTO loginDto)
        {
            var erabiltzailea = _repo.Login(loginDto.erabiltzailea, loginDto.pasahitza);
            if (erabiltzailea == null)
            {
                return Unauthorized(new { mezua = "Erabiltzaile edo pasahitz okerra." });
            }

            return Ok(erabiltzailea);
        }

    }
}
