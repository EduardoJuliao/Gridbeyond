using System.Collections.Generic;
using GridBeyond.Domain.Models;
using NUnit.Framework;
using GridBeyond.Domain.Extensions;
using System;
using System.Linq;

namespace GridBeyond.Domain.Tests.ExtensionTests
{
    [TestFixture]
    public class DataModelExtensionTests
    {
        [Test]
        public void FindLatestValueReturnHighestPeakInTheDay()
        {
            // Arrange
            var peakValue = 50;

            var data = new List<DataModel>
            {
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,30,0),
                    MarketPriceEX1 = 10
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,35,0),
                    MarketPriceEX1 = peakValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,40,0),
                    MarketPriceEX1 = peakValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,45,0),
                    MarketPriceEX1 = 10
                }
            }.OrderBy(x => x.Date);

            // Act
            var peaks = data.FindLatestValue(new DateTime(2020,8,27), peakValue);

            // Assert
            Assert.AreEqual(2, peaks.Count());
            Assert.IsTrue(peaks.SequenceEqual(new[] { new DateTime(2020, 8, 27, 13, 35, 0), new DateTime(2020, 8, 27, 13, 40, 0) }));
        }

        [Test]
        public void FindLatestValueReturnHighestLatestPeakInTheDay()
        {
            // Arrange
            var peakValue = 50;

            var data = new List<DataModel>
            {
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,30,0),
                    MarketPriceEX1 = peakValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,35,0),
                    MarketPriceEX1 = peakValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,40,0),
                    MarketPriceEX1 = 10
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,45,0),
                    MarketPriceEX1 = peakValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,50,0),
                    MarketPriceEX1 = peakValue
                }
            }.OrderBy(x => x.Date);

            // Act
            var peaks = data.FindLatestValue(new DateTime(2020, 8, 27), peakValue);

            // Assert
            Assert.AreEqual(2, peaks.Count());
            Assert.IsTrue(peaks.SequenceEqual(new[] { new DateTime(2020, 8, 27, 13, 45, 0), new DateTime(2020, 8, 27, 13, 50, 0) }));
        }

        [Test]
        public void FindLatestValueReturnLowestPeakInTheDay()
        {
            // Arrange
            var lowestValue = 5;

            var data = new List<DataModel>
            {
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,30,0),
                    MarketPriceEX1 = 10
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,35,0),
                    MarketPriceEX1 = lowestValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,40,0),
                    MarketPriceEX1 = lowestValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,45,0),
                    MarketPriceEX1 = 10
                }
            }.OrderBy(x => x.Date);

            // Act
            var lowest = data.FindLatestValue(new DateTime(2020, 8, 27), lowestValue);

            // Assert
            Assert.AreEqual(2, lowest.Count());
            Assert.IsTrue(lowest.SequenceEqual(new[] { new DateTime(2020, 8, 27, 13, 35, 0), new DateTime(2020, 8, 27, 13, 40, 0) }));
        }

        [Test]
        public void FindLatestValueReturnLowestLatestPeakInTheDay()
        {
            // Arrange
            var lowestValue = 5;

            var data = new List<DataModel>
            {
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,30,0),
                    MarketPriceEX1 = lowestValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,35,0),
                    MarketPriceEX1 = lowestValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,40,0),
                    MarketPriceEX1 = 10
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,45,0),
                    MarketPriceEX1 = lowestValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,50,0),
                    MarketPriceEX1 = lowestValue
                }
            }.OrderBy(x => x.Date);

            // Act
            var lowest = data.FindLatestValue(new DateTime(2020, 8, 27), lowestValue);

            // Assert
            Assert.AreEqual(2, lowest.Count());
            Assert.IsTrue(lowest.SequenceEqual(new[] { new DateTime(2020, 8, 27, 13, 45, 0), new DateTime(2020, 8, 27, 13, 50, 0) }));
        }

        [Test]
        public void FindValueReturnsAListOfPeakDatesInDay()
        {
            // Arrange
            var peakValue = 50;

            var data = new List<DataModel>
            {
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,30,0),
                    MarketPriceEX1 = peakValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,35,0),
                    MarketPriceEX1 = peakValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,40,0),
                    MarketPriceEX1 = 10
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,45,0),
                    MarketPriceEX1 = peakValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,50,0),
                    MarketPriceEX1 = peakValue
                }
            }.OrderBy(x => x.Date);

            // Act
            var peaks = data.FindValue(new DateTime(2020, 8, 27), peakValue);

            // Assert
            Assert.AreEqual(4, peaks.Count());
        }

        [Test]
        public void FindValueReturnsAListOfQuieterDatesInDay()
        {
            // Arrange
            var quietValue = 50;

            var data = new List<DataModel>
            {
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,30,0),
                    MarketPriceEX1 = quietValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,35,0),
                    MarketPriceEX1 = quietValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,40,0),
                    MarketPriceEX1 = 10
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,45,0),
                    MarketPriceEX1 = quietValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,50,0),
                    MarketPriceEX1 = quietValue
                }
            }.OrderBy(x => x.Date);

            // Act
            var quieter = data.FindValue(new DateTime(2020, 8, 27), quietValue);

            // Assert
            Assert.AreEqual(4, quieter.Count());
        }

        [Test]
        public void CanFindLatestPeaksFromUnorderedData()
        {
            // Arrange
            var peakValue = 50;

            var data = new List<DataModel>
            {
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,40,0),
                    MarketPriceEX1 = 10
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,45,0),
                    MarketPriceEX1 = peakValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,35,0),
                    MarketPriceEX1 = peakValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,50,0),
                    MarketPriceEX1 = peakValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,30,0),
                    MarketPriceEX1 = peakValue
                },
            };

            // Act
            var peaks = data.FindLatestPeakDuringDay(new DateTime(2020, 8, 27));

            // Assert
            Assert.AreEqual(2, peaks.Count());
            Assert.IsTrue(peaks.SequenceEqual(new[] { new DateTime(2020, 8, 27, 13, 45, 0), new DateTime(2020, 8, 27, 13, 50, 0) }));
        }

        [Test]
        public void CanFindLatestQuieterFromUnorderedData()
        {
            // Arrange
            var quieterValue = 5;

            var data = new List<DataModel>
            {
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,40,0),
                    MarketPriceEX1 = 10
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,45,0),
                    MarketPriceEX1 = quieterValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,35,0),
                    MarketPriceEX1 = quieterValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,50,0),
                    MarketPriceEX1 = quieterValue
                },
                new DataModel
                {
                    Date = new DateTime(2020, 8,27, 13,30,0),
                    MarketPriceEX1 = quieterValue
                },
            };

            // Act
            var quieter = data.FindLatestQuieterDuringDay(new DateTime(2020, 8, 27));

            // Assert
            Assert.AreEqual(2, quieter.Count());
            Assert.IsTrue(quieter.SequenceEqual(new[] { new DateTime(2020, 8, 27, 13, 45, 0), new DateTime(2020, 8, 27, 13, 50, 0) }));
        }
    }
}
