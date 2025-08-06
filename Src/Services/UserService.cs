using DocumentationAppsApi.Src.IServices;
using DocumentationAppsApi.Src.Models;
using DocumentationAppsApi.Src.Util;
using MongoDB.Driver;

namespace DocumentationAppsApi.Src.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _userCollection;

        public UserService(IMongoCollection<User> userCollection)
        {
            _userCollection = userCollection;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
            if (user == null)
                return null;

            return PasswordHelper.VerifyPassword(password, user.PasswordHash) ? user : null;
        }

        public async Task<(bool Success, string? Message, User? User)> RegisterUserAsync(User newUser)
        {
            var exists = await _userCollection.Find(u => u.Email == newUser.Email).AnyAsync();
            if (exists)
                return (false, "Email já registrado", null);

            newUser.PasswordHash = PasswordHelper.HashPassword(newUser.PasswordHash);
            newUser.CreatedAt = DateTime.UtcNow;

            await _userCollection.InsertOneAsync(newUser);
            return (true, "Usuário registrado com sucesso", newUser);
        }

        public async Task<(bool Success, string? Message, User? User)> UpdateUserAsync(string id, User updatedUser)
        {
            var user = await _userCollection.Find(u => u.Id == id).FirstOrDefaultAsync();
            if (user == null)
                return (false, "User not found", null);

            // Opcional: evitar atualizar email para um já existente (validar se precisa)
            var emailExists = await _userCollection.Find(u => u.Email == updatedUser.Email && u.Id != id).AnyAsync();
            if (emailExists)
                return (false, "Email already in use by another user", null);

            // Atualizar campos - adapte conforme seu modelo
            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;
            user.Role = updatedUser.Role;

            // Se quiser atualizar senha, pode fazer assim:
            if (!string.IsNullOrEmpty(updatedUser.PasswordHash))
            {
                user.PasswordHash = PasswordHelper.HashPassword(updatedUser.PasswordHash);
            }

            var filter = Builders<User>.Filter.Eq(u => u.Id, id);
            var result = await _userCollection.ReplaceOneAsync(filter, user);

            if (result.ModifiedCount == 0)
                return (false, "Update failed", null);

            return (true, "User updated successfully", user);
        }

        public async Task<(bool Success, string? Message)> DeleteUserAsync(string id)
        {
            var result = await _userCollection.DeleteOneAsync(u => u.Id == id);
            if (result.DeletedCount == 0)
                return (false, "User not found or already deleted");

            return (true, "User deleted successfully");
        }
    }
}
