using System.Collections.Generic;
using NUnit.Framework;
using GridBeyond.Domain.Extensions;
using System.Linq;

namespace GridBeyond.Domain.Tests.ExtensionTests
{
    [TestFixture]
    public class IEnumerableExtenstionTests
    {

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
            var uniqueList = listWithDuplicates.RemoveDuplicates().ToList();

            // Assert
            Assert.AreEqual(4, uniqueList.Count());
            Assert.AreEqual("A", uniqueList[0]);
            Assert.AreEqual("B", uniqueList[1]);
            Assert.AreEqual("C", uniqueList[2]);
            Assert.AreEqual("D", uniqueList[3]);
        }

        [Test]
        public void GroupWhileReturnsSequenceOfElements()
        {
            // Arrange
            var sequenceNumbers = new List<int>
            {
                1,
                1,
                2,
                3,
                2,
                3
            };

            // Act
            var uniqueList = sequenceNumbers.GroupWhile((x, y) => y - x == 1)
                .Select(x => new { Count = x.Count(), Elements = x })
                .ToList();

            // Assert
            Assert.AreEqual(3, uniqueList.Count());

            Assert.AreEqual(1, uniqueList[0].Count);
            Assert.IsTrue(uniqueList[0].Elements.ToList()[0] == 1);

            Assert.AreEqual(3, uniqueList[1].Count);
            Assert.IsTrue(uniqueList[1].Elements.SequenceEqual(new [] { 1, 2, 3 }));

            Assert.AreEqual(2, uniqueList[2].Count);
            Assert.IsTrue(uniqueList[2].Elements.SequenceEqual(new[] { 2, 3 }));
        }
    }
}
