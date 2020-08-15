using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GridBeyond.Domain.Interfaces.Repository;
using GridBeyond.Domain.Interfaces.Services;
using GridBeyond.Domain.Services;
using NUnit.Framework;

namespace GridBeyond.Domain.Tests
{
    public class DataServiceTests
    {
        private IMarketDataService _service;

        [SetUp]
        public void SetUp()
        {
            var mock = new Moq.Mock<IMarketDataRepository>();

            
            _service = new MarketDataService(mock.Object);
        }

        [Test]
        public async Task ValidatorReturnIncorrectLines()
        {
            // Arrange
            var data = new List<string>
            {
                $"{DateTime.Now}, 50",
                $"{DateTime.Now},",
                $"{DateTime.Now}, abc",
                $"{DateTime.Now}",
            };

            // Act
            var result = await _service.ValidData(data);

            // Assert
            Assert.AreEqual(3, result.MalformedRecordLine.Count);
            Assert.AreEqual(1, result.ValidRecord.Count);
        }

        [Test]
        public async Task EventIsCalledOnMalformedRecords()
        {
            // Arrange
            var malformedCount = 0;

            _service.AddOnMalformedRecordEvent((sender, recordLine) => { malformedCount++; });

            var data = new List<string>
            {
                $"{DateTime.Now}, 50",
                $"{DateTime.Now},",
                $"{DateTime.Now}, abc",
                $"{DateTime.Now}",
            };

            // Act
            var result = await _service.ValidData(data);

            // Assert
            Assert.AreEqual(malformedCount, result.MalformedRecordLine.Count);
        }
    }
}