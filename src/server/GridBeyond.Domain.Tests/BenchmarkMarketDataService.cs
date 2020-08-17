using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using GridBeyond.Domain.Entities;
using GridBeyond.Domain.Interfaces.Repository;
using GridBeyond.Domain.Interfaces.Services;
using GridBeyond.Domain.Models;
using GridBeyond.Domain.Services;
using NUnit.Framework;

namespace GridBeyond.Domain.Tests
{
    [TestFixture]
    public class BenchmarkMarketDataService
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

            _service = new MarketDataService(mock.Object);
        }
        
        private static TimeSpan Time(Action toTime)
        {
            var timer = Stopwatch.StartNew();
            toTime();
            timer.Stop();
            return timer.Elapsed;
        }
        
        [Test]
        public void CanGetSmallReportDataIn0Seconds()
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
            var time = Time(async () =>
            {
                await _service.GetReportDataHistory();
            });

            // Assert
            Assert.That(time, Is.LessThanOrEqualTo(TimeSpan.FromSeconds(0.5f)));
        }
        
        [Test]
        public void CanGetReportDataIn0Seconds()
        {
            // Arrange
            var random = new Random();
            _repoData.AddRange(Enumerable.Range(0,10000).Select(x => new MarketData
            {
                Id = _repoData.Count + 1,
                Date = DateTime.Now,
                MarketPriceEX1 = random.NextDouble()
            }));
            // Act
            var time = Time(async () =>
            {
                await _service.GetReportDataHistory();
            });

            // Assert
            Assert.That(time, Is.LessThanOrEqualTo(TimeSpan.FromSeconds(0.5f)));
        }
    }
}