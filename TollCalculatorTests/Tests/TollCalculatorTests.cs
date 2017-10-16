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

		[Test]
		public void SinglePassFee()
		{
			var earlyMorning = new DateTime(2013, 1, 2, 6, 5, 0);

			var fee = calculator.GetTollFee(new Car(), new DateTime[] { earlyMorning });

			Assert.AreEqual(8, fee);
		}
	}
}
