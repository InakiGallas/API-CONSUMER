using ChallengeAPI.Data;
using ChallengeAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ChallengeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaldoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SaldoController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "ObtenerSaldo")]
        //[Authorize]
        public async Task<ActionResult<object>> ObtenerSaldo(string numeroTarjeta)
        {
            try
            {
                //var jwtToken = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                // Buscar la tarjeta por su número
                var tarjeta = await _context.Tarjetas.FirstOrDefaultAsync(t => t.NumeroTarjeta == numeroTarjeta);

                if (tarjeta == null)
                {
                    return NotFound("Tarjeta no encontrada");
                }

                // Consultar la información asociada a la tarjeta 
                var saldoActual = tarjeta.Saldo; 
                var ultimaExtraccion = tarjeta.UltimaExtraccion; 

                // Retornar la información como una respuesta JSON
                return new
                {
                    NumeroCuenta = tarjeta.NumeroTarjeta,
                    SaldoActual = saldoActual,
                    UltimaExtraccion = ultimaExtraccion
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error" + ex);
            }
        }
    }
}
