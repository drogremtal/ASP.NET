using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoCodeFactory.DataAccess.Data
{
    public class  EfDbInit
    {

        private readonly DataContext _DataContext;
        public EfDbInit() { }

        // === Синхронная инициализация БД ===
        public void Initialize()
        {// Удаляем старую БД
            _DataContext.Database.EnsureDeleted();
            // Создаём новую
            _DataContext.Database.EnsureCreated();
            _DataContext.Database.EnsureCreated();
        }

    }
}
