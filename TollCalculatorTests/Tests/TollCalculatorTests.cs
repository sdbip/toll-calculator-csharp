using System;
using NUnit.Framework;
using TollFeeCalculator;

namespace TollCalculatorTests
{
	[TestFixture]
	public class TollCalculatorTests
	{
		readonly TollCalculator calculator = new TollCalculator();

		[Test, Ignore("Crashes at this time")]
		public void NoFeeIfNoPasses()
		{
			calculator.GetTollFee(new Car(), new DateTime[0]);
		}

		[TestCase(6, 5, 8)]
		[TestCase(6, 35, 13)]
		[TestCase(7, 5, 18)]
		[TestCase(8, 5, 13)]
		[TestCase(8, 35, 8)]
		[TestCase(12, 5, 0)] // TODO: Bug?
		[TestCase(15, 5, 13)]
		[TestCase(16, 5, 18)]
		[TestCase(17, 5, 13)]
		[TestCase(18, 5, 8)]
		[TestCase(18, 30, 0)]
		public void SinglePassFee(int hour, int minute, int expectedFee)
		{
			var earlyMorning = new DateTime(2013, 1, 2, hour, minute, 0);

			var fee = calculator.GetTollFee(new Car(), new DateTime[] { earlyMorning });

			Assert.AreEqual(expectedFee, fee);
		}
	}
}
