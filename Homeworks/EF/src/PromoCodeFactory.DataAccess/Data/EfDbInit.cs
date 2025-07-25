﻿using PromoCodeFactory.DataAccess.DBContext;

namespace PromoCodeFactory.DataAccess.Data
{
    public interface IEfDbInit
    {
        void Initialize();
    }

    public class  EfDbInit : IEfDbInit
    {

        private readonly DataContext _DataContext;
        public EfDbInit() { }

        // === Синхронная инициализация БД ===
        public void Initialize()
        {// Удаляем старую БД
            _DataContext.Database.EnsureDeleted();
            // Создаём новую
            _DataContext.Database.EnsureCreated();


            _DataContext.Roles.AddRange(FakeDataFactory.Roles);
            _DataContext.SaveChanges();


            _DataContext.Employees.AddRange(FakeDataFactory.Employees);
            _DataContext.SaveChanges();


            _DataContext.Preferences.AddRange(FakeDataFactory.Preferences);
            _DataContext.SaveChanges();

            _DataContext.Customers.AddRange(FakeDataFactory.Customers);
            _DataContext.SaveChanges();

            _DataContext.PromoCodes.AddRange(FakeDataFactory.PromoCodes);

            _DataContext.SaveChanges();
        }

    }
}
