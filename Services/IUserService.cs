using retail_management.Dtos;
using retail_management.Models;

namespace retail_management.Services
{
    public interface IUserService
    {
        // Login to system
        Task<User> LoginAsync(LoginInputDto user);
    }
}
