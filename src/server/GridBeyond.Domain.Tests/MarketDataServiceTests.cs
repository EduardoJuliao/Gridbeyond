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
    public class MarketDataServiceTests
    {
        private IMarketDataService _service;

        private List<MarketData> _repoData;

        [SetUp]
        public void SetUp()
        {
            _repoData = new List<MarketData>();
            
            var mock = new Moq.Mock<IMarketDataRepository>();

            mock.Setup(x => x.Get())
                .Returns(() => _repoData.Select(x => new DataModel
                {
                    Date = x.Date,
                    MarketPriceEX1 = x.MarketPriceEX1
                }).AsQueryable());

            mock.Setup(x => x.Exists(It.IsAny<Expression<Func<MarketData, bool>>>()))
                .Returns((Expression<Func<MarketData, bool>> expression) => _repoData.Any(expression.Compile()));

            mock.Setup(x => x.Insert(It.IsAny<InsertDataModel>()))
                .Callback((InsertDataModel data) => _repoData.Add(new MarketData
                    {Date = data.Date, MarketPriceEX1 = data.MarketPriceEX1}));

            mock.Setup(x => x.Insert(It.IsAny<IEnumerable<InsertDataModel>>()))
                .Callback((IEnumerable<InsertDataModel> data) =>
                {
                    _repoData.AddRange(data.Select(x => new MarketData
                        {Date = x.Date, MarketPriceEX1 = x.MarketPriceEX1}));
                });

            _service = new MarketDataService(mock.Object);
        }

        [Test]
        public async Task ValidatorReturnIncorrectLines()
        {
            // Arrange
            var data = new List<string>
            {
                "17/08/2020, 50",
                "17/08/2020,",
                "17/08/2020, abc",
                "17/08/2020",
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
                "17/08/2020, 50",
                "17/08/2020,",
                "17/08/2020, abc",
                "17/08/2020",
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
                "17/08/2020, 50",
                "17/08/2020,",
                "17/08/2020, abc",
                "17/08/2020",
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
                Date = new DateTime(2020, 8, 16),
                MarketPriceEX1 = 10
            });
            _repoData.Add(new MarketData
            {
                Id = 1,
                Date = new DateTime(2020, 8, 15),
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

        //[Test]
        public void CanGenerateCorrectReportData()
        {
            // Arrange
            _repoData.Add(new MarketData
            {
                Id = 1,
                Date = new DateTime(2020, 8, 16, 11, 30, 0),
                MarketPriceEX1 = 10
            });
            _repoData.Add(new MarketData
            {
                Id = 1,
                Date = new DateTime(2020, 8, 16, 12, 0, 0),
                MarketPriceEX1 = 20
            });
            _repoData.Add(new MarketData
            {
                Id = 1,
                Date = new DateTime(2020, 8, 15),
                MarketPriceEX1 = 11.123456789
            });
            _repoData.Add(new MarketData
            {
                Id = 1,
                Date = new DateTime(2020, 8, 14),
                MarketPriceEX1 = 9
            });

            // Act
            var report = _service.GetReportDataHistory().GetAwaiter().GetResult();

            // Assert
            Assert.AreEqual(9, report.LowestValue);
            Assert.AreEqual(20, report.HighestValue);
            Assert.AreEqual(new DateTime(2020, 8, 16, 12, 0, 0), report.HighestValueDate);
            Assert.AreEqual(new DateTime(2020, 8, 14), report.LowestValueDate);
        }

        [Test]
        public void DuplicateDataWillBeRemoved()
        {
            // Arrange
            var listWithDuplicates = new List<string>
            {
                "A",
                "B",
                "C",
                "C",
                "D",
                "A"
            };

            // Act
            var uniqueList = _service.RemoveDuplicates(listWithDuplicates).ToList();

            // Assert
            Assert.AreEqual(4, uniqueList.Count());
            Assert.AreEqual("A", uniqueList[0]);
            Assert.AreEqual("B", uniqueList[1]);
            Assert.AreEqual("C", uniqueList[2]);
            Assert.AreEqual("D", uniqueList[3]);
        }
    }
}