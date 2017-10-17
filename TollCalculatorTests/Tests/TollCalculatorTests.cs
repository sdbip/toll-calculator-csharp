using System;
using NUnit.Framework;

namespace TollCalculatorTests
{
	[TestFixture]
	public class TollCalculatorTests
	{
		readonly TollCalculator calculator = new TollCalculator();

		[Test]
		public void NoFeeIfNoPasses()
		{
			calculator.GetTollFee(VehicleType.Car, new DateTime[0]);
		}

		[TestCase(6, 5, 8)]
		[TestCase(6, 35, 13)]
		[TestCase(7, 5, 18)]
		[TestCase(8, 5, 13)]
		[TestCase(8, 35, 8)]
		[TestCase(12, 5, 8)]
		[TestCase(15, 5, 13)]
		[TestCase(16, 5, 18)]
		[TestCase(17, 5, 13)]
		[TestCase(18, 5, 8)]
		[TestCase(18, 30, 0)]
		public void SinglePassFee(int hour, int minute, int expectedFee)
		{
			var time = new DateTime(2013, 1, 2, hour, minute, 0);

			var fee = calculator.GetTollFee(VehicleType.Car, new DateTime[] { time });

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
		[TestCase(6, 21)]  // Midsummer Eve
		[TestCase(7, 9)]   // Some day in July
		[TestCase(11, 1)]  // Halloween
		[TestCase(12, 24)] // Christmas Eve
		[TestCase(12, 25)] // Christmas Day
		[TestCase(12, 26)] // Boxing Day
		[TestCase(12, 31)] // New Year's Eve
		public void TollFreeDates2013(int month, int day)
		{
			var time = new DateTime(2013, month, day, 7, 5, 0);

			var fee = calculator.GetTollFee(VehicleType.Car, new DateTime[] { time });

			Assert.AreEqual(0, fee);
		}

		[TestCase(1, 1)]   // New year's Day
		[TestCase(4, 1)]   // April Fools
		[TestCase(4, 13)]  // Maundy Thursday
		[TestCase(4, 14)]  // Good Friday
		[TestCase(4, 30)]  // King's Birthday
		[TestCase(5, 1)]   // Labour Day
		[TestCase(5, 24)]  // Eve of the Ascension
		[TestCase(5, 25)]  // Day of the Ascension
		[TestCase(6, 5)]   // National Day Eve
		[TestCase(6, 6)]   // National Day
		[TestCase(6, 23)]  // Midsummer Eve
		[TestCase(7, 9)]   // Some day in July
		[TestCase(11, 1)]  // Halloween
		[TestCase(12, 24)] // Christmas Eve
		[TestCase(12, 25)] // Christmas Day
		[TestCase(12, 26)] // Boxing Day
		[TestCase(12, 31)] // New Year's Eve
		public void TollFreeDates2017(int month, int day)
		{
			var time = new DateTime(2017, month, day, 7, 5, 0);

			var fee = calculator.GetTollFee(VehicleType.Car, new DateTime[] { time });

			Assert.AreEqual(0, fee);
		}

		[TestCase(1, 1)]   // New year's Day
		[TestCase(4, 1)]   // April Fools
		[TestCase(4, 1)]   // Maundy Thursday
		[TestCase(4, 2)]   // Good Friday
		[TestCase(4, 30)]  // King's Birthday
		[TestCase(5, 1)]   // Labour Day
		[TestCase(5, 12)]  // Eve of the Ascension
		[TestCase(5, 13)]  // Day of the Ascension
		[TestCase(6, 5)]   // National Day Eve
		[TestCase(6, 6)]   // National Day
		[TestCase(6, 25)]  // Midsummer Eve
		[TestCase(7, 9)]   // Some day in July
		[TestCase(11, 1)]  // Halloween
		[TestCase(12, 24)] // Christmas Eve
		[TestCase(12, 25)] // Christmas Day
		[TestCase(12, 26)] // Boxing Day
		[TestCase(12, 31)] // New Year's Eve
		public void TollFreeDates2021(int month, int day)
		{
			var time = new DateTime(2021, month, day, 7, 5, 0);

			var fee = calculator.GetTollFee(VehicleType.Car, new DateTime[] { time });

			Assert.AreEqual(0, fee);
		}

		[Test]
		public void TollFreeForMotorcycles()
		{
			var time = new DateTime(2013, 1, 2, 7, 5, 0);

			var fee = calculator.GetTollFee(VehicleType.Motorbike, new DateTime[] { time });

			Assert.AreEqual(0, fee);
		}

		[Test]
		public void MultiplePassesInOneHourCostsTheHighestFeeOnly()
		{
			var offToWork = new DateTime(2013, 1, 2, 6, 15, 0); // 8
			var goingHome = new DateTime(2013, 1, 2, 7, 5, 0);  // 18

			var fee = calculator.GetTollFee(VehicleType.Car, new DateTime[] { offToWork, goingHome });

			Assert.AreEqual(18, fee);
		}

		[Test]
		public void NewPassAfterOneHourIsAddedToTheTotal()
		{
			var offToWork = new DateTime(2013, 1, 2, 6, 15, 0); // 8
			var goingHome = new DateTime(2013, 1, 2, 15, 5, 0); // 13

			var fee = calculator.GetTollFee(VehicleType.Car, new DateTime[] { offToWork, goingHome });

			Assert.AreEqual(21, fee);
		}

		[Test]
		public void NeverExceeds60InTotal()
		{
			var dates = new DateTime[] {
				new DateTime(2013, 1, 2,  6, 0, 0),  //  8
				new DateTime(2013, 1, 2,  7, 1, 0),  // 18
				new DateTime(2013, 1, 2,  8, 2, 0),  // 13
				new DateTime(2013, 1, 2,  9, 3, 0),  //  8
				new DateTime(2013, 1, 2, 10, 4, 0),  //  8
				new DateTime(2013, 1, 2, 11, 5, 0),  //  8
			};

			var fee = calculator.GetTollFee(VehicleType.Car, dates);

			// 8+18+13+8+8+8 == 63
			Assert.AreEqual(60, fee);
		}
	}
}
