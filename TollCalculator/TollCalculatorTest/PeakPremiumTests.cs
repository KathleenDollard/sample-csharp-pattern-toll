using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TollEngine;

namespace TollCalculatorTest
{
    public class PeakPremiumTests
    {
        private TollCalculator _tollCalculator;

        [SetUp]
        public void Setup()
            => _tollCalculator = new TollCalculator();

        [Test]
        [TestCaseSource(typeof(TestDataProvider))]
        public decimal Peak_premium_correct(DateTime tollEventTime, bool inBound)
        {
            var toll = _tollCalculator.PeakTimePremium(tollEventTime, inBound);
            return toll;
        }

        public class TestDataProvider : IEnumerable
        {
            public IEnumerator GetEnumerator() 
                => new List<TestCaseData> {
                    new TestCaseData(new DateTime(2019, 3, 6, 8, 30, 0), true).Returns(2.00m).SetName("Inbound, Weekday morning rush"),
                    new TestCaseData(new DateTime(2019, 3, 7, 13, 30, 0), true).Returns(1.50m).SetName("Inbound, Weekday day"),
                    new TestCaseData(new DateTime(2019, 3, 14, 17, 30, 0), true).Returns(1.00m).SetName("Inbound, Weekday evening rush"),
                    new TestCaseData(new DateTime(2019, 3, 17, 22, 30, 0), true).Returns(0.75m).SetName("Inbound, Overnight"),
                    new TestCaseData(new DateTime(2019, 3, 17, 14, 30, 0), true).Returns(1.00m).SetName("Inbound, Weekend day"),
                    new TestCaseData(new DateTime(2019, 3, 17, 18, 05, 0), true).Returns(1.00m).SetName("Inbound, Weekend evening rush"),
                    new TestCaseData(new DateTime(2019, 3, 16, 01, 30, 0), true).Returns(0.75m).SetName("Inbound, Weekend overnight"),
                    new TestCaseData(new DateTime(2019, 3, 7, 8, 30, 0), false).Returns(1.00m).SetName("Weekday morning rush"),
                    new TestCaseData(new DateTime(2019, 3, 7, 13, 30, 0), false).Returns(1.50m).SetName("Weekday day"),
                    new TestCaseData(new DateTime(2019, 3, 14, 17, 15, 0), false).Returns(2.00m).SetName("Weekday evening rush"),
                    new TestCaseData(new DateTime(2019, 3, 17, 22, 30, 0), false).Returns(0.75m).SetName("Overnight"),
                    new TestCaseData(new DateTime(2019, 3, 17, 14, 30, 0), false).Returns(1.00m).SetName("Weekend day"),
                    new TestCaseData(new DateTime(2019, 3, 17, 18, 05, 0), false).Returns(1.00m).SetName("Weekend evening rush"),
                    new TestCaseData(new DateTime(2019, 3, 17, 01, 30, 0), false).Returns(0.75m).SetName("Weekend overnight"),
                }.GetEnumerator();
        }


        //private static IEnumerable<TestCaseData> GetTimes
        //{
        //    get
        //    {
        //        yield return new TestCaseData(new DateTime(2019, 3, 6, 8, 30, 0), true).Returns(2.00m).SetName("Inbound, Weekday morning rush");
        //        yield return new TestCaseData(new DateTime(2019, 3, 7, 13, 30, 0), true).Returns(1.50m).SetName("Inbound, Weekday day");
        //        yield return new TestCaseData(new DateTime(2019, 3, 14, 17, 15, 0), true).Returns(7.00m).SetName("Inbound, Weekday evening rush");
        //        yield return new TestCaseData(new DateTime(2019, 3, 17, 22, 30, 0), true).Returns(0.75m).SetName("Inbound, Overnight");
        //        yield return new TestCaseData(new DateTime(2019, 3, 17, 14, 30, 0), true).Returns(1.00m).SetName("Inbound, Weekend day");
        //        yield return new TestCaseData(new DateTime(2019, 3, 17, 18, 05, 0), true).Returns(1.00m).SetName("Inbound, Weekend evening rush");
        //        yield return new TestCaseData(new DateTime(2019, 3, 16, 01, 30, 0), true).Returns(0.75m).SetName("Inbound, Weekend overnight");
        //        yield return new TestCaseData(new DateTime(2019, 3, 7, 8, 30, 0), false).Returns(1.00m).SetName("Weekday morning rush");
        //        yield return new TestCaseData(new DateTime(2019, 3, 7, 13, 30, 0), false).Returns(1.50m).SetName("Weekday day");
        //        yield return new TestCaseData(new DateTime(2019, 3, 14, 17, 15, 0), false).Returns(2.00m).SetName("Weekday evening rush");
        //        yield return new TestCaseData(new DateTime(2019, 3, 17, 22, 30, 0), false).Returns(0.75m).SetName("Overnight");
        //        yield return new TestCaseData(new DateTime(2019, 3, 17, 14, 30, 0), false).Returns(1.00m).SetName("Weekend day");
        //        yield return new TestCaseData(new DateTime(2019, 3, 17, 18, 05, 0), false).Returns(1.00m).SetName("Weekend evening rush");
        //        yield return new TestCaseData(new DateTime(2019, 3, 17, 01, 30, 0), false).Returns(0.75m).SetName("Weekend overnight");
        //    }
        //}
    }
}
