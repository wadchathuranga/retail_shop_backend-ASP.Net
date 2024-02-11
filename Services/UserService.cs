using Microsoft.EntityFrameworkCore;
using retail_management.Data;
using retail_management.Dtos;
using retail_management.Models;

namespace retail_management.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;

        public UserService(AppDbContext db)
        {
            _db = db;
        }

        private bool VerifyPassword(string inputPassword, string storedPassword)
        {
            try
            {
                bool passwordMatch = BCrypt.Net.BCrypt.Verify(inputPassword, storedPassword);

                return passwordMatch;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<User> LoginAsync(LoginInputDto user)
        {
            try
            {
                var existingUser = await _db.Users.FirstOrDefaultAsync(u => u.email == user.email);

                if (existingUser != null && VerifyPassword(user.password, existingUser.password))
                {
                    return existingUser;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
