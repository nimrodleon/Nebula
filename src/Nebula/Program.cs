using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nebula.Common;
using Nebula.Database;
using Nebula.Database.Dto.Common;
using Nebula.Database.Services.Common;
using Nebula.Modules.Auth;
using Nebula.Modules.Cashier;
using Nebula.Modules.Contacts;
using Nebula.Modules.Facturador;
using Nebula.Modules.Finanzas;
using Nebula.Modules.Inventory.Ajustes;
using Nebula.Modules.Inventory.Locations;
using Nebula.Modules.Inventory.Materiales;
using Nebula.Modules.Inventory.Notas;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Transferencias;
using Nebula.Modules.Products;
using Nebula.Modules.Sales;
using Nebula.Modules.Taller.Services;

var builder = WebApplication.CreateBuilder(args);
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

#region Common

builder.Services.AddScoped(typeof(CrudOperationService<>));
builder.Services.AddScoped<WarehouseService>();
builder.Services.AddScoped<InvoiceSerieService>();

#endregion

#region Productos

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ProductLoteService>();

#endregion

builder.Services.AddScoped<ReceivableService>();
builder.Services.AddScoped<ConfigurationService>();
builder.Services.AddScoped<ContactService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CajaDiariaService>();
builder.Services.AddScoped<CashierDetailService>();
builder.Services.AddScoped<InvoiceSaleDetailService>();
builder.Services.AddScoped<InvoiceSaleService>();
builder.Services.AddScoped<TributoSaleService>();
builder.Services.AddScoped<DetallePagoSaleService>();
builder.Services.AddScoped<CashierSaleService>();
builder.Services.AddScoped<LocationService>();
builder.Services.AddScoped<LocationDetailService>();
builder.Services.AddScoped<MaterialService>();
builder.Services.AddScoped<MaterialDetailService>();
builder.Services.AddScoped<InventoryNotasService>();
builder.Services.AddScoped<InventoryNotasDetailService>();
builder.Services.AddScoped<TransferenciaService>();
builder.Services.AddScoped<TransferenciaDetailService>();
builder.Services.AddScoped<AjusteInventarioService>();
builder.Services.AddScoped<AjusteInventarioDetailService>();
builder.Services.AddScoped<FacturadorService>();
builder.Services.AddScoped<ComprobanteService>();
builder.Services.AddScoped<CreditNoteService>();
builder.Services.AddScoped<CreditNoteDetailService>();
builder.Services.AddScoped<TributoCreditNoteService>();
builder.Services.AddScoped<ConsultarValidezComprobanteService>();

#region PluginFacturadorSUNAT

builder.Services.AddScoped<HttpRequestFacturadorService>();

#endregion

#region PluginInventory

builder.Services.AddScoped<ProductStockService>();
builder.Services.AddScoped<ValidateStockService>();
builder.Services.AddScoped<IHelperCalculateProductStockService, HelperCalculateProductStockService>();

#endregion

#region PluginTaller

builder.Services.AddScoped<TallerRepairOrderService>();
builder.Services.AddScoped<TallerItemRepairOrderService>();

#endregion

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

// Generar un hash único.
MasterKeyDto.WriteHashFile();

var app = builder.Build();
var applicationPort = builder.Configuration.GetValue<string>("ApplicationPort");
var storagePath = builder.Configuration.GetValue<string>("StoragePath");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    applicationPort = "5042";
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
    FileProvider = new PhysicalFileProvider(Path.Combine(storagePath, "uploads")),
    RequestPath = "/uploads"
});

string applicationUrl = $"http://localhost:{applicationPort}";

app.UseRouting();
app.UseCors("MyPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run(applicationUrl);
