using System.Text;
using EntretienCiteo.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace EntretienCiteo.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(e =>
        {
            e.HasOne(c => c.ParentCategory)
             .WithMany(c => c.SubCategories)
             .HasForeignKey(c => c.ParentCategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.Property(p => p.Price).HasColumnType("numeric(18,2)");
            // Optimistic concurrency token — incremented on every update
            e.Property(p => p.RowVersion).IsConcurrencyToken();
        });

        modelBuilder.Entity<OrderLine>(e =>
        {
            e.Property(l => l.UnitPrice).HasColumnType("numeric(18,2)");
        });

        SeedData(modelBuilder);

        ApplySnakeCaseNaming(modelBuilder);
    }

    /// <summary>
    /// Applies snake_case naming convention to all table names, column names,
    /// keys, foreign keys, and indexes — standard PostgreSQL convention.
    /// </summary>
    private static void ApplySnakeCaseNaming(ModelBuilder modelBuilder)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            if (entity.GetTableName() is { } tableName)
                entity.SetTableName(ToSnakeCase(tableName));

            foreach (var property in entity.GetProperties())
                property.SetColumnName(ToSnakeCase(property.GetColumnBaseName()));

            foreach (var key in entity.GetKeys())
                if (key.GetName() is { } keyName)
                    key.SetName(ToSnakeCase(keyName));

            foreach (var fk in entity.GetForeignKeys())
                if (fk.GetConstraintName() is { } fkName)
                    fk.SetConstraintName(ToSnakeCase(fkName));

            foreach (var index in entity.GetIndexes())
                if (index.GetDatabaseName() is { } indexName)
                    index.SetDatabaseName(ToSnakeCase(indexName));
        }
    }

    private static string ToSnakeCase(string name)
    {
        var sb = new StringBuilder(name.Length + 4);
        for (var i = 0; i < name.Length; i++)
        {
            var c = name[i];
            if (i > 0 && char.IsUpper(c) &&
                (char.IsLower(name[i - 1]) || (i + 1 < name.Length && char.IsLower(name[i + 1]))))
            {
                sb.Append('_');
            }
            sb.Append(char.ToLowerInvariant(c));
        }
        return sb.ToString();
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics", ParentCategoryId = null },
            new Category { Id = 2, Name = "Clothing", ParentCategoryId = null },
            new Category { Id = 3, Name = "Books", ParentCategoryId = null },
            new Category { Id = 4, Name = "Smartphones", ParentCategoryId = 1 },
            new Category { Id = 5, Name = "Laptops", ParentCategoryId = 1 },
            new Category { Id = 6, Name = "Men's Clothing", ParentCategoryId = 2 },
            new Category { Id = 7, Name = "Women's Clothing", ParentCategoryId = 2 }
        );

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "iPhone 16", Price = 999.99m, Stock = 50, CategoryId = 4, RowVersion = new Guid("a1000000-0000-0000-0000-000000000001") },
            new Product { Id = 2, Name = "Samsung Galaxy S25", Price = 849.99m, Stock = 40, CategoryId = 4, RowVersion = new Guid("a1000000-0000-0000-0000-000000000002") },
            new Product { Id = 3, Name = "MacBook Pro 14\"", Price = 1999.99m, Stock = 20, CategoryId = 5, RowVersion = new Guid("a1000000-0000-0000-0000-000000000003") },
            new Product { Id = 4, Name = "Dell XPS 15", Price = 1599.99m, Stock = 25, CategoryId = 5, RowVersion = new Guid("a1000000-0000-0000-0000-000000000004") },
            new Product { Id = 5, Name = "Men's T-Shirt", Price = 29.99m, Stock = 200, CategoryId = 6, RowVersion = new Guid("a1000000-0000-0000-0000-000000000005") },
            new Product { Id = 6, Name = "Women's Dress", Price = 59.99m, Stock = 150, CategoryId = 7, RowVersion = new Guid("a1000000-0000-0000-0000-000000000006") },
            new Product { Id = 7, Name = "Clean Code", Price = 39.99m, Stock = 100, CategoryId = 3, RowVersion = new Guid("a1000000-0000-0000-0000-000000000007") },
            new Product { Id = 8, Name = "The Pragmatic Programmer", Price = 44.99m, Stock = 80, CategoryId = 3, RowVersion = new Guid("a1000000-0000-0000-0000-000000000008") }
        );

        modelBuilder.Entity<Customer>().HasData(
            // Customers with orders
            new Customer { Id = 1, Name = "Alice Martin",    Email = "alice@example.com" },
            new Customer { Id = 2, Name = "Bob Dupont",      Email = "bob@example.com" },
            new Customer { Id = 3, Name = "Charlie Lefebvre",Email = "charlie@example.com" },
            new Customer { Id = 4, Name = "Diana Bernard",   Email = "diana@example.com" },
            new Customer { Id = 5, Name = "Etienne Moreau",  Email = "etienne@example.com" },
            new Customer { Id = 6, Name = "Fanny Simon",     Email = "fanny@example.com" },
            new Customer { Id = 7, Name = "Georges Laurent", Email = "georges@example.com" },
            // Customers with no orders yet
            new Customer { Id = 8, Name = "Hélène Petit",   Email = "helene@example.com" },
            new Customer { Id = 9, Name = "Ivan Rousseau",   Email = "ivan@example.com" },
            new Customer { Id = 10, Name = "Julie Blanc",    Email = "julie@example.com" }
        );

        // Alice — 3 orders
        modelBuilder.Entity<Order>().HasData(
            new Order { Id = 1,  CustomerId = 1, Status = OrderStatus.Confirmed, CreatedAt = new DateTime(2024,  1, 15, 0, 0, 0, DateTimeKind.Utc) },
            new Order { Id = 2,  CustomerId = 1, Status = OrderStatus.Shipped,   CreatedAt = new DateTime(2024,  3, 20, 0, 0, 0, DateTimeKind.Utc) },
            new Order { Id = 3,  CustomerId = 1, Status = OrderStatus.Pending,   CreatedAt = new DateTime(2025, 11, 10, 0, 0, 0, DateTimeKind.Utc) },
            // Bob — 2 orders
            new Order { Id = 4,  CustomerId = 2, Status = OrderStatus.Confirmed, CreatedAt = new DateTime(2024,  2, 10, 0, 0, 0, DateTimeKind.Utc) },
            new Order { Id = 5,  CustomerId = 2, Status = OrderStatus.Cancelled, CreatedAt = new DateTime(2024,  6,  5, 0, 0, 0, DateTimeKind.Utc) },
            // Charlie — 4 orders
            new Order { Id = 6,  CustomerId = 3, Status = OrderStatus.Shipped,   CreatedAt = new DateTime(2024,  1, 25, 0, 0, 0, DateTimeKind.Utc) },
            new Order { Id = 7,  CustomerId = 3, Status = OrderStatus.Confirmed, CreatedAt = new DateTime(2024,  4, 12, 0, 0, 0, DateTimeKind.Utc) },
            new Order { Id = 8,  CustomerId = 3, Status = OrderStatus.Shipped,   CreatedAt = new DateTime(2024,  9,  3, 0, 0, 0, DateTimeKind.Utc) },
            new Order { Id = 9,  CustomerId = 3, Status = OrderStatus.Pending,   CreatedAt = new DateTime(2025, 12,  1, 0, 0, 0, DateTimeKind.Utc) },
            // Diana — 1 order
            new Order { Id = 10, CustomerId = 4, Status = OrderStatus.Confirmed, CreatedAt = new DateTime(2024,  7, 18, 0, 0, 0, DateTimeKind.Utc) },
            // Etienne — 3 orders
            new Order { Id = 11, CustomerId = 5, Status = OrderStatus.Shipped,   CreatedAt = new DateTime(2024,  2, 28, 0, 0, 0, DateTimeKind.Utc) },
            new Order { Id = 12, CustomerId = 5, Status = OrderStatus.Confirmed, CreatedAt = new DateTime(2024,  8, 15, 0, 0, 0, DateTimeKind.Utc) },
            new Order { Id = 13, CustomerId = 5, Status = OrderStatus.Pending,   CreatedAt = new DateTime(2025,  1,  5, 0, 0, 0, DateTimeKind.Utc) },
            // Fanny — 2 orders
            new Order { Id = 14, CustomerId = 6, Status = OrderStatus.Shipped,   CreatedAt = new DateTime(2024,  5, 22, 0, 0, 0, DateTimeKind.Utc) },
            new Order { Id = 15, CustomerId = 6, Status = OrderStatus.Confirmed, CreatedAt = new DateTime(2024, 11, 30, 0, 0, 0, DateTimeKind.Utc) },
            // Georges — 1 order
            new Order { Id = 16, CustomerId = 7, Status = OrderStatus.Pending,   CreatedAt = new DateTime(2025,  2,  1, 0, 0, 0, DateTimeKind.Utc) }
        );

        modelBuilder.Entity<OrderLine>().HasData(
            // Order 1 — Alice: iPhone 16 x2, Clean Code x1
            new OrderLine { Id = 1,  OrderId = 1,  ProductId = 1, Quantity = 2, UnitPrice = 999.99m  },
            new OrderLine { Id = 2,  OrderId = 1,  ProductId = 7, Quantity = 1, UnitPrice = 39.99m   },
            // Order 2 — Alice: MacBook Pro x1
            new OrderLine { Id = 3,  OrderId = 2,  ProductId = 3, Quantity = 1, UnitPrice = 1999.99m },
            // Order 3 — Alice: Men's T-Shirt x3, Women's Dress x1
            new OrderLine { Id = 4,  OrderId = 3,  ProductId = 5, Quantity = 3, UnitPrice = 29.99m   },
            new OrderLine { Id = 5,  OrderId = 3,  ProductId = 6, Quantity = 1, UnitPrice = 59.99m   },
            // Order 4 — Bob: Samsung Galaxy S25 x1, The Pragmatic Programmer x1
            new OrderLine { Id = 6,  OrderId = 4,  ProductId = 2, Quantity = 1, UnitPrice = 849.99m  },
            new OrderLine { Id = 7,  OrderId = 4,  ProductId = 8, Quantity = 1, UnitPrice = 44.99m   },
            // Order 5 — Bob: Dell XPS 15 x1 (cancelled)
            new OrderLine { Id = 8,  OrderId = 5,  ProductId = 4, Quantity = 1, UnitPrice = 1599.99m },
            // Order 6 — Charlie: Clean Code x2, The Pragmatic Programmer x3
            new OrderLine { Id = 9,  OrderId = 6,  ProductId = 7, Quantity = 2, UnitPrice = 39.99m   },
            new OrderLine { Id = 10, OrderId = 6,  ProductId = 8, Quantity = 3, UnitPrice = 44.99m   },
            // Order 7 — Charlie: iPhone 16 x1, Samsung Galaxy S25 x1
            new OrderLine { Id = 11, OrderId = 7,  ProductId = 1, Quantity = 1, UnitPrice = 999.99m  },
            new OrderLine { Id = 12, OrderId = 7,  ProductId = 2, Quantity = 1, UnitPrice = 849.99m  },
            // Order 8 — Charlie: Women's Dress x2
            new OrderLine { Id = 13, OrderId = 8,  ProductId = 6, Quantity = 2, UnitPrice = 59.99m   },
            // Order 9 — Charlie: MacBook Pro x1
            new OrderLine { Id = 14, OrderId = 9,  ProductId = 3, Quantity = 1, UnitPrice = 1999.99m },
            // Order 10 — Diana: Men's T-Shirt x5
            new OrderLine { Id = 15, OrderId = 10, ProductId = 5, Quantity = 5, UnitPrice = 29.99m   },
            // Order 11 — Etienne: Clean Code x1
            new OrderLine { Id = 16, OrderId = 11, ProductId = 7, Quantity = 1, UnitPrice = 39.99m   },
            // Order 12 — Etienne: iPhone 16 x3
            new OrderLine { Id = 17, OrderId = 12, ProductId = 1, Quantity = 3, UnitPrice = 999.99m  },
            // Order 13 — Etienne: Dell XPS 15 x1, Samsung Galaxy S25 x2
            new OrderLine { Id = 18, OrderId = 13, ProductId = 4, Quantity = 1, UnitPrice = 1599.99m },
            new OrderLine { Id = 19, OrderId = 13, ProductId = 2, Quantity = 2, UnitPrice = 849.99m  },
            // Order 14 — Fanny: Women's Dress x4, Men's T-Shirt x2
            new OrderLine { Id = 20, OrderId = 14, ProductId = 6, Quantity = 4, UnitPrice = 59.99m   },
            new OrderLine { Id = 21, OrderId = 14, ProductId = 5, Quantity = 2, UnitPrice = 29.99m   },
            // Order 15 — Fanny: The Pragmatic Programmer x1
            new OrderLine { Id = 22, OrderId = 15, ProductId = 8, Quantity = 1, UnitPrice = 44.99m   },
            // Order 16 — Georges: MacBook Pro x1
            new OrderLine { Id = 23, OrderId = 16, ProductId = 3, Quantity = 1, UnitPrice = 1999.99m }
        );
    }
}
