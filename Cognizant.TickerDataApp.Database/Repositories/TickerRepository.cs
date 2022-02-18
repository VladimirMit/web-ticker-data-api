using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cognizant.TickerDataApp.Domain.Entities;
using Cognizant.TickerDataApp.Domain.Repositories;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Cognizant.TickerDataApp.Database.Repositories
{
    public class TickersRepository : ITickersRepository
    {
        private readonly IMongoCollection<Ticker> _tickersCollection;

        public TickersRepository(IOptions<StoreSettings> options)
        {
            var mongoClient = new MongoClient(options.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(options.Value.DatabaseName);

            _tickersCollection = mongoDatabase.GetCollection<Ticker>(options.Value.TickersCollectionName);
        }

        public Task<Ticker> Find(string id, CancellationToken cancellationToken)
        {
            return _tickersCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<ICollection<Ticker>> GetAll(CancellationToken cancellationToken)
        {
            return await _tickersCollection.Find(_ => true).ToListAsync(cancellationToken);
        }

        public Task Add(Ticker entity, CancellationToken cancellationToken)
        {
            return _tickersCollection.InsertOneAsync(entity, null, cancellationToken);
        }
    }
}
