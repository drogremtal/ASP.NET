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

}