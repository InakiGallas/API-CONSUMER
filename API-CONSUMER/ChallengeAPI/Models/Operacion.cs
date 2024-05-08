using System.ComponentModel.DataAnnotations;

namespace ChallengeAPI.Models
{
    public class Operacion
    {
        [Key] public int OperationID { get; set; }
        public int CardID { get; set; }
        public TipoOperacion TipoOperacion { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaOperacion { get; set; }
    }
}
