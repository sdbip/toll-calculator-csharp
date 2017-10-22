using System;

namespace TollCalculator
{
	public struct CalendarDay
	{
		public int Year;
		public Month Month;
		public int Day;

		public CalendarDay(int year, Month month, int day)
		{
			Year = year;
			Month = month;
			Day = day;
		}

		public bool IsTollFree => IsWeekend || IsFreeForHoliday;

		private bool IsFreeForHoliday
		{
			get
			{
				if (Month == Month.January && Day == 1 ||
					Month == Month.April && (Day == 1 || Day == 30) ||
					Month == Month.May && Day == 1 ||
					Month == Month.June && (Day == 5 || Day == 6) ||
					Month == Month.July ||
					Month == Month.November && Day == 1 ||
					Month == Month.December && (Day == 24 || Day == 25 || Day == 26 || Day == 31))
				{
					return true;
				}

				if (IsMidsummerEve) return true;

				var easter = EasterDay(Year);
				if (Month == easter.Month && (Day == easter.Day - 2 || Day == easter.Day - 3))
					return true;

				var ascensionDay = AscensionDay(Year);
				return Month == ascensionDay.Month && (Day == ascensionDay.Day || Day == ascensionDay.Day - 1);
			}
		}

		private bool IsWeekend
		{
			get
			{
				var dayOfWeek = new DateTime(Year, (int) Month, Day).DayOfWeek;
				return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
			}
		}

		private static CalendarDay AscensionDay(int year)
		{
			// The Ascension of Christ occurs on Thursday 40 days after Easter Eve.
			var easterDay = EasterDay(year);
			return easterDay.AddDays(39);
		}

		private static CalendarDay EasterDay(int year)
		{
			// Easter occurs on the first Sunday after the first full moon
			// after the spring equinox.
			// Algorithm by Carl Friedrich Gauss
			// https://sv.wikipedia.org/wiki/Påskdagen

			var a = year % 19;
			var b = year % 4;
			var c = year % 7;
			var d = (19 * a + 24) % 30;
			var e = (2 * b + 4 * c + 6 * d + 5) % 7;

			if (d + e <= 9) return new CalendarDay(year, Month.March, 22 + d + e);
			var easterDay = d + e - 9;
			switch (easterDay)
			{
				case 26:
					return new CalendarDay(year, Month.April, 19);
				case 25 when d == 28 && e == 6:
					return new CalendarDay(year, Month.April, 18);
				default:
					return new CalendarDay(year, Month.April, easterDay);
			}
		}

		private bool IsMidsummerEve
		{
			get
			{
				// According to Wikipedia, Swedish midsummer eve is always
				// celebrated on the Friday that occurs between 19-25 of June.
				if (Month != Month.June || Day < 19 || Day > 25) return false;

				return new DateTime(Year, (int) Month, Day).DayOfWeek == DayOfWeek.Friday;
			}
		}

		public override string ToString()
		{
			return $"{Year}-{(int) Month}-{Day}";
		}

		// TODO: This is incomplete, but it is only used to calculate the day of the Ascension anyway.
		private static int DaysInMonth(Month month)
		{
			// ReSharper disable once SwitchStatementMissingSomeCases
			switch (month)
			{
				case Month.March:
				case Month.May:
					return 31;
				case Month.April:
					return 30;
				default:
					throw new ArgumentOutOfRangeException(nameof(month), month, null);
			}
		}

		// TODO: This is incomplete, but it is only used to calculate the day of the Ascension anyway.
		private CalendarDay AddDays(int addDays)
		{
			var result = this;
			result.Day += addDays;
			while (result.Day > DaysInMonth(result.Month))
			{
				result.Day -= DaysInMonth(result.Month);
				result.Month++;
			}
			return result;
		}
	}
}
