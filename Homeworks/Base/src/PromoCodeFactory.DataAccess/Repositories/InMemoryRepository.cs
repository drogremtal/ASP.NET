using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Abstractions.Repositories;
using PromoCodeFactory.Core.Domain;
using PromoCodeFactory.Core.Domain.Administration;
namespace PromoCodeFactory.DataAccess.Repositories
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected IEnumerable<T> Data { get; set; }

        public InMemoryRepository(IEnumerable<T> data)
        {
            Data = data;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return Task.FromResult(Data);
        }

        public Task<T> GetByIdAsync(Guid id)
        {
            return Task.FromResult(Data.FirstOrDefault(x => x.Id == id));
        }

        public Task<bool> DeleteByIdAsync(Guid id)
        {
            var newData = Data.Where(q => q.Id != id);
            Data = newData;
            return Task.FromResult(true);
        }

        public Task<T> UpdateAsync(T entity)
        {
            var newData = Data.FirstOrDefault(q => q.Id == entity.Id);

            if (newData ==  null)
            {
                return Task.FromResult(default(T));
            }
            var update = Data.Where(q => q.Id != entity.Id).Concat(new[] { entity });

            Data = update;
            return Task.FromResult(entity);

        }

        public Task<T> AddAsync(T entity)
        {
            var update = Data.Concat(new[] { entity });

            return Task.FromResult(entity);

        }
    }
}