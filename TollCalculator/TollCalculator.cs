using System;

namespace TollCalculator
{
	public class TollCalculator
	{
		private readonly CalendarDay _day;
		private readonly VehicleType _vehicle;

		public int TotalFee { get; private set; }

		private TimeOfDay? _startOfTheHour;
		private int _feeForThisHour;

		public TollCalculator(CalendarDay day, VehicleType vehicle)
		{
			_day = day;
			_vehicle = vehicle;
		}

		public void PassToll(TimeOfDay time)
		{
			if (_vehicle.IsTollFree()) return;
			if (_day.IsTollFree) return;

			var nextFee = time.TollFee;
			if (_startOfTheHour == null) _startOfTheHour = time;

			if (time.Hour - _startOfTheHour.Value.Hour == 0 ||
				time.Hour - _startOfTheHour.Value.Hour == 1 && time.Minute < _startOfTheHour.Value.Minute)
			{
				TotalFee -= _feeForThisHour;
				_feeForThisHour = Math.Max(_feeForThisHour, nextFee);
				TotalFee += _feeForThisHour;
			}
			else
			{
				TotalFee += nextFee;
				_feeForThisHour = nextFee;
				_startOfTheHour = time;
			}

			if (TotalFee > 60) TotalFee = 60;
		}
	}

	internal static class VehicleTypeExtension
	{
		public static bool IsTollFree(this VehicleType self)
		{
			return self != VehicleType.Car;
		}
	}
}
