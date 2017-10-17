using System;

public class TollCalculator
{
	public int GetTollFee(VehicleType vehicle, DateTime[] dates)
	{
		DateTime intervalStart = dates[0];
		int totalFee = 0;
		foreach (DateTime date in dates)
		{
			int nextFee = GetTollFee(date, vehicle);
			int tempFee = GetTollFee(intervalStart, vehicle);

			long diffInMillies = date.Millisecond - intervalStart.Millisecond;
			long minutes = diffInMillies / 1000 / 60;

			if (minutes <= 60)
			{
				if (totalFee > 0) totalFee -= tempFee;
				if (nextFee >= tempFee) tempFee = nextFee;
				totalFee += tempFee;
			}
			else
			{
				totalFee += nextFee;
			}
		}
		if (totalFee > 60) totalFee = 60;
		return totalFee;
	}

	public int GetTollFee(DateTime date, VehicleType vehicle)
	{
		if (new CalendarDay(date.Year, date.Month, date.Day).IsTollFree || vehicle.IsTollFree()) return 0;

		return new TimeOfDay(date.Hour, date.Minute).TollFee;
	}
}

static class VehicleTypeExtension
{
	public static bool IsTollFree(this VehicleType self)
	{
		return self != VehicleType.Car;
	}
}
