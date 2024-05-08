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

namespace ChallengeAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private IConfiguration config;
        private readonly AppDbContext _context;
        public LoginController(AppDbContext context,IConfiguration config)
        {
            _context = context;
            this.config = config;
        }
      

       

        [HttpGet(Name = "Login")]
        public async Task<ActionResult<string>> Login(String numeroTarjeta,String PIN)
        {
            
            try
            {
                var tarjeta = await _context.Tarjetas.FirstOrDefaultAsync(t => t.NumeroTarjeta == numeroTarjeta);
                

                if (tarjeta == null || tarjeta.Bloqueada)
                {
                    return NotFound("Tarjeta no encontrada o bloqueada");
                }
                //validar pin
                if (tarjeta.PIN != PIN)
                {
                    // Incrementar contador de intentos
                    tarjeta.Intentos++;
                    if (tarjeta.Intentos >= 4)
                    {
                        tarjeta.Bloqueada = true;
                    }
                    await _context.SaveChangesAsync();

                    return BadRequest("PIN inválido");
                }

                // Restablecer contador de intentos si el PIN es correcto
                tarjeta.Intentos = 0;
                await _context.SaveChangesAsync();

                var token = GenerateToken(tarjeta);
                return Ok(new { token });
            }
           
            catch (Exception ex)
            {

                return ex.ToString();
            }
               
        }

        private string GenerateToken(Tarjeta tarjeta) 
        {
            var jwt = config.GetSection("Jwt").Get<Jwt>();
            
            var claims = new[]
            {
           
                new Claim("NumeroTarjeta", tarjeta.NumeroTarjeta),//nrotarjeta,
                new Claim("PIN", tarjeta.PIN)//PIN,

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.Aes128CbcHmacSha256);
            
            var token = new JwtSecurityToken(
                                claims: claims,
                                expires: DateTime.Now.AddMinutes(60),
                                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

            

        }



    
    }
}
