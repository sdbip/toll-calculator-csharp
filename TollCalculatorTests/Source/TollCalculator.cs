using System;

public class TollCalculator
{
	readonly CalendarDay day;

	int totalFee;
	TimeOfDay? startOfTheHour;
	int feeForThisHour;

	public TollCalculator(CalendarDay day)
	{
		this.day = day;
	}

	public int GetTollFee(VehicleType vehicle, DateTime[] dates)
	{
		if (dates.Length == 0) return 0;
		if (vehicle.IsTollFree()) return 0;
		if (day.IsTollFree) return 0;

		foreach (var date in dates)
		{
			var time = new TimeOfDay(date.Hour, date.Minute);
			int nextFee = time.TollFee;
			if (startOfTheHour == null) startOfTheHour = time;

			if (time.hour - startOfTheHour.Value.hour == 0 ||
			    time.hour - startOfTheHour.Value.hour == 1 && time.minute < startOfTheHour.Value.minute)
			{
				totalFee -= feeForThisHour;
				feeForThisHour = Math.Max(feeForThisHour, nextFee);
				totalFee += feeForThisHour;
			}
			else
			{
				totalFee += nextFee;
				feeForThisHour = nextFee;
				startOfTheHour = time;
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
