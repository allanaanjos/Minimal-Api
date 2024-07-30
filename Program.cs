using MinimalApi.ViewModels;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/v1/login", (LoginViewModel loginModel) => {
    if(loginModel.Email == "adm@teste.com" && loginModel.Passworld == "12345"){
        return Results.Ok("Login com Sucesso!!");
    }
    else{
        return Results.Unauthorized();
    }
});

app.Run();
