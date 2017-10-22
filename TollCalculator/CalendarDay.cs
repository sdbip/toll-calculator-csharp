using System;

namespace TollCalculator
{
	public struct CalendarDay
	{
		public int Year;
		public int Month;
		public int Day;

		public CalendarDay(int year, int month, int day)
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
				if (Month == 1 && Day == 1 ||
					Month == 4 && (Day == 1 || Day == 30) ||
					Month == 5 && Day == 1 ||
					Month == 6 && (Day == 5 || Day == 6) ||
					Month == 7 ||
					Month == 11 && Day == 1 ||
					Month == 12 && (Day == 24 || Day == 25 || Day == 26 || Day == 31))
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
				var dayOfWeek = new DateTime(Year, Month, Day).DayOfWeek;
				return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
			}
		}

		private static CalendarDay AscensionDay(int year)
		{
			// The Ascension of Christ occurs on Thursday 40 days after Easter Eve.
			var easterDay = EasterDay(year);
			var ascension = easterDay;
			ascension.Day += 39;

			// ReSharper disable once ConvertIfStatementToSwitchStatement
			if (ascension.Month == 3 && ascension.Day > 31)
			{
				ascension.Day -= 31;
				ascension.Month = 4;
			}

			// ReSharper disable once InvertIf
			if (ascension.Month == 4 && ascension.Day > 30)
			{
				ascension.Day -= 30;
				ascension.Month = 5;
			}

			return ascension;
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

			if (d + e > 9)
			{
				var easterDay = d + e - 9;
				// ReSharper disable once SwitchStatementMissingSomeCases
				switch (easterDay)
				{
					case 26:
						easterDay = 19;
						break;
					case 25 when d == 28 && e == 6:
						easterDay = 18;
						break;
				}
				return new CalendarDay(year, 4, easterDay);
			}
			else
			{
				return new CalendarDay(year, 3, 22 + d + e);
			}
		}

		private bool IsMidsummerEve
		{
			get
			{
				// According to Wikipedia, Swedish midsummer eve is always
				// celebrated on the Friday that occurs between 19-25 of June.
				if (Month != 6 || Day < 19 || Day > 25)
				{
					return false;
				}

				return new DateTime(Year, Month, Day).DayOfWeek == DayOfWeek.Friday;
			}
		}

		public override string ToString()
		{
			return $"{Year}-{Month}-{Day}";
		}
	}
}
