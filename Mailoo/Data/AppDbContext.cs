using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Mailo.Data.Enums;
using Mailo.Models;

namespace Mailo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

		public AppDbContext()
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<OrderProduct>().ToTable("OrderProduct");
            modelBuilder.Entity<Payment>().ToTable("Payment");
            modelBuilder.Entity<Wishlist>().ToTable("Wishlist");
            modelBuilder.Entity<Contact>().ToTable("Contact");
            modelBuilder.Entity<User>().HasData(
            new User
            {
                ID = 2,
                FName = "Mai",
                LName = "Assem",
                Username = "MaiAssemAdmin123",
                PhoneNumber = "01011895030",
                Email = "mailostoreee@gmail.com",
                Password = "MaiiiAsss123#44",
                Gender = Gender.Female,
                UserType = UserType.Admin,
                Address = "Beni-Suef"
            }
            );
            modelBuilder.Entity<Employee>().HasData(
               new Employee
               {
                   ID = 1,
                   FName = "Ahmed",
                   LName = "Assem",
                   Username = "AhmedAssemAdmin123",
                   PhoneNumber = "01011890000",
                   Email = "Ahmedstoreee@gmail.com",
                   Password = "AhmedddAsss123#44",
                   Gender = Gender.Male,
                   UserType = UserType.Employee,
                   Address = "Beni-Suef"
               }
               );
            modelBuilder.Entity<Product>().HasKey(p => new { p.ID });

            #region M:M Tables
            modelBuilder.Entity<Order>()
            .HasOne(o => o.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<Payment>(o => o.OrderId);

            modelBuilder.Entity<OrderProduct>().HasKey(op => new { op.OrderID, op.ProductID });
            modelBuilder.Entity<OrderProduct>()
           .HasOne(op => op.order)
           .WithMany(o => o.OrderProducts)
           .HasForeignKey(op => op.OrderID);

          

            modelBuilder.Entity<Wishlist>().HasKey(op => new { op.UserID, op.ProductID });
            modelBuilder.Entity<Wishlist>()
           .HasOne(w => w.user)
           .WithMany(u => u.wishlist)
           .HasForeignKey(w => w.UserID);

            modelBuilder.Entity<Wishlist>()
                .HasOne(w => w.product)
                .WithMany(p => p.wishlists)
                .HasForeignKey(w => w.ProductID);

           

            //modelBuilder.Entity<Review>()
            //    .HasOne(r => r)
            //    .WithMany(p => p)
              //  .HasForeignKey(r => r.ProductID);
         

            #endregion

            #region Unique Attributes
            modelBuilder.Entity<User>()
            .HasIndex(u => u.PhoneNumber)
            .IsUnique();
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
  //          modelBuilder.Entity<Product>()
  //.HasIndex(u => u.Name)
  //.IsUnique();
            //modelBuilder.Entity<Productss>()
            //.HasIndex(op => new { op.ID, op.Colors, op.Size })
            //.IsUnique();
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
            #endregion

            #region Computed Attributes
            modelBuilder.Entity<User>().Property(u => u.ID).ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().Property(x => x.TotalPrice).HasComputedColumnSql("[Price]-([Discount]/100)*[Price]");
            modelBuilder.Entity<User>()
            .Property(u => u.FullName)
            .HasComputedColumnSql("[FName] + ' ' + [LName]");
           
            modelBuilder.Entity<Order>()
            .Property(e => e.TotalPrice)
            .HasComputedColumnSql("[OrderPrice] + [DeliveryFee]");
            #endregion

            // #region User Data
            // modelBuilder.Entity<User>()
            // .HasData(
            // new User { ID = 1, FName = "Yara", LName = "Emad Eldien", Username = "Yara_Emad4869", PhoneNumber = "+201127769084", Email = "Yara.Emad4869@gmail.com", Password = "YaraEmad4869", Gender = Gender.Female, UserType = UserType.Client, Address = "Al-Rawda Street, Off the Nile Courniche, Beni Suef" }
            // );
            //#endregion

            foreach (var rel in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                rel.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        #region DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Employee> Employees { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        #endregion
    }
}