namespace TollCalculator
{
	public struct TimeOfDay
	{
		public int Hour;
		public int Minute;

		public int TollFee
		{
			get
			{
				if (IsBefore(new TimeOfDay(6, 0))) return 0;
				if (IsBefore(new TimeOfDay(6, 30))) return 8;
				if (IsBefore(new TimeOfDay(7, 0))) return 13;
				if (IsBefore(new TimeOfDay(8, 0))) return 18;
				if (IsBefore(new TimeOfDay(8, 30))) return 13;
				if (IsBefore(new TimeOfDay(15, 0))) return 8;
				if (IsBefore(new TimeOfDay(15, 30))) return 13;
				if (IsBefore(new TimeOfDay(17, 0))) return 18;
				if (IsBefore(new TimeOfDay(18, 0))) return 13;
				return IsBefore(new TimeOfDay(18, 30)) ? 8 : 0;
			}
		}

		public TimeOfDay(int hour, int minute)
		{
			Hour = hour;
			Minute = minute;
		}

		private bool IsBefore(TimeOfDay otherTime)
		{
			if (otherTime.Hour > Hour) return true;
			if (otherTime.Hour < Hour) return false;
			return otherTime.Minute > Minute;
		}
	}
}
