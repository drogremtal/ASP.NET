using Microsoft.EntityFrameworkCore;
using PromoCodeFactory.Core.Domain.Administration;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using System.Collections.Generic;
using System.Linq;

namespace PromoCodeFactory.DataAccess.Data;

public class DataContext : DbContext
{
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Preference> Preferences { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<PromoCode> PromoCodes { get; set; } = null!;
    public DbSet<CustomerPreference> CustomerPreferences { get; set; } = null!;

    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureEntities(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
   
    }

    // === Синхронная инициализация БД ===
    public void Initialize()
    {// Удаляем старую БД
        Database.EnsureDeleted();
        // Создаём новую
        Database.EnsureCreated();
        // Заполняем тестовыми данными
        SeedData();
    }

    private void SeedData()
    {
        if (Roles.Count() == 0)
        {
            var roles = FakeDataFactory.Roles.Select(q => new Role { Id =q.Id, Description = q.Description, Name = q.Name }).ToList();
            Roles.AddRange(roles);
        }
        this.ChangeTracker.Clear(); // Очищаем, чтобы не было конфликтов
        if (Employees.Count() == 0)
        {
            var employees = FakeDataFactory.Employees.Select(q => 
            new Employee { 
                Id=q.Id, 
                AppliedPromocodesCount = q.AppliedPromocodesCount,
                Email = q.Email,
                FirstName = q.FirstName,
                LastName = q.LastName,
                Role = q.Role,
                RoleId = q.Role.Id
            }).ToList();
            foreach (var employee in employees)
            {
                employee.RoleId = employee.Role.Id;
            }
            Employees.AddRange(employees);
        }

        if (Preferences.Count() == 0)
        {
            var preferences = FakeDataFactory.Preferences.ToList();
            Preferences.AddRange(preferences);
        }

        if (Customers.Count() == 0)
        {
            var customers = FakeDataFactory.Customers.ToList();
            Customers.AddRange(customers);
        }

        if (CustomerPreferences.Count() == 0)
        {
            var customerPreferences = new List<CustomerPreference>();
            var preferences = Preferences.ToList();
            var customers = Customers.ToList();

            foreach (var customer in customers)
            {
                foreach (var preference in preferences.Take(2))
                {
                    customerPreferences.Add(new CustomerPreference
                    {
                        CustomerId = customer.Id,
                        PreferenceId = preference.Id
                    });
                }
            }

            CustomerPreferences.AddRange(customerPreferences);
        }

        SaveChanges();
    }

    // === Конфигурация сущностей через Fluent API ===
    private void ConfigureEntities(ModelBuilder builder)
    {
        // Role
        builder.Entity<Role>(e =>
        {
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
            e.Property(x => x.Description).HasMaxLength(500);
        });

        // Employee
        builder.Entity<Employee>(e =>
        {
            e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            e.Property(x => x.Email).HasMaxLength(255).IsRequired();

            e.HasOne(x => x.Role)
             .WithMany()
             .HasForeignKey("RoleId")
             .IsRequired();
        });

        // Preference
        builder.Entity<Preference>(e =>
        {
            e.Property(x => x.Name).HasMaxLength(100).IsRequired();
        });

        // Customer
        builder.Entity<Customer>(e =>
        {
            e.Property(x => x.FirstName).HasMaxLength(100).IsRequired();
            e.Property(x => x.LastName).HasMaxLength(100).IsRequired();
            e.Property(x => x.Email).HasMaxLength(255).IsRequired();

            e.HasMany(x => x.PromoCodes)
             .WithOne()
             .HasForeignKey("CustomerId");
        });

        // PromoCode
        builder.Entity<PromoCode>(e =>
        {
            e.Property(x => x.Code).HasMaxLength(50).IsRequired();
            e.Property(x => x.ServiceInfo).HasMaxLength(500);
            e.Property(x => x.PartnerName).HasMaxLength(255);

            e.HasOne(x => x.PartnerManager)
             .WithMany()
             .HasForeignKey("EmployeeId");

            e.HasOne(x => x.Preference)
             .WithMany()
             .HasForeignKey("PreferenceId");
        });

        // CustomerPreference (Many-to-Many между Customer и Preference)
        builder.Entity<CustomerPreference>(e =>
        {
            e.HasKey(cp => new { cp.CustomerId, cp.PreferenceId });

            e.HasOne(cp => cp.Customer)
             .WithMany(c => c.PreferencesLink)
             .HasForeignKey(cp => cp.CustomerId);

            e.HasOne(cp => cp.Preference)
             .WithMany(p => p.CustomersLink)
             .HasForeignKey(cp => cp.PreferenceId);
        });
    }
}