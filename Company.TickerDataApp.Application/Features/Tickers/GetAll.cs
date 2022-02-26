using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Company.TickerDataApp.Domain.Entities;
using Company.TickerDataApp.Domain.Repositories;
using MediatR;

namespace Company.TickerDataApp.Application.Features.Tickers
{
    public class GetAll
    {
        public class Request : IRequest<ICollection<Ticker>>
        {
        }

        internal class Handler : IRequestHandler<Request, ICollection<Ticker>>
        {
            private readonly ITickersRepository _tickersRepository;

            public Handler(ITickersRepository tickersRepository)
            {
                _tickersRepository = tickersRepository;
            }
            public Task<ICollection<Ticker>> Handle(Request request, CancellationToken cancellationToken)
            {
                return _tickersRepository.GetAll(cancellationToken);
            }
        }
    }
}
