using System;

public class TollCalculator
{
	readonly CalendarDay day;

	public TollCalculator(CalendarDay day)
	{
		this.day = day;
	}

	public int GetTollFee(VehicleType vehicle, DateTime[] dates)
	{
		if (dates.Length == 0) return 0;
		if (vehicle.IsTollFree()) return 0;
		if (day.IsTollFree) return 0;

		int totalFee = 0;
		var startOfTheHour = dates[0];
		int feeForThisHour = 0;
		foreach (var date in dates)
		{
			int nextFee = new TimeOfDay(date.Hour, date.Minute).TollFee;
			if ((date - startOfTheHour).TotalHours < 1.0)
			{
				totalFee -= feeForThisHour;
				feeForThisHour = Math.Max(feeForThisHour, nextFee);
				totalFee += feeForThisHour;
			}
			else
			{
				totalFee += nextFee;
				feeForThisHour = nextFee;
				startOfTheHour = date;
			}
		}
		if (totalFee > 60) totalFee = 60;
		return totalFee;
	}
}

static class VehicleTypeExtension
{
	public static bool IsTollFree(this VehicleType self)
	{
		return self != VehicleType.Car;
	}
}
