using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Labb2_Databaser.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFörfattareBöckerRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Butiker",
                columns: table => new
                {
                    ButikID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Butiksnamn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PostNummer = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Ort = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Butiker__B5D66BFA94885CF2", x => x.ButikID);
                });

            migrationBuilder.CreateTable(
                name: "Författare",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Förnamn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Efternamn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Födelsedatum = table.Column<DateOnly>(type: "date", nullable: false),
                    Dödsdatum = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Författa__3214EC27ED9A1422", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Förlag",
                columns: table => new
                {
                    FörlagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Namn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefon_Nummer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PostNummer = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Ort = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Förlag__DE6A852C9B0007C1", x => x.FörlagID);
                });

            migrationBuilder.CreateTable(
                name: "Kunder",
                columns: table => new
                {
                    KundID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Förnamn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Efternamn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Epost = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefon_Nummer = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PostNummer = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false),
                    Ort = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Kunder__F2B5DEAC676B8B49", x => x.KundID);
                });

            migrationBuilder.CreateTable(
                name: "Böcker",
                columns: table => new
                {
                    ISBN13 = table.Column<string>(type: "char(13)", unicode: false, fixedLength: true, maxLength: 13, nullable: false),
                    Titel = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Språk = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Pris = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Utgivningsdatum = table.Column<DateOnly>(type: "date", nullable: false),
                    Antal_sidor = table.Column<int>(type: "int", nullable: true),
                    FörlagID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Böcker__3BF79E034CB8E506", x => x.ISBN13);
                    table.ForeignKey(
                        name: "FK__Böcker__FörlagID__4AB81AF0",
                        column: x => x.FörlagID,
                        principalTable: "Förlag",
                        principalColumn: "FörlagID");
                });

            migrationBuilder.CreateTable(
                name: "Ordrar",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KundID = table.Column<int>(type: "int", nullable: true),
                    OrderDatum = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ordrar__C3905BAFF02E51B6", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK__Ordrar__KundID__440B1D61",
                        column: x => x.KundID,
                        principalTable: "Kunder",
                        principalColumn: "KundID");
                });

            migrationBuilder.CreateTable(
                name: "BöckerFörfattare",
                columns: table => new
                {
                    FörfattareId = table.Column<int>(type: "int", nullable: false),
                    Isbn13 = table.Column<string>(type: "char(13)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BöckerFörfattare", x => new { x.FörfattareId, x.Isbn13 });
                    table.ForeignKey(
                        name: "FK_BöckerFörfattare_Böcker_Isbn13",
                        column: x => x.Isbn13,
                        principalTable: "Böcker",
                        principalColumn: "ISBN13",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BöckerFörfattare_Författare_FörfattareId",
                        column: x => x.FörfattareId,
                        principalTable: "Författare",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FörfattareBöcker",
                columns: table => new
                {
                    FörfattareId = table.Column<int>(type: "int", nullable: false),
                    Isbn = table.Column<string>(type: "char(13)", unicode: false, fixedLength: true, maxLength: 13, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FörfattareBöcker", x => new { x.FörfattareId, x.Isbn });
                    table.ForeignKey(
                        name: "FK_FörfattareBöcker_Böcker_Isbn",
                        column: x => x.Isbn,
                        principalTable: "Böcker",
                        principalColumn: "ISBN13",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FörfattareBöcker_Författare_FörfattareId",
                        column: x => x.FörfattareId,
                        principalTable: "Författare",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LagerSaldo",
                columns: table => new
                {
                    ButikID = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "char(13)", unicode: false, fixedLength: true, maxLength: 13, nullable: false),
                    Antal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LagerSal__1191B894FD876E18", x => new { x.ButikID, x.ISBN });
                    table.ForeignKey(
                        name: "FK__LagerSald__Butik__3E52440B",
                        column: x => x.ButikID,
                        principalTable: "Butiker",
                        principalColumn: "ButikID");
                    table.ForeignKey(
                        name: "FK__LagerSaldo__ISBN__3F466844",
                        column: x => x.ISBN,
                        principalTable: "Böcker",
                        principalColumn: "ISBN13");
                });

            migrationBuilder.CreateTable(
                name: "OrderDetaljer",
                columns: table => new
                {
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "char(13)", unicode: false, fixedLength: true, maxLength: 13, nullable: false),
                    Antal = table.Column<int>(type: "int", nullable: false),
                    Pris = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderDet__67D788C1FDBA446D", x => new { x.OrderID, x.ISBN });
                    table.ForeignKey(
                        name: "FK__OrderDeta__Order__46E78A0C",
                        column: x => x.OrderID,
                        principalTable: "Ordrar",
                        principalColumn: "OrderID");
                    table.ForeignKey(
                        name: "FK__OrderDetal__ISBN__47DBAE45",
                        column: x => x.ISBN,
                        principalTable: "Böcker",
                        principalColumn: "ISBN13");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Böcker_FörlagID",
                table: "Böcker",
                column: "FörlagID");

            migrationBuilder.CreateIndex(
                name: "IX_BöckerFörfattare_Isbn13",
                table: "BöckerFörfattare",
                column: "Isbn13");

            migrationBuilder.CreateIndex(
                name: "IX_FörfattareBöcker_Isbn",
                table: "FörfattareBöcker",
                column: "Isbn");

            migrationBuilder.CreateIndex(
                name: "IX_LagerSaldo_ISBN",
                table: "LagerSaldo",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetaljer_ISBN",
                table: "OrderDetaljer",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_Ordrar_KundID",
                table: "Ordrar",
                column: "KundID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BöckerFörfattare");

            migrationBuilder.DropTable(
                name: "FörfattareBöcker");

            migrationBuilder.DropTable(
                name: "LagerSaldo");

            migrationBuilder.DropTable(
                name: "OrderDetaljer");

            migrationBuilder.DropTable(
                name: "Författare");

            migrationBuilder.DropTable(
                name: "Butiker");

            migrationBuilder.DropTable(
                name: "Ordrar");

            migrationBuilder.DropTable(
                name: "Böcker");

            migrationBuilder.DropTable(
                name: "Kunder");

            migrationBuilder.DropTable(
                name: "Förlag");
        }
    }
}
