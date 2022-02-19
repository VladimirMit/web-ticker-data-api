using System;
using System.Threading;
using System.Threading.Tasks;
using Cognizant.TickerDataApp.Application.Services;
using Cognizant.TickerDataApp.Domain.Entities;
using Cognizant.TickerDataApp.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Cognizant.TickerDataApp.Application.Features.Tickers
{
    public class CreateTicker
    {
        public class Request : IRequest<Ticker>
        {
            public Request(string name, DateTime dateFrom, DateTime dateTo)
            {
                Name = name;
                DateFrom = dateFrom;
                DateTo = dateTo;
            }

            public string Name { get; set;  }
            public DateTime DateFrom { get; set; }
            public DateTime DateTo { get; set; }
        }

        public class RequestValidator : AbstractValidator<Request>
        {
            public RequestValidator()
            {
                RuleFor(request => request.DateFrom).LessThan(request => request.DateTo);
            }
        }

        internal class Handler : IRequestHandler<Request, Ticker>
        {
            private readonly ITickersSourceService _tickersSourceService;
            private readonly ITickersRepository _tickersRepository;
            private readonly RequestValidator _validator;

            public Handler(ITickersSourceService tickersSourceService, ITickersRepository tickersRepository, RequestValidator validator)
            {
                _tickersSourceService = tickersSourceService;
                _tickersRepository = tickersRepository;
                _validator = validator;
            }

            public async Task<Ticker> Handle(Request request, CancellationToken cancellationToken)
            {
                await _validator.ValidateAndThrowAsync(request, cancellationToken);

                var name = request.Name.ToUpper();
                
                var historyRecords = await _tickersSourceService.GetTickerInfo(name, request.DateFrom,
                    request.DateTo, cancellationToken);

                var ticker = new Ticker(Guid.NewGuid().ToString(), name, historyRecords);

                await _tickersRepository.Add(ticker, cancellationToken);

                return ticker;
            }
        }
    }
}
