using DommiArts.API.Data;
using Microsoft.EntityFrameworkCore;

// Importando FluentValidation e FluentValidation.AspNetCore
using FluentValidation;
using FluentValidation.AspNetCore;

// Importando Authentication e JWT
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuração dos serviços:
builder.Services.AddControllers();  //usando os Controllers tradicionais

// Configuração do FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();



builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "DommiArts API",
        Version = "v1"
    });

    // Configura o esquema de segurança Bearer para JWT
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header usando o esquema Bearer. 
                        Exemplo: Bearer {token}",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});



builder.Services.AddDbContext<DommiArtsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração do CORS para permitir requisições de qualquer origem
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Configuração do AutoMapper

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // Verifica se a chave JWT está configurada
        var jwtKey = builder.Configuration["Jwt:Key"]; // Chave JWT definida no appsettings.json
        if (string.IsNullOrEmpty(jwtKey)) // Verifica se a chave JWT é nula ou vazia
            throw new ArgumentNullException("Jwt:Key", "JWT Key cannot be null or empty. Please check your configuration."); // Lança uma exceção se a chave JWT não estiver configurada

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin Only", policy => policy.RequireRole("Admin"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DommiArtsDbContext>();
    var configuration = services.GetRequiredService<IConfiguration>();

    await DataSeeder.SeedAdminUserAsync(context, configuration);
}


// Middleware

app.UseMiddleware<DommiArts.API.Middlewares.ExceptionMiddleware>(); // Usando Middleware para resposta ser um JSON padronizado de erro

// Pipeline HTTP:
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "DommiArts API V1");
        options.RoutePrefix = string.Empty; // Define a rota base para acessar o Swagger
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll"); // Aplicando a política de CORS

app.UseAuthentication(); // Ativando a autenticação

app.UseAuthorization();

app.MapControllers(); // ativando os controllers

app.Run();
