using Microsoft.EntityFrameworkCore;
using PropostaService.Application.Interfaces;
using PropostaService.Application.Interfaces.Repositories;
using PropostaService.Application.UseCases;
using PropostaService.Domain.Common;
using PropostaService.Infrastructure.Persistence;
using PropostaService.Infrastructure.Repositories;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IClock, SystemClock>();
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});


builder.Services.AddScoped<IProposalRepository, ProposalRepository>();
builder.Services.AddScoped<ICreateProposalUseCase, CreateProposalUseCase>();
builder.Services.AddScoped<IApproveProposalUseCase, ApproveProposalUseCase>();
builder.Services.AddScoped<IRejectProposalUseCase, RejectProposalUseCase>();
builder.Services.AddScoped<IGetProposalByIdUseCase, GetProposalByIdUseCase>();
builder.Services.AddScoped<IGetAllUseCase, GetAllUseCase>();
builder.Services.AddScoped<IDeleteProposalUseCase, DeleteProposalUseCase>();
builder.Services.AddScoped<IRestoreProposalUseCase, RestoreProposalUseCase>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ProposalDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
    .UseSnakeCaseNamingConvention()
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ProposalDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
