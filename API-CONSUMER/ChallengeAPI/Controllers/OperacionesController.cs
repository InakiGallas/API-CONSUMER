using ChallengeAPI.Models;
using ChallengeAPI.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ChallengeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OperacionesController : ControllerBase
    {
       private readonly IOperacionRepository _operacionRepository;

        public OperacionesController(IOperacionRepository operacionRepository)
        {
            _operacionRepository = operacionRepository;
        }

        [HttpGet(Name = "Operaciones")]
        public async Task<ActionResult<IEnumerable<Operacion>>> ObtenerOperaciones([FromQuery] int cardId, [FromQuery] int pagina = 1)
        {
            try
            {
                int tamañoPagina = 10; // Tamaño de la página por defecto
                var operacionesPaginadas = await _operacionRepository.ObtenerOperacionesPaginadas(cardId, pagina, tamañoPagina);
                return Ok(operacionesPaginadas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error" + ex);
            }
        }
    }
}
