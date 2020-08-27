using System;
using NUnit.Framework;
using GridBeyond.Domain.Extensions;

namespace GridBeyond.Domain.Tests.ExtensionTests
{
    [TestFixture]
    public class CsvValueExtensionTests
    {
        [Test]
        public void IsValidReturnFalseForMalformedRecords()
        {
            // Arrange

            // Act

            // Assert
            Assert.IsFalse("17/08/2020,".IsValid(out DateTime _, out double _));
            Assert.IsFalse("17/08/2020, abc".IsValid(out DateTime _, out double _));
            Assert.IsFalse("17/08/2020".IsValid(out DateTime _, out double _));
        }

        [Test]
        public void IsValidReturnTrueForValidData()
        {
            // Arrange

            // Act

            // Assert
            Assert.IsTrue("17/08/2020, 50".IsValid(out DateTime _, out double _));
        }

        [TestCase("15/05/1993 20:14:10, 50")]
        [TestCase("15/05/1993 20:14, 50")]
        [TestCase("15/05/1993, 50")]
        public void ValidatorCanIdentifyDifferentTypesofDates(string value)
        {
            // Arrange

            // Act

            // Assert
            Assert.IsTrue(value.IsValid(out DateTime _, out double _));
        }
    }
}
