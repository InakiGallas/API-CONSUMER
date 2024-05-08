using ChallengeAPI.Data;
using ChallengeAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChallengeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RetiroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RetiroController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost(Name = "RealizarRetiro")]
       
        public async Task<ActionResult<object>> RealizarRetiro(string numeroTarjeta, decimal monto)
        {
            try
            {
                // Buscar la tarjeta por su número
                var tarjeta = await _context.Tarjetas.FirstOrDefaultAsync(t => t.NumeroTarjeta == numeroTarjeta);

                if (tarjeta == null)
                {
                    return NotFound("Tarjeta no encontrada");
                }

                // Verificar si el monto a retirar es superior al saldo disponible
                if (monto > tarjeta.Saldo || monto <= 0)
                {
                    return BadRequest("Monto a retirar invalido");
                }

                // Actualizar el saldo de la tarjeta
                tarjeta.Saldo -= monto;
                tarjeta.UltimaExtraccion = DateTime.Now;
                await _context.SaveChangesAsync();

                // Retornar un resumen de la operación realizada
                return new
                {   
                    Message = "Operacion Realizada",
                    NumeroCuenta = tarjeta.NumeroTarjeta,
                    MontoRetirado = monto,
                    SaldoActual = tarjeta.Saldo,
                    FechaExtraccion = tarjeta.UltimaExtraccion
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error" + ex);
            }
        }
    }
}
