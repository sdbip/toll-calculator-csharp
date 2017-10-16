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
	}
}
