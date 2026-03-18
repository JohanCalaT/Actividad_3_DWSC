using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversidadAPI.Data;
using UniversidadAPI.Models;

namespace UniversidadAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UniversidadContext _context;

        public UsuariosController(UniversidadContext context)
        {
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            // Requisito: Obtener todos sin modificar SQL y ordenar por username usando LINQ
            var usuariosQuery = await _context.Usuarios.ToListAsync(); // SELECT *
            var usuariosOrdenados = usuariosQuery.OrderBy(u => u.Username).ToList(); // LINQ en memoria

            return Ok(usuariosOrdenados);
        }

        // GET: api/users/{username}
        [HttpGet("{username}")]
        public async Task<ActionResult<Usuario>> GetUsuario(string username)
        {
            var usuario = await _context.Usuarios.FindAsync(username);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            // Requisito: Validar username o DNI repetidos
            bool existeUsername = await _context.Usuarios.AnyAsync(u => u.Username == usuario.Username);
            if (existeUsername)
            {
                return Conflict(new { message = "El username ya existe." });
            }

            bool existeDni = await _context.Usuarios.AnyAsync(u => u.Dni == usuario.Dni);
            if (existeDni)
            {
                return Conflict(new { message = "El DNI ya existe." });
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { username = usuario.Username }, usuario);
        }

        // PUT: api/users/{username}
        [HttpPut("{username}")]
        public async Task<IActionResult> PutUsuario(string username, Usuario usuario)
        {
            if (username != usuario.Username)
            {
                return BadRequest(new { message = "El username en la URL no coincide con el objeto." });
            }

            // Validar DNI si está siendo actualizado a uno que ya pertenece a otro
            bool existeDni = await _context.Usuarios.AnyAsync(u => u.Dni == usuario.Dni && u.Username != username);
            if (existeDni)
            {
                return Conflict(new { message = "El DNI ya está en uso por otro usuario." });
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(username))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/users/{username}
        [HttpDelete("{username}")]
        public async Task<IActionResult> DeleteUsuario(string username)
        {
            var usuario = await _context.Usuarios.FindAsync(username);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(string username)
        {
            return _context.Usuarios.Any(e => e.Username == username);
        }
    }
}
