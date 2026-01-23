using ErronkaApi.DTOak;
using ErronkaApi.Modeloak;
using ErronkaApi.Repositorioak;
using Microsoft.AspNetCore.Mvc;

namespace ErronkaApi.Controlerrak
{
    [ApiController]
    [Route("api/Logina")]
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
                return Unauthorized(new ErantzunaDTO<object>
                {
                    Code = 401,
                    Message = "Erabiltzaile edo pasahitz okerra.",
                    Datuak = null
                });
            }

            var erabiltzaileDatuak = new Erabiltzailea
            {
                id = erabiltzailea.id,
                erabiltzailea = erabiltzailea.erabiltzailea,
                emaila = erabiltzailea.emaila,
                ezabatua = erabiltzailea.ezabatua,
                txat = erabiltzailea.txat,
                rola = new Rola { id = erabiltzailea.rola.id }
            };

            return Ok(new ErantzunaDTO<Erabiltzailea>
            {
                Code = 200,
                Message = "Login ondo eginda",
                Datuak = new List<Erabiltzailea> { erabiltzaileDatuak }
            });

        }

    }
}
