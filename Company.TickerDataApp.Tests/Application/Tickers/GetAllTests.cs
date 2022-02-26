using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Company.TickerDataApp.Application.Features.Tickers;
using Company.TickerDataApp.Domain.Entities;
using Company.TickerDataApp.Domain.Repositories;
using Company.TickerDataApp.Domain.ValueObjects;
using Moq;
using NUnit.Framework;

namespace Company.TickerDataApp.Tests.Application.Tickers
{
    [TestFixture]
    internal class GetAllTests
    {
        [Test]
        public async Task ShouldReturnAll()
        {
            var expectedResult = new[]
            {
                new Ticker(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Array.Empty<HistoryRecord>()),
                new Ticker(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), new[]
                {
                    new HistoryRecord(DateTime.Today, 200.3m),
                    new HistoryRecord(DateTime.MinValue, 245.31m)
                })
            };
            
            var repository = SetupRepository(expectedResult); 
            
            var handler = new GetAll.Handler(repository);

            var result = await handler.Handle(new GetAll.Request(), CancellationToken.None);

            foreach (var r in result)
            {
                var er = expectedResult.FirstOrDefault(ex => ex.Id == r.Id);

                Assert.IsNotNull(er);
                Assert.AreEqual(er.Code, r.Code);

                foreach (var ehr in er.HistoryRecords)
                {
                    Assert.True(r.HistoryRecords.Contains(ehr));
                }
            }
        }

        private static ITickersRepository SetupRepository(ICollection<Ticker> data)
        {
            var repositoryMock = new Mock<ITickersRepository>();

            repositoryMock.Setup(r => r.GetAll(CancellationToken.None)).ReturnsAsync(data);

            return repositoryMock.Object;
        }
    }
}
