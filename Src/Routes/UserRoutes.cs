using DocumentationAppsApi.Src.IServices;
using DocumentationAppsApi.Src.Models;
using DocumentationAppsApi.Src.Request;
using DocumentationAppsApi.Src.Responses;

namespace DocumentationAppsApi.Src.Routes
{
    public static class UserRoutes
    {
        public static void userRoutes(this IEndpointRouteBuilder app)
        {
            var route = app.MapGroup("/user");

            route.MapPost("/login", async (LoginRequest request, IUserService userService) =>
            {
                var user = await userService.AuthenticateAsync(request.Email, request.Password);

                if (user == null)
                    return Results.Ok(ApiResponse<object>.Unauthorized());

                return Results.Ok(ApiResponse<object>.Ok(user, "Login realizado com sucesso"));
            });

            route.MapPost("/", async (UserRequest userRequest, IUserService userService) =>
            {

                User newUser = new User()
                {
                    UserName = userRequest.UserName,
                    Email = userRequest.Email,
                    PasswordHash = userRequest.Password,
                    Role = userRequest.Role

                };

                var (success, message, createdUser) = await userService.RegisterUserAsync(newUser);

                if (!success)
                {
                    return Results.Conflict(ApiResponse<object>.Fail(message));
                }

                return Results.Created($"/user/{createdUser.Id}", ApiResponse<object>.Ok(createdUser, message!));
            });

            route.MapPut("/{id}", async (string id, UserRequest userRequest, IUserService userService) =>
            {
                User updatedUser = new User()
                {
                    UserName = userRequest.UserName,
                    Email = userRequest.Email,
                    PasswordHash = userRequest.Password,
                    Role = userRequest.Role
                };

                var (success, message, user) = await userService.UpdateUserAsync(id, updatedUser);

                if (!success)
                    return Results.NotFound(ApiResponse<object>.Fail(message!));

                return Results.Ok(ApiResponse<object>.Ok(user!, message!));
            });

            route.MapDelete("/{id}", async (string id, IUserService userService) =>
            {
                var (success, message) = await userService.DeleteUserAsync(id);

                if (!success)
                    return Results.NotFound(ApiResponse<object>.Fail(message!));

                return Results.Ok(ApiResponse<object>.Ok(id, message!));
            });


        }
    }
}
