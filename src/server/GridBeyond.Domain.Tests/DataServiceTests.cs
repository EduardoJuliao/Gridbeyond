using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GridBeyond.Domain.Entities;
using GridBeyond.Domain.Interfaces.Repository;
using GridBeyond.Domain.Interfaces.Services;
using GridBeyond.Domain.Models;
using GridBeyond.Domain.Services;
using Moq;
using NUnit.Framework;

namespace GridBeyond.Domain.Tests
{
    public class DataServiceTests
    {
        private IMarketDataService _service;

        private readonly List<MarketData> _repoData = new List<MarketData>();

        [SetUp]
        public void SetUp()
        {
            var mock = new Moq.Mock<IMarketDataRepository>();

            mock.Setup(x => x.Exists(It.IsAny<Expression<Func<MarketData, bool>>>()))
                .Returns((Expression<Func<MarketData, bool>> expression) => _repoData.Any(expression.Compile()));

            mock.Setup(x => x.Insert(It.IsAny<InsertDataModel>()))
                .Callback((InsertDataModel data) => _repoData.Add(new MarketData{Date = data.Date, MarketPriceEX1 = data.MarketPriceEX1}));

            mock.Setup(x => x.Insert(It.IsAny<IEnumerable<InsertDataModel>>()))
                .Callback((IEnumerable<InsertDataModel> data) =>
                {
                    _repoData.AddRange(data.Select(x => new MarketData{Date = x.Date, MarketPriceEX1 = x.MarketPriceEX1}));
                });

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
            await _service.ValidData(data);

            // Assert
            Assert.AreEqual(3, malformedCount);
        }
        
        [Test]
        public async Task EventIsCalledForValidRecords()
        {
            // Arrange
            var valid = 0;

            _service.AddOnValidRecord((sender, recordLine) => { valid++; });

            var data = new List<string>
            {
                $"{DateTime.Now}, 50",
                $"{DateTime.Now},",
                $"{DateTime.Now}, abc",
                $"{DateTime.Now}",
            };

            // Act
            await _service.ValidData(data);

            // Assert
            Assert.AreEqual(1, valid);
        }

        [Test]
        public async Task InsertSkipDuplicateRecords()
        {
            // Arrange
            _repoData.Add(new MarketData
            {
                Id = 1,
                Date = new DateTime(2020,8,16),
                MarketPriceEX1 = 10
            });
            _repoData.Add(new MarketData
            {
                Id = 1,
                Date = new DateTime(2020,8,15),
                MarketPriceEX1 = 11.123456789
            });

            var insertData = new List<InsertDataModel>
            {
                new InsertDataModel
                {
                    Date = new DateTime(2020, 8, 16),
                    MarketPriceEX1 = 10
                },
                new InsertDataModel
                {
                    Date = new DateTime(2020, 8, 15),
                    MarketPriceEX1 = 11.123456789
                },
                new InsertDataModel
                {
                    Date = new DateTime(2020, 8, 15),
                    MarketPriceEX1 = 11.1234
                },
            };
            
            // Act
            await _service.InsertMultiple(insertData);
            
            // Assert
            Assert.AreEqual(3, _repoData.Count);
        }
    }
}