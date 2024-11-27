using Cafe.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Infrastructure.Context
{
    public class CafeDbContext : DbContext
    {
        public CafeDbContext(DbContextOptions<CafeDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<CafeEntity> Cafes { get; set; }
        public DbSet<EmployeeEntity> Employees { get; set; }
        public DbSet<CafeEmployeeEntity> CafeEmployees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CafeEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(250);
                entity.Property(e => e.Logo).HasMaxLength(2000);
            });

            modelBuilder.Entity<EmployeeEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EmployeeId).IsRequired().HasMaxLength(9);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(10);
                entity.Property(e => e.EmailAddress).IsRequired();
                entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(8);
            });

            modelBuilder.Entity<CafeEmployeeEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate);

                entity.HasOne(ce => ce.CafeEntity)
                      .WithMany(c => c.CafeEmployees)
                      .HasForeignKey(ce => ce.CafeId);

                entity.HasOne(ce => ce.EmployeeEntity)
                      .WithMany(e => e.CafeEmployees)
                      .HasForeignKey(ce => ce.EmployeeId);

                entity.HasIndex(ce => new { ce.EmployeeId, ce.IsActive })
                      .HasFilter("[IsActive] = 1")
                      .IsUnique();
            });
            // Seed Data
            var cafes = new List<CafeEntity>
            {
                new CafeEntity
                {
                    Id = Guid.Parse("c8f1c92a-1234-4e6d-915f-223344d4f73a"),
                    Name = "Su Cafe",
                    Description = "A cozy place with great coffee and pastries.",
                    Location = "123 Maple Street, Springfield",
                    Logo = "",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new CafeEntity
                {
                    Id = Guid.Parse("a8f3c92a-2345-4e6d-915f-223344d4f74b"),
                    Name = "City Brew",
                    Description = "Modern cafe serving organic coffee and teas.",
                    Location = "456 Oak Avenue, Riverdale",
                    Logo = "",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new CafeEntity
                {
                    Id = Guid.Parse("d9e4c92a-3456-4e6d-915f-223344d4f75c"),
                    Name = "Green Leaf",
                    Description = "Vegan-friendly cafe with a range of smoothies.",
                    Location = "789 Pine Road, Sunnyvale",
                    Logo = "",
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                }
            };

            var employees = new List<EmployeeEntity>
            {
                new EmployeeEntity
                {
                    Id = Guid.Parse("a1b2c3d4-e5f6-7890-1234-56789abcdef0"),
                    EmployeeId = "UI0000001",
                    Name = "John",
                    EmailAddress = "john.doe@gmail.com",
                    PhoneNumber = "88888884",
                    Gender = GenderType.Male,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new EmployeeEntity
                {
                    Id = Guid.Parse("b2c3d4e5-f6a7-8901-2345-67890abcdef1"),
                    EmployeeId = "UI0000002",
                    Name = "Smith",
                    EmailAddress = "jane.smith@gmail.com",
                    PhoneNumber = "88888885",
                    Gender = GenderType.Female,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new EmployeeEntity
                {
                    Id = Guid.Parse("c3d4e5f6-a7b8-9012-3456-7890abcdef12"),
                    EmployeeId = "UI0000003",
                    Name = "Alex",
                    EmailAddress = "alex.johnson@gmail.com",
                    PhoneNumber = "88888886",
                    Gender = GenderType.Male,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new EmployeeEntity
                {
                    Id = Guid.Parse("d4e5f6a7-b8c9-0123-4567-890abcdef123"),
                    EmployeeId = "UI0000004",
                    Name = "Emily",
                    EmailAddress = "emily.davis@gmail.com",
                    PhoneNumber = "88888887",
                    Gender = GenderType.Female,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                },
                new EmployeeEntity
                {
                    Id = Guid.Parse("e5f6a7b8-c9d0-1234-5678-90abcdef1234"),
                    EmployeeId = "UI0000005",
                    Name = "Daniel",
                    EmailAddress = "daniel.brown@gmail.com",
                    PhoneNumber = "88888888",
                    Gender = GenderType.Male,
                    IsActive = true,
                    CreatedDate = DateTime.UtcNow
                }
            };

            modelBuilder.Entity<CafeEntity>().HasData(cafes);
            modelBuilder.Entity<EmployeeEntity>().HasData(employees);
        }
    }
}