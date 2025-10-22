using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace app_web_net.Migrationss
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockProduits",
                columns: table => new
                {
                    ArticleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Categorie = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Tarif = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StockDisponible = table.Column<int>(type: "int", nullable: false),
                    DateEnregistrement = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockProduits", x => x.ArticleId);
                });

            migrationBuilder.CreateTable(
                name: "Ventes",
                columns: table => new
                {
                    CommandeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdresseExpedition = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    AcheteurId = table.Column<int>(type: "int", nullable: false),
                    Statut = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "En traitement"),
                    Montant = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DateEnregistrement = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ventes", x => x.CommandeId);
                });

            migrationBuilder.CreateTable(
                name: "DetailsCommande",
                columns: table => new
                {
                    ElementId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommandeId = table.Column<int>(type: "int", nullable: false),
                    LibelleProduit = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    CoutUnitaire = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ProduitId = table.Column<int>(type: "int", nullable: false),
                    NombreUnites = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailsCommande", x => x.ElementId);
                    table.ForeignKey(
                        name: "FK_DetailsCommande_StockProduits_ProduitId",
                        column: x => x.ProduitId,
                        principalTable: "StockProduits",
                        principalColumn: "ArticleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DetailsCommande_Ventes_CommandeId",
                        column: x => x.CommandeId,
                        principalTable: "Ventes",
                        principalColumn: "CommandeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetailsCommande_CommandeId",
                table: "DetailsCommande",
                column: "CommandeId");

            migrationBuilder.CreateIndex(
                name: "IX_DetailsCommande_ProduitId",
                table: "DetailsCommande",
                column: "ProduitId");

            migrationBuilder.CreateIndex(
                name: "IX_Ventes_DateEnregistrement",
                table: "Ventes",
                column: "DateEnregistrement");

            migrationBuilder.CreateIndex(
                name: "IX_Ventes_Statut",
                table: "Ventes",
                column: "Statut");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailsCommande");

            migrationBuilder.DropTable(
                name: "StockProduits");

            migrationBuilder.DropTable(
                name: "Ventes");
        }
    }
}
