
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using OrderTask.Models;

namespace OrderTask.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }


        #region ConnectionOldWay

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=> optionsBuilder.UseSqlServer("server = . ; database = OrderTask; Trusted_Connection = true ; trust server certificate = true "); //Trusted server Certificate adding it to connection string after .net 7 
        #endregion

        public DbSet<Order> Orders { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Governorate> governorates { get; set; }
        public DbSet<Product> products { get; set; }
        public DbSet<ProductOrder> productOrders { get; set; }
        public DbSet<User> users { get; set; }



        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{

        //    modelBuilder.Entity<Product>(entity =>
        //    {
        //        entity.HasKey(P => P.Id);

        //        entity.Property(P => P.Name)
        //        .HasColumnName("Product Name")
        //        .HasMaxLength(50)
        //        .IsRequired(true);

        //        entity.Property(P => P.Price)
        //        .HasColumnType("decimal(18,5)")
        //        .IsRequired(true);

        //        entity.HasMany<Order>()
        //        .WithMany()
        //        .UsingEntity<ProductOrder>()
        //        .HasKey(PO => new { PO.ProductId, PO.OrderId });


        //    });

        //    modelBuilder.Entity<City>().HasKey(c => c.Id);
        //    modelBuilder.Entity<City>().Property(c => c.Name)
        //        .HasColumnName(@"CITY")
        //        .HasMaxLength(50)
        //        .IsRequired(true);


        //    modelBuilder.Entity<Governorate>().HasKey(G => G.Id);
        //    modelBuilder.Entity<Governorate>().Property(G => G.Name)
        //        .HasColumnName(@"GOVERNORATE")
        //        .HasMaxLength(50)
        //        .IsRequired(true);



        //    //seed data 









        //    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Governorate)
                .WithMany()
                .HasForeignKey(o => o.GovernorateId);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.City)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CityId);

            //modelBuilder.Entity<City>()
            //    .HasOne(c => c.Governorate)
            //    .WithMany(g => g.Cities) // Ensure Governorate has a Cities property
            //    .HasForeignKey(c => c.GovernorateId);

            //modelBuilder.Entity<City>()
            //.HasOne(c => c.Governorate)
            //.WithMany() // No collection navigation on Governorate side
            //.HasForeignKey(c => c.GovernorateId)
            //.OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete issues

            modelBuilder.Entity<ProductOrder>()
                .HasKey(op => new { op.ProductId, op.OrderId });

            modelBuilder.Entity<ProductOrder>()
                .HasOne(op => op.Product)
                .WithMany(p => p.ProductOrders)
                .HasForeignKey(op => op.ProductId);

            modelBuilder.Entity<ProductOrder>()
                .HasOne(op => op.Order)
                .WithMany(o => o.ProductOrders)
                .HasForeignKey(op => op.OrderId);

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, UserName = "admin", Password = "password123" },
                new User { Id = 2, UserName = "user1", Password = "pass456" }
    );


        }
        public DbSet<OrderTask.Models.User> User { get; set; } = default!;
    }
}
