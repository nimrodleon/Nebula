using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Nebula.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configuration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ruc = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    RznSocial = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodLocalEmisor = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TipMoneda = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    PorcentajeIgv = table.Column<decimal>(type: "numeric", nullable: true),
                    ValorImpuestoBolsa = table.Column<decimal>(type: "numeric", nullable: true),
                    CpeSunat = table.Column<string>(type: "text", nullable: true),
                    CuentaBancoDetraccion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TextoDetraccion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    MontoDetraccion = table.Column<decimal>(type: "numeric", nullable: true),
                    ContactId = table.Column<int>(type: "integer", nullable: true),
                    UrlApi = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    FileSunat = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    FileControl = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Document = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    DocType = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Address = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    PhoneNumber = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Email = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryReasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipOperacion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    FecEmision = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    HorEmision = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodLocalEmisor = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TipDocUsuario = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    NumDocUsuario = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    RznSocialUsuario = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TipMoneda = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodMotivo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    DesMotivo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TipDocAfectado = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    NumDocAfectado = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SumTotTributos = table.Column<decimal>(type: "numeric", nullable: true),
                    SumTotValVenta = table.Column<decimal>(type: "numeric", nullable: true),
                    SumPrecioVenta = table.Column<decimal>(type: "numeric", nullable: true),
                    SumDescTotal = table.Column<decimal>(type: "numeric", nullable: true),
                    SumOtrosCargos = table.Column<decimal>(type: "numeric", nullable: true),
                    SumTotalAnticipos = table.Column<decimal>(type: "numeric", nullable: true),
                    SumImpVenta = table.Column<decimal>(type: "numeric", nullable: true),
                    Year = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Month = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DocType = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    InvoiceType = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Serie = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Number = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TipOperacion = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    FecEmision = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    HorEmision = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    FecVencimiento = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodLocalEmisor = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    FormaPago = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TipDocUsuario = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    NumDocUsuario = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    RznSocialUsuario = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    TipMoneda = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SumTotTributos = table.Column<decimal>(type: "numeric", nullable: true),
                    SumTotValVenta = table.Column<decimal>(type: "numeric", nullable: true),
                    SumPrecioVenta = table.Column<decimal>(type: "numeric", nullable: true),
                    SumImpVenta = table.Column<decimal>(type: "numeric", nullable: true),
                    Year = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Month = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeOperationSunat",
                columns: table => new
                {
                    Id = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeOperationSunat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UndMedida",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SunatCode = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UndMedida", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Remark = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceNoteDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoiceNoteId = table.Column<int>(type: "integer", nullable: true),
                    CodUnidadMedida = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CtdUnidadItem = table.Column<decimal>(type: "numeric", nullable: true),
                    CodProducto = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodProductoSunat = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    DesItem = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MtoValorUnitario = table.Column<decimal>(type: "numeric", nullable: true),
                    SumTotTributosItem = table.Column<decimal>(type: "numeric", nullable: true),
                    CodTriIgv = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    MtoIgvItem = table.Column<decimal>(type: "numeric", nullable: true),
                    MtoBaseIgvItem = table.Column<decimal>(type: "numeric", nullable: true),
                    NomTributoIgvItem = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodTipTributoIgvItem = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TipAfeIgv = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    PorIgvItem = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodTriIcbper = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    MtoTriIcbperItem = table.Column<decimal>(type: "numeric", nullable: true),
                    CtdBolsasTriIcbperItem = table.Column<int>(type: "integer", nullable: true),
                    NomTributoIcbperItem = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodTipTributoIcbperItem = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    MtoTriIcbperUnidad = table.Column<decimal>(type: "numeric", nullable: true),
                    MtoPrecioVentaUnitario = table.Column<decimal>(type: "numeric", nullable: true),
                    MtoValorVentaItem = table.Column<decimal>(type: "numeric", nullable: true),
                    MtoValorReferencialUnitario = table.Column<decimal>(type: "numeric", nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceNoteDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceNoteDetails_InvoiceNotes_InvoiceNoteId",
                        column: x => x.InvoiceNoteId,
                        principalTable: "InvoiceNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoiceId = table.Column<int>(type: "integer", nullable: true),
                    Serie = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Number = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    AccountType = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Status = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Cuota = table.Column<int>(type: "integer", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: true),
                    Balance = table.Column<decimal>(type: "numeric", nullable: true),
                    EndDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Year = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Month = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceAccounts_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoiceId = table.Column<int>(type: "integer", nullable: true),
                    CodUnidadMedida = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CtdUnidadItem = table.Column<decimal>(type: "numeric", nullable: true),
                    CodProducto = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodProductoSunat = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    DesItem = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    MtoValorUnitario = table.Column<decimal>(type: "numeric", nullable: true),
                    SumTotTributosItem = table.Column<decimal>(type: "numeric", nullable: true),
                    CodTriIgv = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    MtoIgvItem = table.Column<decimal>(type: "numeric", nullable: true),
                    MtoBaseIgvItem = table.Column<decimal>(type: "numeric", nullable: true),
                    NomTributoIgvItem = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodTipTributoIgvItem = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TipAfeIgv = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    PorIgvItem = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodTriIcbper = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    MtoTriIcbperItem = table.Column<decimal>(type: "numeric", nullable: true),
                    CtdBolsasTriIcbperItem = table.Column<int>(type: "integer", nullable: true),
                    NomTributoIcbperItem = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodTipTributoIcbperItem = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    MtoTriIcbperUnidad = table.Column<decimal>(type: "numeric", nullable: true),
                    MtoPrecioVentaUnitario = table.Column<decimal>(type: "numeric", nullable: true),
                    MtoValorVentaItem = table.Column<decimal>(type: "numeric", nullable: true),
                    MtoValorReferencialUnitario = table.Column<decimal>(type: "numeric", nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceDetails_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tributos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoiceId = table.Column<int>(type: "integer", nullable: true),
                    IdeTributo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    NomTributo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CodTipTributo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    MtoBaseImponible = table.Column<decimal>(type: "numeric", nullable: true),
                    MtoTributo = table.Column<decimal>(type: "numeric", nullable: true),
                    Year = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Month = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tributos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tributos_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Barcode = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Price1 = table.Column<decimal>(type: "numeric", nullable: true),
                    Price2 = table.Column<decimal>(type: "numeric", nullable: true),
                    FromQty = table.Column<decimal>(type: "numeric", nullable: true),
                    IgvSunat = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Icbper = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    UndMedidaId = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    PathImage = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_UndMedida_UndMedidaId",
                        column: x => x.UndMedidaId,
                        principalTable: "UndMedida",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContactId = table.Column<int>(type: "integer", nullable: true),
                    WarehouseId = table.Column<int>(type: "integer", nullable: true),
                    NoteType = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Motivo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Remark = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Status = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Year = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Month = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryNotes_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventoryNotes_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceSeries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    WarehouseId = table.Column<int>(type: "integer", nullable: true),
                    Factura = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CounterFactura = table.Column<int>(type: "integer", nullable: true),
                    Boleta = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CounterBoleta = table.Column<int>(type: "integer", nullable: true),
                    NotaDeVenta = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    CounterNotaDeVenta = table.Column<int>(type: "integer", nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceSeries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvoiceSeries_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OriginId = table.Column<int>(type: "integer", nullable: true),
                    TargetId = table.Column<int>(type: "integer", nullable: true),
                    Motivo = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Remark = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Status = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Year = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Month = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferNotes_Warehouses_OriginId",
                        column: x => x.OriginId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferNotes_Warehouses_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryNoteDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InventoryNoteId = table.Column<int>(type: "integer", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: true),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryNoteDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryNoteDetails_InventoryNotes_InventoryNoteId",
                        column: x => x.InventoryNoteId,
                        principalTable: "InventoryNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CajasDiaria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InvoiceSerieId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    TotalApertura = table.Column<decimal>(type: "numeric", nullable: true),
                    TotalContabilizado = table.Column<decimal>(type: "numeric", nullable: true),
                    TotalCierre = table.Column<decimal>(type: "numeric", nullable: true),
                    Year = table.Column<string>(type: "text", nullable: true),
                    Month = table.Column<string>(type: "text", nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CajasDiaria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CajasDiaria_InvoiceSeries_InvoiceSerieId",
                        column: x => x.InvoiceSerieId,
                        principalTable: "InvoiceSeries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransferNoteDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TransferNoteId = table.Column<int>(type: "integer", nullable: true),
                    ProductId = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Price = table.Column<decimal>(type: "numeric", nullable: true),
                    Quantity = table.Column<decimal>(type: "numeric", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferNoteDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferNoteDetails_TransferNotes_TransferNoteId",
                        column: x => x.TransferNoteId,
                        principalTable: "TransferNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CashierDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CajaDiariaId = table.Column<int>(type: "integer", nullable: true),
                    InvoiceId = table.Column<int>(type: "integer", nullable: true),
                    TypeOperation = table.Column<int>(type: "integer", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Document = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Contact = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Glosa = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    PaymentMethod = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    Total = table.Column<decimal>(type: "numeric", nullable: true),
                    SoftDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashierDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CashierDetails_CajasDiaria_CajaDiariaId",
                        column: x => x.CajaDiariaId,
                        principalTable: "CajasDiaria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CashierDetails_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CajasDiaria_InvoiceSerieId",
                table: "CajasDiaria",
                column: "InvoiceSerieId");

            migrationBuilder.CreateIndex(
                name: "IX_CashierDetails_CajaDiariaId",
                table: "CashierDetails",
                column: "CajaDiariaId");

            migrationBuilder.CreateIndex(
                name: "IX_CashierDetails_InvoiceId",
                table: "CashierDetails",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryNoteDetails_InventoryNoteId",
                table: "InventoryNoteDetails",
                column: "InventoryNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryNotes_ContactId",
                table: "InventoryNotes",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryNotes_WarehouseId",
                table: "InventoryNotes",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceAccounts_InvoiceId",
                table: "InvoiceAccounts",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceDetails_InvoiceId",
                table: "InvoiceDetails",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceNoteDetails_InvoiceNoteId",
                table: "InvoiceNoteDetails",
                column: "InvoiceNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_InvoiceSeries_WarehouseId",
                table: "InvoiceSeries",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_UndMedidaId",
                table: "Products",
                column: "UndMedidaId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferNoteDetails_TransferNoteId",
                table: "TransferNoteDetails",
                column: "TransferNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferNotes_OriginId",
                table: "TransferNotes",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferNotes_TargetId",
                table: "TransferNotes",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_Tributos_InvoiceId",
                table: "Tributos",
                column: "InvoiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CashierDetails");

            migrationBuilder.DropTable(
                name: "Configuration");

            migrationBuilder.DropTable(
                name: "InventoryNoteDetails");

            migrationBuilder.DropTable(
                name: "InventoryReasons");

            migrationBuilder.DropTable(
                name: "InvoiceAccounts");

            migrationBuilder.DropTable(
                name: "InvoiceDetails");

            migrationBuilder.DropTable(
                name: "InvoiceNoteDetails");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "TransferNoteDetails");

            migrationBuilder.DropTable(
                name: "Tributos");

            migrationBuilder.DropTable(
                name: "TypeOperationSunat");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "CajasDiaria");

            migrationBuilder.DropTable(
                name: "InventoryNotes");

            migrationBuilder.DropTable(
                name: "InvoiceNotes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "UndMedida");

            migrationBuilder.DropTable(
                name: "TransferNotes");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropTable(
                name: "InvoiceSeries");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Warehouses");
        }
    }
}
