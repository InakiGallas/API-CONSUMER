using ChallengeAPI.Data;
using ChallengeAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace ChallengeAPI.Repositories
{
    public interface IOperacionRepository
    {
        Task<IEnumerable<Operacion>> ObtenerOperacionesPaginadas(int cardId, int pagina, int tamañoPagina);
    }

    public class OperacionRepository : IOperacionRepository
    {
        private readonly AppDbContext _context;

        public OperacionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Operacion>> ObtenerOperacionesPaginadas(int cardId, int pagina, int tamañoPagina)
        {
            var operaciones = await _context.Operaciones
                .Where(o => o.CardID == cardId)
                .OrderByDescending(o => o.FechaOperacion)
                .Skip((pagina - 1) * tamañoPagina)
                .Take(tamañoPagina)
                .ToListAsync();

            return operaciones;
        }
    }
}
