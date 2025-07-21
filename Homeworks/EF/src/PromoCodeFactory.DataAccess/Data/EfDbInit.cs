using PromoCodeFactory.DataAccess.DBContext;

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
            _DataContext.Database.EnsureCreated();
        }

    }
}
