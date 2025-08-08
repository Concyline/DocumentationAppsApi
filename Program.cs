using DocumentationAppsApi.Src.IServices;
using DocumentationAppsApi.Src.MongoConfig;
using DocumentationAppsApi.Src.Routes;
using DocumentationAppsApi.Src.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CUSTONS SERVICES
builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddHttpClient<IEmailService, EmailService>();

// 🔹 Adiciona política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTudo", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("PermitirTudo"); // aplica a política

app.MapGet("/", () =>
{
    return Results.Ok("v1");
});


// ROUTES
app.userRoutes();


//app.Urls.Add("http://+:80");

app.Run();

