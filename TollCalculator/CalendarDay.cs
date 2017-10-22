using System;

namespace TollCalculator
{
	public struct CalendarDay
	{
		public int year;
		public int month;
		public int day;

		public CalendarDay(int year, int month, int day)
		{
			this.year = year;
			this.month = month;
			this.day = day;
		}

		public bool IsTollFree
		{
			get { return IsWeekend || IsFreeForHoliday; }
		}

		bool IsFreeForHoliday
		{
			get
			{
				if (month == 1 && day == 1 ||
					month == 4 && (day == 1 || day == 30) ||
					month == 5 && day == 1 ||
					month == 6 && (day == 5 || day == 6) ||
					month == 7 ||
					month == 11 && day == 1 ||
					month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
				{
					return true;
				}

				if (IsMidsummerEve) return true;

				var easter = EasterDay(year);
				if (month == easter.month && (day == easter.day - 2 || day == easter.day - 3))
					return true;

				var ascensionDay = AscensionDay(year);
				if (month == ascensionDay.month && (day == ascensionDay.day || day == ascensionDay.day - 1))
					return true;

				return false;
			}
		}

		bool IsWeekend
		{
			get
			{
				var dayOfWeek = new DateTime(year, month, day).DayOfWeek;
				return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
			}
		}

		static CalendarDay AscensionDay(int year)
		{
			// The Ascension of Christ occurs on Thursday 40 days after Easter Eve.
			var easterDay = EasterDay(year);
			var ascension = easterDay;
			ascension.day += 39;

			if (ascension.month == 3 && ascension.day > 31)
			{
				ascension.day -= 31;
				ascension.month = 4;
			}

			if (ascension.month == 4 && ascension.day > 30)
			{
				ascension.day -= 30;
				ascension.month = 5;
			}

			return ascension;
		}

		static CalendarDay EasterDay(int year)
		{
			// Easter occurs on the first Sunday after the first full moon
			// after the spring equinox.
			// Algorithm by Carl Friedrich Gauss
			// https://sv.wikipedia.org/wiki/Påskdagen

			int a = year % 19;
			int b = year % 4;
			int c = year % 7;
			int d = (19 * a + 24) % 30;
			int e = (2 * b + 4 * c + 6 * d + 5) % 7;

			if (d + e > 9)
			{
				int easterDay = d + e - 9;
				if (easterDay == 26) easterDay = 19;
				if (easterDay == 25 && d == 28 && e == 6) easterDay = 18;
				return new CalendarDay(year, 4, easterDay);
			}
			else
			{
				return new CalendarDay(year, 3, 22 + d + e);
			}
		}

		bool IsMidsummerEve
		{
			get
			{
				// According to Wikipedia, Swedish midsummer eve is always
				// celebrated on the Friday that occurs between 19-25 of June.
				if (month != 6 || day < 19 || day > 25)
				{
					return false;
				}

				return new DateTime(year, month, day).DayOfWeek == DayOfWeek.Friday;
			}
		}

		public override string ToString()
		{
			return $"{year}-{month}-{day}";
		}
	}
}
