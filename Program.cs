using CamposDealerApp.Context;
using CamposDealerApp.Service;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Using builder to create a connection to SQL Server
var connectionString = builder.Configuration.GetConnectionString("CampDContext");

builder.Services.AddDbContext<CampDContext>(options =>
            options.UseSqlServer(connectionString));

builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions
    .ReferenceHandler = ReferenceHandler.IgnoreCycles);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options
.ResolveConflictingActions(apiDescription => apiDescription.First()));

builder.Services.AddScoped<VendaService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<ProdutoService>();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(option =>
{
    option.AddPolicy(name: MyAllowSpecificOrigins,
    policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
