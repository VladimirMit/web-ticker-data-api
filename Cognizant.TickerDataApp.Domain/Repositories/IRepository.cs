using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cognizant.TickerDataApp.Domain.Entities;

namespace Cognizant.TickerDataApp.Domain.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        public Task<T> Find(string id, CancellationToken cancellationToken);
        public Task<ICollection<T>> GetAll(CancellationToken cancellationToken);
        public Task Add(T entity, CancellationToken cancellationToken);
    }
}
