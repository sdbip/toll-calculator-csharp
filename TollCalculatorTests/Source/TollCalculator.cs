using System;
using System.Globalization;

public class TollCalculator
{

	/**
     * Calculate the total toll fee for one day
     *
     * @param vehicle - the vehicle
     * @param dates   - date and time of all passes on one day
     * @return - the total toll fee for that day
     */

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

	private bool IsTollFreeVehicle(VehicleType vehicle)
	{
		return vehicle != VehicleType.Car;
	}

	public int GetTollFee(DateTime date, VehicleType vehicle)
	{
		if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

		var timeOfDay = new TimeOfDay(date.Hour, date.Minute);
		return timeOfDay.GetTollFee();
	}

	private Boolean IsTollFreeDate(DateTime date)
	{
		var calendarDay = new CalendarDay(date.Year, date.Month, date.Day);
		return calendarDay.IsTollFree();
	}
}
