using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MinimalApi.Configure;
using MinimalApi.Data.Db;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Services;
using MinimalApi.Domain.ViewModels;
using MinimalApi.Interface;
using MinimalApi.ViewModels;
using Microsoft.OpenApi.Models;

#region Build
var builder = WebApplication.CreateBuilder(args);

#region JwtBeare
var key = builder.Configuration.GetSection("Jwt:Key").Value;
var issuer = builder.Configuration.GetSection("Jwt:Issuer").Value;
var audience = builder.Configuration.GetSection("Jwt:Audience").Value;

var keyBytes = Encoding.UTF8.GetBytes(key);

builder.Services.AddAuthentication(options =>
{

    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(opt =>
{

    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)

    };
});
#endregion

builder.Services.AddSingleton<GerarToken>();
builder.Services.AddScoped<IAdministradorServices, AdministradorServices>();
builder.Services.AddScoped<IVeiculosServices, VeiculosServices>();
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

#region SwaggerConfigure
builder.Services.AddSwaggerGen(opt => 
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Minha API",
        Version = "v1",
        Description = "Uma Minimal-Api de veiculos e outros serviços"
    });

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Insira o token JWT desta maneira: `Bearer {seu token}`"
    });

    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
#endregion

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home()));
#endregion

#region Administrador
app.MapPost("/v1/administrador/login", ([FromBody] LoginViewModel loginModel, GerarToken tokenService, IAdministradorServices services) =>
{
    var administrador = services.Login(loginModel);
    if (administrador != null)
    {
        var userToken = tokenService.GenerateToken(administrador.Email, administrador.Perfil);
        return Results.Ok(new
        {
            token = userToken
        });
    }
    else
    {
        return Results.Unauthorized();
    }
}).AllowAnonymous().WithTags("Administrador").WithSummary("Entrar como Administrador");

app.MapPost("/v1/administrador/Criar", ([FromBody] CriarAdministradorViewModel model,
[FromServices] IAdministradorServices service) =>
{
    if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Senha) || string.IsNullOrEmpty(model.Perfil))
        return Results.BadRequest("Todos os campos são Obrigatórios");


    var data = service.Criar(model);

    return Results.Ok("Administrador cadastrado com sucesso.");

}).RequireAuthorization().WithTags("Administrador").WithSummary("Criar um novo Administrador");

app.MapGet("/v1/administradores", (IAdministradorServices services) => services.Todos())
.RequireAuthorization()
.WithTags("Administrador")
.WithSummary("Buscar todos os Administradores");

app.MapPut("/Administrador/{id}", (int id, CriarAdministradorViewModel model, [FromServices] IAdministradorServices services) =>
{
    if (id == 0) return Results.BadRequest("ID Inválido");

    var data = services.Atualizar(id, model);

    return Results.Ok(data);

}).RequireAuthorization().WithTags("Administrador").WithSummary("Atualizar um Administrador");

app.MapGet("/v1/administrador/{id}", (int id, IAdministradorServices services) =>
{
    if (id <= 0)
        return Results.BadRequest("ID Inválido");

    var model = services.BuscarPorId(id);

    return Results.Ok(model);
}).RequireAuthorization().WithTags("Administrador").WithSummary("Buscar um Administrador por ID");

app.MapDelete("/v1/administrador/remover", (int id, IAdministradorServices services) =>
{
    if (id <= 0)
        return Results.BadRequest("ID Inválido");

    var data = services.Remover(id);

    return Results.Ok(data);
}).RequireAuthorization().WithTags("Administrador").WithSummary("Remover um Administrador");
#endregion



#region Veiculos

app.MapPost("/v1/veiculos/criar", ([FromBody] VeiculoViewModel model, [FromServices] IVeiculosServices services) =>
{

    if (string.IsNullOrEmpty(model.Nome) || string.IsNullOrEmpty(model.Marca) || model.Ano <= 0)
        return Results.BadRequest("Todos os campos são Obrigatório");

    var veiculo = new Veiculos
    {

        Nome = model.Nome,
        Marca = model.Marca,
        Ano = model.Ano
    };

    var data = services.CriarVeiculo(veiculo);

    return Results.Created($"/v1/veiculos/{veiculo.id}", veiculo);


}).RequireAuthorization().WithTags("Veiculos").WithSummary("Criar um novo Veiculo");

app.MapGet("/v1/veiculos", ([FromQuery] int pagina, [FromServices] IVeiculosServices services) =>
{
    var data = services.TodosOsVeiculos(pagina);

    return Results.Ok(data);
}).RequireAuthorization().WithTags("Veiculos").WithSummary("Buscar todos os Veiculos");

app.MapGet("/v1/veiculos/{id}", ([FromQuery] int id, [FromServices] IVeiculosServices services) =>
{
    if (id <= 0) return Results.BadRequest("ID Inválido");

    var data = services.BuscarVeiculoPorId(id);

    if (data is null) return Results.NotFound("Veiculo não encontrado.");

    return Results.Ok(data);

}).RequireAuthorization().WithTags("Veiculos").WithSummary("Buscar um veiculo por ID");

app.MapPut("/v1/veiculos/atualizar", (int id, VeiculoViewModel model, IVeiculosServices services) =>
{
    if (model is null)
        return Results.BadRequest("Ocorreu um poblema na Atalização do Veiculo");

    var data = services.AtualizarVeiculo(id, model);

    return Results.Ok(data);
}).RequireAuthorization().WithTags("Veiculos").WithSummary("Atulizar um Veiculo");

app.MapDelete("/v1/veiculos/remover", ([FromQuery] int id, [FromServices] IVeiculosServices services) =>
{
    if (id <= 0)
        return Results.BadRequest("ID Inválido");

    var data = services.RemoverVeiculo(id);
    return Results.Ok(data);
}).RequireAuthorization().WithTags("Veiculos").WithSummary("Remover um veiculo");


#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.Run();
#endregion 



