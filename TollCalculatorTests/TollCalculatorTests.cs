using NUnit.Framework;
using TollCalculator;

namespace TollCalculatorTests
{
	[TestFixture]
	public class TollCalculatorTests
	{

		[Test]
		public void NoFeeIfNoPasses()
		{
			var day = new CalendarDay { Year = 2013, Month = Month.January, Day = 2 };
			var calculator = new TollCalculator.TollCalculator(day, VehicleType.Car);

			Assert.AreEqual(0, calculator.TotalFee);
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
			var day = new CalendarDay { Year = 2013, Month = Month.January, Day = 2 };
			var calculator = new TollCalculator.TollCalculator(day, VehicleType.Car);

			calculator.PassToll(new TimeOfDay(hour, minute));

			Assert.AreEqual(expectedFee, calculator.TotalFee);
		}

		[TestCase(Month.January, 1)]   // New year's Day
		[TestCase(Month.March, 28)]    // Maundy Thursday
		[TestCase(Month.March, 29)]    // Good Friday
		[TestCase(Month.April, 1)]     // April Fools
		[TestCase(Month.April, 30)]    // King's Birthday
		[TestCase(Month.May, 1)]       // Labour Day
		[TestCase(Month.May, 8)]       // Eve of the Ascension
		[TestCase(Month.May, 9)]       // Day of the Ascension
		[TestCase(Month.June, 5)]      // National Day Eve
		[TestCase(Month.June, 6)]      // National Day
		[TestCase(Month.June, 21)]     // Midsummer Eve
		[TestCase(Month.July, 9)]      // Some day in July
		[TestCase(Month.November, 1)]  // Halloween
		[TestCase(Month.December, 24)] // Christmas Eve
		[TestCase(Month.December, 25)] // Christmas Day
		[TestCase(Month.December, 26)] // Boxing Day
		[TestCase(Month.December, 31)] // New Year's Eve
		public void TollFreeDates2013(Month month, int day)
		{
			var calendarDay = new CalendarDay(2013, month, day);
			var calculator = new TollCalculator.TollCalculator(calendarDay, VehicleType.Car);

			calculator.PassToll(new TimeOfDay { Hour = 7, Minute = 5 });

			Assert.AreEqual(0, calculator.TotalFee);
		}

		[TestCase(Month.January, 1)]   // New year's Day
		[TestCase(Month.April, 1)]     // April Fools
		[TestCase(Month.April, 13)]    // Maundy Thursday
		[TestCase(Month.April, 14)]    // Good Friday
		[TestCase(Month.April, 30)]    // King's Birthday
		[TestCase(Month.May, 1)]       // Labour Day
		[TestCase(Month.May, 24)]      // Eve of the Ascension
		[TestCase(Month.May, 25)]      // Day of the Ascension
		[TestCase(Month.June, 5)]      // National Day Eve
		[TestCase(Month.June, 6)]      // National Day
		[TestCase(Month.June, 23)]     // Midsummer Eve
		[TestCase(Month.July, 9)]      // Some day in July
		[TestCase(Month.November, 1)]  // Halloween
		[TestCase(Month.December, 24)] // Christmas Eve
		[TestCase(Month.December, 25)] // Christmas Day
		[TestCase(Month.December, 26)] // Boxing Day
		[TestCase(Month.December, 31)] // New Year's Eve
		public void TollFreeDates2017(Month month, int day)
		{
			var calendarDay = new CalendarDay(2017, month, day);
			var calculator = new TollCalculator.TollCalculator(calendarDay, VehicleType.Car);

			calculator.PassToll(new TimeOfDay { Hour = 7, Minute = 5 });

			Assert.AreEqual(0, calculator.TotalFee);
		}

		[TestCase(Month.January, 1)]   // New year's Day
		[TestCase(Month.April, 1)]     // April Fools
		[TestCase(Month.April, 1)]     // Maundy Thursday
		[TestCase(Month.April, 2)]     // Good Friday
		[TestCase(Month.April, 30)]    // King's Birthday
		[TestCase(Month.May, 1)]       // Labour Day
		[TestCase(Month.May, 12)]      // Eve of the Ascension
		[TestCase(Month.May, 13)]      // Day of the Ascension
		[TestCase(Month.June, 5)]      // National Day Eve
		[TestCase(Month.June, 6)]      // National Day
		[TestCase(Month.June, 25)]     // Midsummer Eve
		[TestCase(Month.July, 9)]      // Some day in July
		[TestCase(Month.November, 1)]  // Halloween
		[TestCase(Month.December, 24)] // Christmas Eve
		[TestCase(Month.December, 25)] // Christmas Day
		[TestCase(Month.December, 26)] // Boxing Day
		[TestCase(Month.December, 31)] // New Year's Eve
		public void TollFreeDates2021(Month month, int day)
		{
			var calendarDay = new CalendarDay(2021, month, day);
			var calculator = new TollCalculator.TollCalculator(calendarDay, VehicleType.Car);

			calculator.PassToll(new TimeOfDay { Hour = 7, Minute = 5 });

			Assert.AreEqual(0, calculator.TotalFee);
		}

		[Test]
		public void TollFreeForMotorcycles()
		{
			var day = new CalendarDay { Year = 2013, Month = Month.January, Day = 2 };
			var calculator = new TollCalculator.TollCalculator(day, VehicleType.Motorbike);

			calculator.PassToll(new TimeOfDay { Hour = 7, Minute = 5 });

			Assert.AreEqual(0, calculator.TotalFee);
		}

		[Test]
		public void MultiplePassesInOneHourCostsTheHighestFeeOnly()
		{
			var day = new CalendarDay { Year = 2013, Month = Month.January, Day = 2 };
			var calculator = new TollCalculator.TollCalculator(day, VehicleType.Car);

			calculator.PassToll(new TimeOfDay { Hour = 6, Minute = 15 }); //  8 SEK
			calculator.PassToll(new TimeOfDay { Hour = 7, Minute = 5 });  // 18 SEK

			Assert.AreEqual(18, calculator.TotalFee);
		}

		[Test]
		public void NewPassAfterOneHourIsAddedToTheTotal()
		{
			var day = new CalendarDay { Year = 2013, Month = Month.January, Day = 2 };
			var calculator = new TollCalculator.TollCalculator(day, VehicleType.Car);

			calculator.PassToll(new TimeOfDay { Hour = 6, Minute = 15 }); //  8 SEK
			calculator.PassToll(new TimeOfDay { Hour = 15, Minute = 5 }); // 13 SEK

			Assert.AreEqual(21, calculator.TotalFee);
		}

		[Test]
		public void NeverExceeds60InTotal()
		{
			var day = new CalendarDay { Year = 2013, Month = Month.January, Day = 2 };

			var calculator = new TollCalculator.TollCalculator(day, VehicleType.Car);
			calculator.PassToll(new TimeOfDay { Hour =  6, Minute = 0 }); //  8 SEK
			calculator.PassToll(new TimeOfDay { Hour =  7, Minute = 1 }); // 18 SEK
			calculator.PassToll(new TimeOfDay { Hour =  8, Minute = 2 }); // 13 SEK
			calculator.PassToll(new TimeOfDay { Hour =  9, Minute = 3 }); //  8 SEK
			calculator.PassToll(new TimeOfDay { Hour = 10, Minute = 4 }); //  8 SEK
			calculator.PassToll(new TimeOfDay { Hour = 11, Minute = 5 }); //  8 SEK

			// 8+18+13+8+8+8 == 63
			Assert.AreEqual(60, calculator.TotalFee);
		}
	}
}
