using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using Cognizant.TickerDataApp.Application.Features.Tickers;
using Cognizant.TickerDataApp.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Cognizant.TickerDataApp.Api.Controllers
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

            return tickers.Select(t => new TickerShortDto(t.Id, t.Code)).ToList();
        } 
    }
}
