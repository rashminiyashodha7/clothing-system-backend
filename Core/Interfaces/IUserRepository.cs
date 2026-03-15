
using GlamoraApi.DTOs;
using GlamoraApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IUserRepository
{
    
    Task<User> GetUserByIdAsync(int id);

    Task<User> GetUserByEmailAsync(string email);
    
    Task<List<User>> GetAllUsersAsync();

    Task AddUserAsync(User user);

    Task UpdateUserAsync(User user);

    Task DeleteUserAsync(int id);
}