using DocumentationAppsApi.Src.Models;

namespace DocumentationAppsApi.Src.IServices
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string email, string password);
        Task<(bool Success, string? Message, User? User)> RegisterUserAsync(User newUser);

        Task<(bool Success, string? Message, User? User)> UpdateUserAsync(string id, User updatedUser);

        Task<(bool Success, string? Message)> DeleteUserAsync(string id);
    }
}
