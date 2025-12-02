using Microsoft.EntityFrameworkCore;
using UserManager.Infrastructure.Data;
using UserManager.Application.Features.Users.CreateUser;
using FluentValidation;
using MediatR;
using UserManager.Application.Behaviors;

var builder = WebApplication.CreateBuilder(args);

// TODO: Before it grows too much, divide the program.cs code into extension methods in separate files.

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddDbContext<UserManagerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserManagerDbConnection")));

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateUserRequestHandler).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(CreateUserRequestValidator).Assembly);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
