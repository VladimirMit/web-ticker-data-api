using System.Threading;
using System.Threading.Tasks;
using Company.TickerDataApp.Domain.Entities;
using Company.TickerDataApp.Domain.Repositories;
using MediatR;

namespace Company.TickerDataApp.Application.Features.Tickers
{
    public class GetById
    {
        public class Request : IRequest<Ticker>
        {
            public Request(string id)
            {
                Id = id;
            }

            public string Id { get; }
        }

        internal class Handler : IRequestHandler<Request, Ticker>
        {
            private readonly ITickersRepository _tickersRepository;

            public Handler(ITickersRepository tickersRepository)
            {
                _tickersRepository = tickersRepository;
            }
            public Task<Ticker> Handle(Request request, CancellationToken cancellationToken)
            {
                return _tickersRepository.Find(request.Id, cancellationToken);
            }
        }
    }
}
