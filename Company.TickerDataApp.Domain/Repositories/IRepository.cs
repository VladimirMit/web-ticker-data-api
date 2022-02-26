using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Company.TickerDataApp.Domain.Entities;

namespace Company.TickerDataApp.Domain.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        public Task<T> Find(string id, CancellationToken cancellationToken);
        public Task<ICollection<T>> GetAll(CancellationToken cancellationToken);
        public Task Add(T entity, CancellationToken cancellationToken);
    }
}
