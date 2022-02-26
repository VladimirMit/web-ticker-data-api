using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Company.TickerDataApp.Api.Models;
using Company.TickerDataApp.Application.Features.Tickers;
using Company.TickerDataApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Company.TickerDataApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TickersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TickersController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Ticker))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(string id, CancellationToken cancellationToken)
        {
            var ticker = await _mediator.Send(new GetById.Request(id), cancellationToken);

            if (ticker == null)
                return NotFound();
            
            return Ok(ticker);
        }

        [HttpGet]
        public async Task<ICollection<TickerShortDto>> GetAll(CancellationToken cancellationToken)
        {
            var tickers = await _mediator.Send(new GetAll.Request(), cancellationToken);

            return tickers.Select(t =>
                    new TickerShortDto(t.Id, t.Code, t.HistoryRecords.Min(r => r.Date),
                        t.HistoryRecords.Max(r => r.Date)))
                .ToList();
        }

        [HttpPost]
        public async Task<Ticker> Post([FromBody]CreateTicker.Request request)
        {
            var ticker = await _mediator.Send(request);

            return ticker;
        }
    }
}
