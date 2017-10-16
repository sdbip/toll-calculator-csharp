﻿using System;
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

		[TestCase(1, 1)]   // New year's Day
		[TestCase(3, 28)]  // Maundy Thursday
		[TestCase(3, 29)]  // Good Friday
		[TestCase(4, 1)]   // April Fools
		[TestCase(4, 30)]  // King's Birthday
		[TestCase(5, 1)]   // Labour Day
		[TestCase(5, 8)]   // Eve of the Ascension
		[TestCase(5, 9)]   // Day of the Ascension
		[TestCase(6, 5)]   // National Day Eve
		[TestCase(6, 6)]   // National Day
		[TestCase(7, 9)]   // Some day in July
		[TestCase(11, 1)]  // Halloween
		[TestCase(12, 24)] // Christmas Eve
		[TestCase(12, 25)] // Christmas Day
		[TestCase(12, 26)] // Boxing Day
		[TestCase(12, 31)] // New Year's Eve
		public void tollFreeDates(int month, int day)
		{
			var earlyMorning = new DateTime(2013, month, day, 7, 5, 0);

			var fee = calculator.GetTollFee(new Car(), new DateTime[] { earlyMorning });

			Assert.AreEqual(0, fee);
		}
	}
}
