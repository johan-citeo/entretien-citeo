using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EntretienCiteo.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    stock = table.Column<int>(type: "integer", nullable: false),
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    row_version = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_orders_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_lines",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_lines", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_lines__products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_lines_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "categories",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Electronics" },
                    { 2, "Clothing" },
                    { 3, "Books" },
                    { 4, "Smartphones" },
                    { 5, "Laptops" },
                    { 6, "Men's Clothing" },
                    { 7, "Women's Clothing" }
                });

            migrationBuilder.InsertData(
                table: "customers",
                columns: new[] { "id", "email", "name" },
                values: new object[,]
                {
                    { 1, "alice@example.com", "Alice Martin" },
                    { 2, "bob@example.com", "Bob Dupont" },
                    { 3, "charlie@example.com", "Charlie Lefebvre" },
                    { 4, "diana@example.com", "Diana Bernard" },
                    { 5, "etienne@example.com", "Etienne Moreau" },
                    { 6, "fanny@example.com", "Fanny Simon" },
                    { 7, "georges@example.com", "Georges Laurent" },
                    { 8, "helene@example.com", "Hélène Petit" },
                    { 9, "ivan@example.com", "Ivan Rousseau" },
                    { 10, "julie@example.com", "Julie Blanc" }
                });

            migrationBuilder.InsertData(
                table: "orders",
                columns: new[] { "id", "created_at", "customer_id", "status" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1 },
                    { 2, new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2 },
                    { 3, new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1, 0 },
                    { 4, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Utc), 2, 1 },
                    { 5, new DateTime(2024, 6, 5, 0, 0, 0, 0, DateTimeKind.Utc), 2, 3 },
                    { 6, new DateTime(2024, 1, 25, 0, 0, 0, 0, DateTimeKind.Utc), 3, 2 },
                    { 7, new DateTime(2024, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), 3, 1 },
                    { 8, new DateTime(2024, 9, 3, 0, 0, 0, 0, DateTimeKind.Utc), 3, 2 },
                    { 9, new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, 0 },
                    { 10, new DateTime(2024, 7, 18, 0, 0, 0, 0, DateTimeKind.Utc), 4, 1 },
                    { 11, new DateTime(2024, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), 5, 2 },
                    { 12, new DateTime(2024, 8, 15, 0, 0, 0, 0, DateTimeKind.Utc), 5, 1 },
                    { 13, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), 5, 0 },
                    { 14, new DateTime(2024, 5, 22, 0, 0, 0, 0, DateTimeKind.Utc), 6, 2 },
                    { 15, new DateTime(2024, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), 6, 1 },
                    { 16, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, 0 }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "category_id", "name", "price", "row_version", "stock" },
                values: new object[,]
                {
                    { 1, 4, "iPhone 16", 999.99m, new Guid("a1000000-0000-0000-0000-000000000001"), 50 },
                    { 2, 4, "Samsung Galaxy S25", 849.99m, new Guid("a1000000-0000-0000-0000-000000000002"), 40 },
                    { 3, 5, "MacBook Pro 14\"", 1999.99m, new Guid("a1000000-0000-0000-0000-000000000003"), 20 },
                    { 4, 5, "Dell XPS 15", 1599.99m, new Guid("a1000000-0000-0000-0000-000000000004"), 25 },
                    { 5, 6, "Men's T-Shirt", 29.99m, new Guid("a1000000-0000-0000-0000-000000000005"), 200 },
                    { 6, 7, "Women's Dress", 59.99m, new Guid("a1000000-0000-0000-0000-000000000006"), 150 },
                    { 7, 3, "Clean Code", 39.99m, new Guid("a1000000-0000-0000-0000-000000000007"), 100 },
                    { 8, 3, "The Pragmatic Programmer", 44.99m, new Guid("a1000000-0000-0000-0000-000000000008"), 80 }
                });

            migrationBuilder.InsertData(
                table: "order_lines",
                columns: new[] { "id", "order_id", "product_id", "quantity", "unit_price" },
                values: new object[,]
                {
                    { 1, 1, 1, 2, 999.99m },
                    { 2, 1, 7, 1, 39.99m },
                    { 3, 2, 3, 1, 1999.99m },
                    { 4, 3, 5, 3, 29.99m },
                    { 5, 3, 6, 1, 59.99m },
                    { 6, 4, 2, 1, 849.99m },
                    { 7, 4, 8, 1, 44.99m },
                    { 8, 5, 4, 1, 1599.99m },
                    { 9, 6, 7, 2, 39.99m },
                    { 10, 6, 8, 3, 44.99m },
                    { 11, 7, 1, 1, 999.99m },
                    { 12, 7, 2, 1, 849.99m },
                    { 13, 8, 6, 2, 59.99m },
                    { 14, 9, 3, 1, 1999.99m },
                    { 15, 10, 5, 5, 29.99m },
                    { 16, 11, 7, 1, 39.99m },
                    { 17, 12, 1, 3, 999.99m },
                    { 18, 13, 4, 1, 1599.99m },
                    { 19, 13, 2, 2, 849.99m },
                    { 20, 14, 6, 4, 59.99m },
                    { 21, 14, 5, 2, 29.99m },
                    { 22, 15, 8, 1, 44.99m },
                    { 23, 16, 3, 1, 1999.99m }
                });

            migrationBuilder.CreateIndex(
                name: "ix_order_lines_order_id",
                table: "order_lines",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_lines_product_id",
                table: "order_lines",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_customer_id",
                table: "orders",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_category_id",
                table: "products",
                column: "category_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_lines");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "customers");
        }
    }
}
