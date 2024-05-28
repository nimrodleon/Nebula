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
using Nebula.Modules.Inventory;
using Nebula.Modules.InvoiceHub;
using Nebula.Modules.Products;
using Nebula.Modules.Sales;
using Nebula.Modules.Taller;

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
builder.Services.AddModuleAuthServices();
builder.Services.AddModuleAccountServices();
builder.Services.AddModuleCashierServices();
builder.Services.AddModuleContactsServices();
builder.Services.AddModuleFinanzasServices();
builder.Services.AddModuleInventoryServices();
builder.Services.AddModuleProductsServices();
builder.Services.AddModuleSalesServices();
builder.Services.AddModuleTallerServices();
builder.Services.Configure<InvoiceHubSettings>(builder.Configuration.GetSection(nameof(InvoiceHubSettings)));
builder.Services.AddModuleInvoiceHubServices();

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
