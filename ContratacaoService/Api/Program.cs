using ContratacaoService.Application.Interfaces;
using ContratacaoService.Application.Interfaces.Repositories;
using ContratacaoService.Application.UseCases;
using ContratacaoService.Domain.Common;
using ContratacaoService.Infrastructure.Persistence;
using ContratacaoService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using PropostaService.Infrastructure.Persistence;

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

builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IGetPolicyByIdUseCase, GetPolicyByIdUseCase>();
builder.Services.AddScoped<IContractPolicyUseCase, ContractPolicyUseCase>();
builder.Services.AddHttpClient<IProposalHttpClient, ProposalHttpClient>(client =>
{
    var baseUrl = builder.Configuration["ProposalService:BaseUrl"]
                  ?? "http://localhost:5000"; // HTTP, não HTTPS
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<PolicyDbContext>(options =>
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
    var dbContext = scope.ServiceProvider.GetRequiredService<PolicyDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
