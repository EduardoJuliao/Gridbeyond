using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GridBeyond.Domain.Entities;
using GridBeyond.Domain.Interfaces.Repository;
using GridBeyond.Domain.Interfaces.Services;
using GridBeyond.Domain.Models;
using GridBeyond.Domain.Repository;
using GridBeyond.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace GridBeyond.Domain.Tests
{
    public class MarketDataServiceTests
    {
        private IMarketDataService _service;
        private IMarketDataRepository _repo;

        private MarketContext _inMemoryContext;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<MarketContext>()
                .UseInMemoryDatabase(databaseName: Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8))
                .Options;

            _inMemoryContext = new MarketContext(options);
            _repo = new MarketDataRepository(_inMemoryContext);

            var mockCache = new Moq.Mock<ICacheRepository>();

            _service = new MarketDataService(_repo, mockCache.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _inMemoryContext.Dispose();
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
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 16),
                MarketPriceEX1 = 10
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 15),
                MarketPriceEX1 = 11.123456789
            });

            _inMemoryContext.SaveChanges();

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
            Assert.AreEqual(3, _inMemoryContext.MarketDatas.Count());
        }

        [Test]
        public async Task CanGenerateCorrectReportData()
        {
            // Arrange
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 16, 11, 30, 0),
                MarketPriceEX1 = 10
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 16, 12, 0, 0),
                MarketPriceEX1 = 20
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 15),
                MarketPriceEX1 = 11.123456789
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 14),
                MarketPriceEX1 = 9
            });

            _inMemoryContext.SaveChanges();

            // Act
            var report = await _service.GetReport();

            // Assert
            Assert.AreEqual(9, report.LowestValue);
            Assert.AreEqual(20, report.HighestValue);
            Assert.AreEqual(new DateTime(2020, 8, 16), report.HighestValueDate);
            Assert.AreEqual(new DateTime(2020, 8, 14), report.LowestValueDate);
        }

        [Test]
        public async Task ReportWillShowPeakHours()
        {
            // Arrange
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 14, 11, 30, 0),
                MarketPriceEX1 = 10
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 15, 12, 0, 0),
                MarketPriceEX1 = 20
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 15, 12,30,0),
                MarketPriceEX1 = 20
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 15, 13, 0, 0),
                MarketPriceEX1 = 10
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 15, 13, 30, 0),
                MarketPriceEX1 = 20
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 15, 14, 0, 0),
                MarketPriceEX1 = 20
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 15, 14, 30, 0),
                MarketPriceEX1 = 20
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 16),
                MarketPriceEX1 = 20
            });

            _inMemoryContext.SaveChanges();

            // Act
            var report = await _service.GetReport();

            // Assert
            Assert.AreEqual(3, report.PeakQuietPerDate.Count);
            Assert.AreEqual(1, report.PeakQuietPerDate[0].PeakHours.Count());
            Assert.AreEqual(3, report.PeakQuietPerDate[1].PeakHours.Count());
            Assert.AreEqual(1, report.PeakQuietPerDate[2].PeakHours.Count());
            var highestPeakTimes = new[] { new DateTime(2020, 8, 15, 13, 30, 0), new DateTime(2020, 8, 15, 14, 0, 0), new DateTime(2020, 8, 15, 14, 30, 0) };
            Assert.IsTrue(highestPeakTimes.SequenceEqual(report.HighestPeak.PeakHours));
        }

        [Test]
        public async Task ReportWillShowQuieterHours()
        {
            // Arrange
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 14, 11, 30, 0),
                MarketPriceEX1 = 10
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 15, 12, 0, 0),
                MarketPriceEX1 = 20
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 15, 12, 30, 0),
                MarketPriceEX1 = 20
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 15, 13, 0, 0),
                MarketPriceEX1 = 10
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 15, 13, 30, 0),
                MarketPriceEX1 = 9
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 15, 14, 0, 0),
                MarketPriceEX1 = 9
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 15, 14, 30, 0),
                MarketPriceEX1 = 9
            });
            _inMemoryContext.MarketDatas.Add(new MarketData
            {
                Date = new DateTime(2020, 8, 16),
                MarketPriceEX1 = 20
            });

            _inMemoryContext.SaveChanges();

            // Act
            var report = await _service.GetReport();

            // Assert
            Assert.AreEqual(3, report.PeakQuietPerDate.Count);
            Assert.AreEqual(3, report.PeakQuietPerDate[1].QuietHours.Count());
            var QuieterTimes = new[] { new DateTime(2020, 8, 15, 13, 30, 0), new DateTime(2020, 8, 15, 14, 0, 0), new DateTime(2020, 8, 15, 14, 30, 0) };
            Assert.IsTrue(QuieterTimes.SequenceEqual(report.HighestPeak.QuietHours));
        }
    }
}