using Nebula.Data;
using Nebula.Data.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<PersistenceSettings>(builder.Configuration.GetSection("RavenDb"));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddSingleton<IRavenDbContext, RavenDbContext>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ICpeService, CpeService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
