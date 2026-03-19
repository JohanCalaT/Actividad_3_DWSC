using Microsoft.EntityFrameworkCore;
using UniversidadAPI.Data;
using UniversidadAPI.Models;

namespace UniversidadAPI.Services;

public class UserService : IUserService
{
    private readonly UniversidadContext _context;

    public UserService(UniversidadContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        // Requisito: Ordenar por edad (age)
        return await _context.Usuarios
            .OrderBy(u => u.Age)
            .ToListAsync();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Usuarios.FindAsync(username);
    }

    public async Task<bool> InsertAsync(User user)
    {
        // Verificar duplicados por username o dni
        bool exists = await _context.Usuarios.AnyAsync(u => u.Username == user.Username || u.Dni == user.Dni);
        if (exists) return false;

        _context.Usuarios.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }
}
