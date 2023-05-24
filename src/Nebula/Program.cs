using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nebula.Common;
using Nebula.Modules.Auth;
using Nebula.Modules.Cashier;
using Nebula.Modules.Configurations;
using Nebula.Modules.Configurations.Dto;
using Nebula.Modules.Configurations.Subscriptions;
using Nebula.Modules.Configurations.Warehouses;
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
builder.Services.AddScoped(typeof(ICrudOperationService<>), typeof(CrudOperationService<>));

#region Configuration

builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IInvoiceSerieService, InvoiceSerieService>();

#endregion

#region Productos

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductLoteService, ProductLoteService>();

#endregion

builder.Services.AddScoped<IReceivableService, ReceivableService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICajaDiariaService, CajaDiariaService>();
builder.Services.AddScoped<ICashierDetailService, CashierDetailService>();
builder.Services.AddScoped<IInvoiceSaleDetailService, InvoiceSaleDetailService>();
builder.Services.AddScoped<IInvoiceSaleService, InvoiceSaleService>();
builder.Services.AddScoped<ITributoSaleService, TributoSaleService>();
builder.Services.AddScoped<IDetallePagoSaleService, DetallePagoSaleService>();
builder.Services.AddScoped<ICashierSaleService, CashierSaleService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ILocationDetailService, LocationDetailService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<IMaterialDetailService, MaterialDetailService>();
builder.Services.AddScoped<IInventoryNotasService, InventoryNotasService>();
builder.Services.AddScoped<IInventoryNotasDetailService, InventoryNotasDetailService>();
builder.Services.AddScoped<ITransferenciaService, TransferenciaService>();
builder.Services.AddScoped<ITransferenciaDetailService, TransferenciaDetailService>();
builder.Services.AddScoped<IAjusteInventarioService, AjusteInventarioService>();
builder.Services.AddScoped<IAjusteInventarioDetailService, AjusteInventarioDetailService>();
builder.Services.AddScoped<IFacturadorService, FacturadorService>();
builder.Services.AddScoped<IComprobanteService, ComprobanteService>();
builder.Services.AddScoped<ICreditNoteService, CreditNoteService>();
builder.Services.AddScoped<ICreditNoteDetailService, CreditNoteDetailService>();
builder.Services.AddScoped<ITributoCreditNoteService, TributoCreditNoteService>();
builder.Services.AddScoped<IConsultarValidezComprobanteService, ConsultarValidezComprobanteService>();

#region ModuleFacturadorSUNAT

builder.Services.AddScoped<IHttpRequestFacturadorService, HttpRequestFacturadorService>();

#endregion

#region ModuleInventory

builder.Services.AddScoped<IProductStockService, ProductStockService>();
builder.Services.AddScoped<IValidateStockService, ValidateStockService>();
builder.Services.AddScoped<IHelperCalculateProductStockService, HelperCalculateProductStockService>();

#endregion

#region ModuleTaller

builder.Services.AddScoped<ITallerRepairOrderService, TallerRepairOrderService>();
builder.Services.AddScoped<ITallerItemRepairOrderService, TallerItemRepairOrderService>();

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
