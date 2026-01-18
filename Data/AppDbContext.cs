using Microsoft.EntityFrameworkCore;
using SmartStoreReservation.Core.Entities;

namespace SmartStoreReservation.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Shop> Shops { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Cabin> Cabins { get; set; }
    public DbSet<Reservation> Reservations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map Tables (snake_case)
        modelBuilder.Entity<Shop>().ToTable("shops");
        modelBuilder.Entity<Category>().ToTable("categories");
        modelBuilder.Entity<Product>().ToTable("products");
        modelBuilder.Entity<ProductCategory>().ToTable("product_categories");
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Cabin>().ToTable("cabins");
        modelBuilder.Entity<Reservation>().ToTable("reservations");

        // Map Columns (snake_case)
        
        // Shops
        modelBuilder.Entity<Shop>(e => {
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.Location).HasColumnName("location");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        // Products
        modelBuilder.Entity<Product>(e => {
             e.Property(x => x.Id).HasColumnName("id");
             e.Property(x => x.Name).HasColumnName("name");
             e.Property(x => x.Price).HasColumnName("price").HasColumnType("decimal(18,2)");
             e.Property(x => x.Stock).HasColumnName("stock");
             e.Property(x => x.Size).HasColumnName("size");
             e.Property(x => x.Color).HasColumnName("color");
             e.Property(x => x.ImageUrl).HasColumnName("image_url");
             e.Property(x => x.ShopId).HasColumnName("shop_id");
             e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        // Categories
         modelBuilder.Entity<Category>(e => {
             e.Property(x => x.Id).HasColumnName("id");
             e.Property(x => x.Name).HasColumnName("name");
        });

         // Cabins
         modelBuilder.Entity<Cabin>(e => {
             e.Property(x => x.Id).HasColumnName("id");
             e.Property(x => x.CabinNumber).HasColumnName("cabin_number");
             e.Property(x => x.ShopId).HasColumnName("shop_id");
             e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

         // Users
         modelBuilder.Entity<User>(e => {
             e.Property(x => x.Id).HasColumnName("id");
             e.Property(x => x.Name).HasColumnName("name");
             e.Property(x => x.Email).HasColumnName("email");
             e.Property(x => x.PasswordHash).HasColumnName("password_hash");
             e.Property(x => x.Measurements).HasColumnName("measurements");
             e.Property(x => x.StylePreferences).HasColumnName("style_preferences");
             e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        // Reservations
        modelBuilder.Entity<Reservation>(e => {
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.UserId).HasColumnName("user_id");
            e.Property(x => x.ProductId).HasColumnName("product_id");
            e.Property(x => x.CabinId).HasColumnName("cabin_id");
            e.Property(x => x.Date).HasColumnName("date");
            e.Property(x => x.Hour).HasColumnName("hour");
            e.Property(x => x.AccessCode).HasColumnName("access_code");
            e.Property(x => x.Duration).HasColumnName("duration");
            e.Property(x => x.Status).HasColumnName("status");
            e.Property(x => x.CreatedAt).HasColumnName("created_at");
        });

        // Product_Categories
        modelBuilder.Entity<ProductCategory>()
            .HasKey(pc => new { pc.ProductId, pc.CategoryId }); // Keeping Composite Key for logic simplicity, assuming 'id' column in DB is surrogate
            
        modelBuilder.Entity<ProductCategory>(e => {
             // We can ignore the 'id' column in EF mapping if we use composite key logic, or map it if needed. 
             // Ideally we shouldn't map 'id' if we use composite key as PK in EF.
             e.Property(x => x.ProductId).HasColumnName("product_id");
             e.Property(x => x.CategoryId).HasColumnName("category_id");
             e.ToTable("product_categories");
        });

        modelBuilder.Entity<ProductCategory>()
            .HasOne(pc => pc.Product)
            .WithMany(p => p.ProductCategories)
            .HasForeignKey(pc => pc.ProductId);

        modelBuilder.Entity<ProductCategory>()
            .HasOne(pc => pc.Category)
            .WithMany(c => c.ProductCategories)
            .HasForeignKey(pc => pc.CategoryId);

        // Reservations - Configure relationships with NoAction to avoid cascade cycles
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Product)
            .WithMany()
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Cabin)
            .WithMany()
            .HasForeignKey(r => r.CabinId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
