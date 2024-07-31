using Microsoft.AspNetCore.Mvc;
using MinimalApi.Data.Db;
using MinimalApi.Domain.Entities;
using MinimalApi.Domain.Services;
using MinimalApi.Domain.ViewModels;
using MinimalApi.Interface;
using MinimalApi.ViewModels;

#region Build
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministradorServices, AdministradorServices>();
builder.Services.AddScoped<IVeiculosServices, VeiculosServices>();
builder.Services.AddDbContext<AppDbContext>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
#endregion

#region Home
app.MapGet("/", () => Results.Json(new Home()));
#endregion

#region Administrador
app.MapPost("/v1/administrador/login", ([FromBody] LoginViewModel loginModel, [FromServices] IAdministradorServices services) =>
{
    if (services.Login(loginModel) != null)
    {
        return Results.Ok("Login com Sucesso!!");
    }
    else
    {
        return Results.Unauthorized();
    }
});


app.MapPost("/v1/administrador/Criar", ([FromBody]CriarAdministradorViewModel model, [FromServices]IAdministradorServices service) =>
{
    if (model is null)
        return Results.BadRequest("Não foi possível criar o administrador");

   var data = service.Criar(model);

    return Results.Ok("Administrador cadastrado com sucesso.");
});


app.MapGet("/v1/administradores", (IAdministradorServices services) => services.Todos());


app.MapGet("/v1/administrador/buscar-por-id", (int id, IAdministradorServices services) => 
{
    if(id <= 0)
       return Results.BadRequest("ID Inválido");

    var model = services.BuscarPorId(id);

    return Results.Ok(model);
});


app.MapDelete("/v1/administrador/remover", (int id, IAdministradorServices services) => 
{
   if(id <= 0)
     return Results.BadRequest("ID Inválido");

    var data = services.Remover(id);

    return Results.Ok(data);
});
#endregion



#region Veiculos

app.MapPost("/v1/veiculos/criar", ([FromBody] VeiculoViewModel model, [FromServices] IVeiculosServices services) =>
{

    if (model is null)
        return Results.BadRequest("Não foi possivel criar seu veiculo");

    var veiculo = new Veiculos
    {

        Nome = model.Nome,
        Marca = model.Marca,
        Ano = model.Ano
    };

    var data = services.CriarVeiculo(veiculo);

    return Results.Created($"/v1/veiculos/{veiculo.id}", veiculo);


});

app.MapGet("/v1/veiculos", ([FromServices] IVeiculosServices services) => services.TodosOsVeiculos());

app.MapGet("/v1/veiculos/buscar-por-id", ([FromQuery] int id, [FromServices] IVeiculosServices services) =>
{
    if (id == 0)
        return Results.BadRequest("ID Inválido");

    var data = services.BuscarVeiculoPorId(id);

    if (data is null)
        return Results.NotFound("Veiculo não encontrado.");

    return Results.Ok(data);
});

app.MapPut("/v1/veiculos/atualizar", (int id, VeiculoViewModel model, IVeiculosServices services) =>
{
    if (model is null)
        return Results.BadRequest("Ocorreu um poblema na Atalização do Veiculo");

    var data = services.AtualizarVeiculo(id, model);

    return Results.Ok(data);
});

app.MapDelete("/v1/veiculos/remover", ([FromQuery] int id, [FromServices] IVeiculosServices services) =>
{
    if (id <= 0)
        return Results.BadRequest("ID Inválido");

    var data = services.RemoverVeiculo(id);
    return Results.Ok(data);
});


#endregion

#region App
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
#endregion