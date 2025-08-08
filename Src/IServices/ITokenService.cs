using DocumentationAppsApi.Src.Models;

namespace DocumentationAppsApi.Src.IServices
{
    public interface ITokenService
    {
        Task SaveTokenAsync(Guid userId, string token, DateTime expiresAt);
        Task<User?> ValidateTokenAsync(string token);
        Task InvalidateTokenAsync(string token);
    }
}
