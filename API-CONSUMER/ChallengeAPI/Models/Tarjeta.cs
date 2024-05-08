using System.ComponentModel.DataAnnotations;

namespace ChallengeAPI.Models
{
    public class Tarjeta
    {
        [Key] public int CardID { get; set; }
        public string NumeroTarjeta { get; set; }
        public string PIN { get; set; }
        public decimal Saldo { get; set; }
        public bool Bloqueada { get; set; }
        public DateTime? UltimaExtraccion { get; set; }
        public int Intentos { get; set; }
    }
}
