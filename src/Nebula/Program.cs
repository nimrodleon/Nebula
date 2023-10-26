using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Nebula.Common;
using Nebula.Common.Helpers;
using Nebula.Modules.Account;
using Nebula.Modules.Auth;
using Nebula.Modules.Cashier;
using Nebula.Modules.Contacts;
using Nebula.Modules.Finanzas;
using Nebula.Modules.Inventory.Ajustes;
using Nebula.Modules.Inventory.Locations;
using Nebula.Modules.Inventory.Materiales;
using Nebula.Modules.Inventory.Notas;
using Nebula.Modules.Inventory.Stock;
using Nebula.Modules.Inventory.Transferencias;
using Nebula.Modules.InvoiceHub;
using Nebula.Modules.Products;
using Nebula.Modules.Purchases;
using Nebula.Modules.Sales;
using Nebula.Modules.Sales.Comprobantes;
using Nebula.Modules.Sales.Invoices;
using Nebula.Modules.Sales.Notes;
using Nebula.Modules.Taller.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
string keygen = "5dc7a849f2668157c3a7c4f037dcafa62c6ba82e5d9b31b56edbcc6731f8c9c8";
var secretKey = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("SecretKey") ?? keygen);

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

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddScoped<MongoDatabaseService>();
builder.Services.AddScoped(typeof(ICrudOperationService<>), typeof(CrudOperationService<>));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(builder.Configuration.GetValue<string>("Redis") ?? string.Empty));
builder.Services.AddSingleton(provider => provider.GetRequiredService<IConnectionMultiplexer>().GetDatabase());
builder.Services.AddScoped<ICacheAuthService, CacheAuthService>();

#region ModuleAuth

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICollaboratorService, CollaboratorService>();
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
builder.Services.AddScoped<IJwtService, JwtService>();

#endregion

#region ModuleAccount

builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();
builder.Services.AddScoped<IInvoiceSerieService, InvoiceSerieService>();

#endregion

#region ModuleCashier

builder.Services.AddScoped<ICajaDiariaService, CajaDiariaService>();
builder.Services.AddScoped<ICashierDetailService, CashierDetailService>();
builder.Services.AddScoped<ICashierSaleService, CashierSaleService>();

#endregion

#region ModuleContacts

builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IContribuyenteService, ContribuyenteService>();

#endregion

#region ModuleFinanzas

builder.Services.AddScoped<IAccountsReceivableService, AccountsReceivableService>();

#endregion

#region ModuleInventory

builder.Services.AddScoped<IProductStockService, ProductStockService>();
builder.Services.AddScoped<IHelperCalculateProductStockService, HelperCalculateProductStockService>();
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
builder.Services.AddScoped<IValidateStockService, ValidateStockService>();

#endregion

#region ModuleProductos

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

#endregion

#region ModulePurchases

builder.Services.AddScoped<IPurchaseInvoiceService, PurchaseInvoiceService>();
builder.Services.AddScoped<IPurchaseInvoiceDetailService, PurchaseInvoiceDetailService>();
builder.Services.AddScoped<IConsultarValidezCompraService, ConsultarValidezCompraService>();

#endregion

#region ModuleSales

builder.Services.AddScoped<IInvoiceSaleService, InvoiceSaleService>();
builder.Services.AddScoped<IInvoiceSaleDetailService, InvoiceSaleDetailService>();
builder.Services.AddScoped<IComprobanteService, ComprobanteService>();
builder.Services.AddScoped<ICreditNoteService, CreditNoteService>();
builder.Services.AddScoped<ICreditNoteDetailService, CreditNoteDetailService>();
builder.Services.AddScoped<IConsultarValidezComprobanteService, ConsultarValidezComprobanteService>();

#endregion

#region ModuleTaller

builder.Services.AddScoped<ITallerRepairOrderService, TallerRepairOrderService>();
builder.Services.AddScoped<ITallerItemRepairOrderService, TallerItemRepairOrderService>();

#endregion

#region ModuleInvoiceHub

builder.Services.Configure<InvoiceHubSettings>(builder.Configuration.GetSection(nameof(InvoiceHubSettings)));
builder.Services.AddHttpClient<ICreditNoteHubService, CreditNoteHubService>();
builder.Services.AddHttpClient<IInvoiceHubService, InvoiceHubService>();
builder.Services.AddHttpClient<ICertificadoUploaderService, CertificadoUploaderService>();
builder.Services.AddHttpClient<IEmpresaHubService, EmpresaHubService>();

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

var app = builder.Build();
var applicationPort = builder.Configuration.GetValue<string>("ApplicationPort") ?? "5042";
var storagePath = builder.Configuration.GetValue<string>("StoragePath") ?? string.Empty;

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
