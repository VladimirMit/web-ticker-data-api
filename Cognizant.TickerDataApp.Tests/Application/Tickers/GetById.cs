using System;
using System.Threading;
using System.Threading.Tasks;
using Cognizant.TickerDataApp.Application.Features.Tickers;
using Cognizant.TickerDataApp.Domain.Entities;
using Cognizant.TickerDataApp.Domain.Repositories;
using Cognizant.TickerDataApp.Domain.ValueObjects;
using Moq;
using NUnit.Framework;

namespace Cognizant.TickerDataApp.Tests.Application.Tickers
{
    [TestFixture]
    class GetByIdTests
    {
        [Test]
        public async Task WhenExist_ShouldReturnExpectedResult()
        {
            var expectedResult =
                new Ticker(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), new[]
                {
                    new HistoryRecord(DateTime.Today, 200.3m),
                    new HistoryRecord(DateTime.MinValue, 245.31m)
                });
            
            var repository = SetupRepository(expectedResult); 
            
            var handler = new GetById.Handler(repository);

            var result = await handler.Handle(new GetById.Request(expectedResult.Id), CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Id, result.Id);
            Assert.AreEqual(expectedResult.Code, result.Code);
            Assert.AreEqual(expectedResult.HistoryRecords, result.HistoryRecords);
        }

        [Test]
        public async Task WhenDoesNotExist_ShouldReturnNull()
        {
            var repository = SetupRepository(null);

            var handler = new GetById.Handler(repository);

            var result = await handler.Handle(new GetById.Request(Guid.NewGuid().ToString()), CancellationToken.None);
            
            Assert.IsNull(result);
        }

        private static ITickersRepository SetupRepository(Ticker ticker)
        {
            var repositoryMock = new Mock<ITickersRepository>();

            repositoryMock.Setup(r => r.Find(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(ticker);

            return repositoryMock.Object;
        }
    }
}
