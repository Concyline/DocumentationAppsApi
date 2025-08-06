using MongoDB.Bson.Serialization.Attributes;

namespace DocumentationAppsApi.Src.Request
{
    public record UserRequest(string UserName, string Email, string Password, string Role);
}
