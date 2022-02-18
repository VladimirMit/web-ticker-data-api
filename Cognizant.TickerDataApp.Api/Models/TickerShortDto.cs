namespace Cognizant.TickerDataApp.Api.Controllers
{
    public class TickerShortDto
    {
        public TickerShortDto(string id, string code)
        {
            Id = id;
            Code = code;
        }

        public string Id { get; }

        public string Code { get; }
    }
}