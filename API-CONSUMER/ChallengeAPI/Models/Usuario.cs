using System.ComponentModel.DataAnnotations;

namespace ChallengeAPI.Models
{
    public class Usuario 
    {
        [Key] public int UserId { get; set; }
        public string? Nombre { get; set; }
        public int CardID { get; set; }
    }
}
