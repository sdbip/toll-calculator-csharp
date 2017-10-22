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
			var day = new CalendarDay { year = 2013, month = 1, day = 2 };
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
			var day = new CalendarDay { year = 2013, month = 1, day = 2 };
			var calculator = new TollCalculator.TollCalculator(day, VehicleType.Car);

			calculator.PassToll(new TimeOfDay(hour, minute));

			Assert.AreEqual(expectedFee, calculator.TotalFee);
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
			var calendarDay = new CalendarDay(2013, month, day);
			var calculator = new TollCalculator.TollCalculator(calendarDay, VehicleType.Car);

			calculator.PassToll(new TimeOfDay { hour = 7, minute = 5 });

			Assert.AreEqual(0, calculator.TotalFee);
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
			var calendarDay = new CalendarDay(2017, month, day);
			var calculator = new TollCalculator.TollCalculator(calendarDay, VehicleType.Car);

			calculator.PassToll(new TimeOfDay { hour = 7, minute = 5 });

			Assert.AreEqual(0, calculator.TotalFee);
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
			var calendarDay = new CalendarDay(2021, month, day);
			var calculator = new TollCalculator.TollCalculator(calendarDay, VehicleType.Car);

			calculator.PassToll(new TimeOfDay { hour = 7, minute = 5 });

			Assert.AreEqual(0, calculator.TotalFee);
		}

		[Test]
		public void TollFreeForMotorcycles()
		{
			var day = new CalendarDay { year = 2013, month = 1, day = 2 };
			var calculator = new TollCalculator.TollCalculator(day, VehicleType.Motorbike);

			calculator.PassToll(new TimeOfDay { hour = 7, minute = 5 });

			Assert.AreEqual(0, calculator.TotalFee);
		}

		[Test]
		public void MultiplePassesInOneHourCostsTheHighestFeeOnly()
		{
			var day = new CalendarDay { year = 2013, month = 1, day = 2 };
			var calculator = new TollCalculator.TollCalculator(day, VehicleType.Car);

			calculator.PassToll(new TimeOfDay { hour = 6, minute = 15 }); //  8 SEK
			calculator.PassToll(new TimeOfDay { hour = 7, minute = 5 });  // 18 SEK

			Assert.AreEqual(18, calculator.TotalFee);
		}

		[Test]
		public void NewPassAfterOneHourIsAddedToTheTotal()
		{
			var day = new CalendarDay { year = 2013, month = 1, day = 2 };
			var calculator = new TollCalculator.TollCalculator(day, VehicleType.Car);

			calculator.PassToll(new TimeOfDay { hour = 6, minute = 15 }); //  8 SEK
			calculator.PassToll(new TimeOfDay { hour = 15, minute = 5 }); // 13 SEK

			Assert.AreEqual(21, calculator.TotalFee);
		}

		[Test]
		public void NeverExceeds60InTotal()
		{
			var day = new CalendarDay { year = 2013, month = 1, day = 2 };

			var calculator = new TollCalculator.TollCalculator(day, VehicleType.Car);
			calculator.PassToll(new TimeOfDay { hour =  6, minute = 0 }); //  8 SEK
			calculator.PassToll(new TimeOfDay { hour =  7, minute = 1 }); // 18 SEK
			calculator.PassToll(new TimeOfDay { hour =  8, minute = 2 }); // 13 SEK
			calculator.PassToll(new TimeOfDay { hour =  9, minute = 3 }); //  8 SEK
			calculator.PassToll(new TimeOfDay { hour = 10, minute = 4 }); //  8 SEK
			calculator.PassToll(new TimeOfDay { hour = 11, minute = 5 }); //  8 SEK

			// 8+18+13+8+8+8 == 63
			Assert.AreEqual(60, calculator.TotalFee);
		}
	}
}
