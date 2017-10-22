using System;

namespace TollCalculator
{
	public class TollCalculator
	{
		readonly CalendarDay day;
		readonly VehicleType vehicle;

		public int TotalFee { get; private set; }

		TimeOfDay? startOfTheHour;
		int feeForThisHour;

		public TollCalculator(CalendarDay day, VehicleType vehicle)
		{
			this.day = day;
			this.vehicle = vehicle;
		}

		public void PassToll(TimeOfDay time)
		{
			if (vehicle.IsTollFree()) return;
			if (day.IsTollFree) return;

			int nextFee = time.TollFee;
			if (startOfTheHour == null) startOfTheHour = time;

			if (time.hour - startOfTheHour.Value.hour == 0 ||
				time.hour - startOfTheHour.Value.hour == 1 && time.minute < startOfTheHour.Value.minute)
			{
				TotalFee -= feeForThisHour;
				feeForThisHour = Math.Max(feeForThisHour, nextFee);
				TotalFee += feeForThisHour;
			}
			else
			{
				TotalFee += nextFee;
				feeForThisHour = nextFee;
				startOfTheHour = time;
			}

			if (TotalFee > 60) TotalFee = 60;
		}
	}

	static class VehicleTypeExtension
	{
		public static bool IsTollFree(this VehicleType self)
		{
			return self != VehicleType.Car;
		}
	}
}
