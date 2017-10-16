using System;

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
		get
		{
			if (IsWeekend) return true;

			if (year == 2013)
			{
				if (month == 1 && day == 1 ||
					month == 3 && (day == 28 || day == 29) ||
					month == 4 && (day == 1 || day == 30) ||
					month == 5 && (day == 1 || day == 8 || day == 9) ||
					month == 6 && (day == 5 || day == 6 || day == 21) ||
					month == 7 ||
					month == 11 && day == 1 ||
					month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
				{
					return true;
				}
			}
			else
			{
				if (month == 1 && day == 1 ||
					month == 4 && (day == 13 || day == 14) ||
					month == 4 && (day == 1 || day == 30) ||
					month == 5 && (day == 1 || day == 24 || day == 25) ||
				    month == 6 && (day == 5 || day == 6 || day == 21) ||
				    month == 7 ||
				    month == 11 && day == 1 ||
				    month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
				{
					return true;
				}

			}
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
}
