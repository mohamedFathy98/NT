
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using OrderTask.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using OrderTask.ViewModel;

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

            

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).ValueGeneratedOnAdd();
                entity.Property(u => u.FirstName).HasMaxLength(100).IsRequired();
                entity.Property(u => u.LastName).HasMaxLength(100).IsRequired();
                entity.Property(u => u.UserName).HasMaxLength(100).IsRequired();
                entity.Property(u => u.Email).HasMaxLength(256).IsRequired();
                entity.Property(u => u.Password).HasMaxLength(256).IsRequired();
                entity.Property(u => u.ResetToken).HasMaxLength(256).IsRequired(false);
                entity.Property(u => u.ResetTokenExpiry);
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);
                entity.Property(o => o.Id).ValueGeneratedOnAdd();
                entity.Property(o => o.Name).HasMaxLength(200).IsRequired();
                entity.Property(o => o.Address).HasMaxLength(500);
                entity.Property(o => o.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(o => o.CreatedBy).HasMaxLength(100).IsRequired(); // New constraint
                entity.HasOne(o => o.Governorate)
                    .WithMany()
                    .HasForeignKey(o => o.GovernorateId);
                entity.HasOne(o => o.City)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(o => o.CityId);
            });

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





        }
    }
}
