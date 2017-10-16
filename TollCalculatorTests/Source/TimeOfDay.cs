public struct TimeOfDay
{
	public int hour;
	public int minute;

	public TimeOfDay(int hour, int minute)
	{
		this.hour = hour;
		this.minute = minute;
	}

	public int GetTollFee()
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
		if (IsBefore(new TimeOfDay(18, 30))) return 8;
		return 0;
	}

	bool IsBefore(TimeOfDay otherTime)
	{
		if (otherTime.hour > hour) return true;
		if (otherTime.hour < hour) return false;
		return otherTime.minute > minute;
	}
}
