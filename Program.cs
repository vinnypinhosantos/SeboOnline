using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SeboOnline;
using SeboOnline.Data;
using SeboOnline.Services;

var builder = WebApplication.CreateBuilder(args);
ConfigureAuthentication(builder);
ConfigureMvc(builder);
ConfigureServices(builder);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EditUserPolicy", policy =>
    {
        policy.RequireAuthenticatedUser(); // Garante que o usuário esteja autenticado
        policy.RequireClaim("UserId", "{userId}"); // Verifica se o usuário possui a reivindicação 'UserId' com o ID correto
    });
    options.AddPolicy("DeleteUserPolicy", policy =>
    {
        policy.RequireAuthenticatedUser(); // Garante que o usuário esteja autenticado
        policy.RequireClaim("UserId", "{userId}"); // Verifica se o usuário possui a reivindicação 'UserId' com o ID correto
    });
});

var app = builder.Build();
LoadConfiguration(app);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

void LoadConfiguration(WebApplication app)
{
    Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
}
void ConfigureMvc(WebApplicationBuilder builder)
{
    builder
    .Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });
}
void ConfigureServices(WebApplicationBuilder builder)
{
    builder.Services.AddDbContext<SeboDataContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
    builder.Services.AddTransient<TokenService>();
    builder.Services.AddTransient<PasswordHashService>();
}