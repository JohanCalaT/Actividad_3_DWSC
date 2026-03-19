using Microsoft.AspNetCore.Mvc;
using UniversidadAPI.Models;
using UniversidadAPI.Services;

namespace UniversidadAPI.Controllers;

[ApiController]
public class UserController : Controller
{
    private readonly IUserService _svc;

    public UserController(IUserService svc) => _svc = svc;

    // ── 1. Lista de usuarios en JSON ─────────────────────────────────────
    [HttpGet("/users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _svc.GetAllAsync();
        return Ok(users);
    }

    // ── 2. Lista de usuarios en tabla HTML ───────────────────────────────
    [HttpGet("/userstable")]
    public async Task<IActionResult> GetUsersTable()
    {
        var users = await _svc.GetAllAsync();
        return View("UsersTable", users);
    }

    // ── 3. Un usuario por username en JSON ───────────────────────────────
    [HttpGet("/users/{username}")]
    public async Task<IActionResult> GetUser(string username)
    {
        var user = await _svc.GetByUsernameAsync(username);
        if (user is null) return NotFound(new { message = $"User '{username}' not found." });
        return Ok(user);
    }

    // ── 4. Detalle de un usuario en HTML (estilo diferente) ──────────────
    [HttpGet("/users/{username}/view")]
    public async Task<IActionResult> GetUserView(string username)
    {
        var user = await _svc.GetByUsernameAsync(username);
        if (user is null) return NotFound();
        return View("UserDetail", user);
    }

    // ── 5. Insertar usuario (sin duplicados) ─────────────────────────────
    [HttpPost("/users")]
    public async Task<IActionResult> InsertUser([FromBody] User user)
    {
        if (user is null) return BadRequest("Body vacío o formato inválido.");

        var inserted = await _svc.InsertAsync(user);
        if (!inserted)
            return Conflict(new { message = "Ya existe un usuario con ese username o DNI." });

        return StatusCode(201, new { message = "Usuario creado correctamente." });
    }
}
