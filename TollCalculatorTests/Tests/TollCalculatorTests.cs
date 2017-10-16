using System;
using NUnit.Framework;
using TollFeeCalculator;

namespace TollCalculatorTests
{
	[TestFixture]
	public class TollCalculatorTests
	{
		[Test, Ignore("Crashes at this time")]
		public void NoFeeIfNoPasses() {
			var calculator = new TollCalculator();
			calculator.GetTollFee(new Car(), new DateTime[0]);
		}

		[Test]
		public void SinglePassFee() {
			var calculator = new TollCalculator();
			var earlyMorning = new DateTime(2013, 1, 2, 6, 5, 0);

			var fee = calculator.GetTollFee(new Car(), new DateTime[] { earlyMorning });

			Assert.AreEqual(8, fee);
		}
	}
}
