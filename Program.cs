using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nebula.Database;
using Nebula.Database.Services;
using Nebula.Database.Services.Cashier;
using Nebula.Database.Services.Common;
using Nebula.Database.Services.Facturador;
using Nebula.Database.Services.Inventory;
using Nebula.Database.Services.Sales;

var builder = WebApplication.CreateBuilder(args);

// Configure Kestrel web server.
builder.WebHost.ConfigureKestrel((context, serverOptions) =>
{
    var kestrelSection = context.Configuration.GetSection("Kestrel");
    serverOptions.Configure(kestrelSection);
});

var secretKey = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("SecretKey"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(secretKey),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "MyPolicy",
        c => { c.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
});

// Add services to the container.
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddDbContext<FacturadorDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("BDFacturador")));
builder.Services.AddSingleton(typeof(CrudOperationService<>));
builder.Services.AddSingleton<ReceivableService>();
builder.Services.AddSingleton<ConfigurationService>();
builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<ContactService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<CajaDiariaService>();
builder.Services.AddSingleton<CashierDetailService>();
builder.Services.AddSingleton<InvoiceSaleDetailService>();
builder.Services.AddSingleton<InvoiceSaleService>();
builder.Services.AddSingleton<TributoSaleService>();
builder.Services.AddSingleton<DetallePagoSaleService>();
builder.Services.AddSingleton<CashierSaleService>();
builder.Services.AddSingleton<LocationService>();
builder.Services.AddSingleton<LocationDetailService>();
builder.Services.AddSingleton<MaterialService>();
builder.Services.AddSingleton<MaterialDetailService>();
builder.Services.AddSingleton<InventoryNotasService>();
builder.Services.AddSingleton<InventoryNotasDetailService>();
builder.Services.AddSingleton<ProductStockService>();
builder.Services.AddSingleton<ValidateStockService>();
builder.Services.AddSingleton<TransferenciaService>();
builder.Services.AddSingleton<TransferenciaDetailService>();
builder.Services.AddSingleton<AjusteInventarioService>();
builder.Services.AddSingleton<AjusteInventarioDetailService>();
builder.Services.AddSingleton<FacturadorService>();
builder.Services.AddSingleton<ComprobanteService>();
builder.Services.AddSingleton<CreditNoteService>();
builder.Services.AddSingleton<CreditNoteDetailService>();
builder.Services.AddSingleton<TributoCreditNoteService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Nebula", Version = "v1" });
    // To Enable authorization using Swagger (JWT).
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
            "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\""
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Nebula v1"));
}

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "StaticFiles")),
    RequestPath = "/StaticFiles"
});

app.UseRouting();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
