using System.Threading;
using System.Threading.Tasks;
using Cognizant.TickerDataApp.Domain.Entities;
using Cognizant.TickerDataApp.Domain.Repositories;
using MediatR;

namespace Cognizant.TickerDataApp.Application.Features.Tickers
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
