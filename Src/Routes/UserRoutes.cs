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

            route.MapPost("/recover", async (string email, IEmailService emailService) =>
            {
                var token = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();

                var html = $@"<!DOCTYPE html>
                                <html lang=""pt-BR"">
                                <head>
                                  <meta charset=""UTF-8"" />
                                  <title>Recuperação de Senha</title>
                                  <style>
                                    body {{
                                      font-family: 'Segoe UI', sans-serif;
                                      background-color: #f5f7fa;
                                      padding: 40px;
                                      color: #333;
                                    }}
                                    .container {{
                                      background-color: #ffffff;
                                      padding: 32px;
                                      border-radius: 10px;
                                      max-width: 600px;
                                      margin: auto;
                                      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
                                    }}
                                    .header {{
                                      text-align: center;
                                      margin-bottom: 24px;
                                    }}
                                    .header h2 {{
                                      color: #0a516d;
                                      margin-bottom: 10px;
                                    }}
                                    .token-box {{
                                      background-color: #e9f7fb;
                                      border: 2px dashed #018790;
                                      padding: 16px;
                                      text-align: center;
                                      font-size: 28px;
                                      font-weight: bold;
                                      color: #018790;
                                      margin: 20px 0;
                                      border-radius: 6px;
                                      letter-spacing: 2px;
                                    }}
                                    .content p {{
                                      font-size: 16px;
                                      line-height: 1.6;
                                      text-align: center;
                                    }}
                                    .button {{
                                      display: inline-block;
                                      padding: 14px 28px;
                                      margin: 30px auto;
                                      background-color: #018790;
                                      color: #ffffff !important;
                                      font-weight: bold;
                                      text-decoration: none;
                                      border-radius: 6px;
                                      font-size: 16px;
                                      text-align: center;
                                    }}
                                    .footer {{
                                      margin-top: 40px;
                                      font-size: 13px;
                                      color: #999;
                                      text-align: center;
                                    }}
                                  </style>
                                </head>
                                <body>
                                  <div class=""container"">
                                    <div class=""header"">
                                      <h2>Recuperação de Senha</h2>
                                      <p>Você solicitou redefinir sua senha.</p>
                                    </div>
                                    <div class=""content"">
                                      <p>Use o código abaixo:</p>
                                      <div class=""token-box"">{token}</div>
                                      <p>Ou clique no botão abaixo para redefinir sua senha diretamente:</p>
                                      <a href=""https://documentationapppwa.netlify.app/src/recuperacao.html?token={token}"" class=""button"">Redefinir Senha</a>
                                      <p>Este código é válido por <strong>10 minutos</strong>. Se você não solicitou esta ação, ignore este e-mail.</p>
                                    </div>
                                    <div class=""footer"">
                                      © 2025 - SeuApp. Todos os direitos reservados.
                                    </div>
                                  </div>
                                </body>
                                </html>";

                var success = await emailService.SendEmailAsync(email, "Recuperação de Senha", html);

                if (!success)
                    return Results.Problem("Erro ao enviar o email.");

                return Results.Ok(ApiResponse<object>.Ok(token, "Token enviado com sucesso, acesse seu email para modificar sua senha!"));
            });



        }
    }
}
