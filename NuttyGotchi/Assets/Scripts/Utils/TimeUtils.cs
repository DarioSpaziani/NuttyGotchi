using UnityEngine;
using System;

namespace  Utils {
	public static class TimeUtils {
		private static readonly DateTime StartingTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		[Obsolete("Obsolete")]
		public static DateTime DateTimeFromULongInLocalTime(ulong unixTimeStamp) {
			
			TimeZone zone = TimeZone.CurrentTimeZone;
			DateTime date = StartingTime.AddSeconds(unixTimeStamp);
			TimeSpan offset = zone.GetUtcOffset(DateTime.Now);
			DateTime localDate = date + offset;
			//return TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.Local);
			return localDate;
		}

		public static ulong ULongFromDateTime(DateTime dateTime) {
			DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTime);
			ulong unixTimeStamp = (ulong) (utcDateTime.Subtract(StartingTime).TotalSeconds);
			return unixTimeStamp;
		}
		
		public static int DifferenceInSeconds(DateTime a, DateTime b) {
			return Convert.ToInt32(Mathf.Round(Mathf.Abs((float) (a - b).TotalSeconds)));
		}
		
		public static int DifferenceInMinutes(DateTime a, DateTime b) {
			return Convert.ToInt32(Mathf.Round(Mathf.Abs((float) (a - b).TotalMinutes)));
		}
		
		public static int DifferenceInHours(DateTime a, DateTime b) {
			return Convert.ToInt32(Mathf.Round(Mathf.Abs((float) (a - b).TotalHours)));
		}
		
		public static int DifferenceInDays(DateTime a, DateTime b) {
			return Convert.ToInt32(Mathf.Round(Mathf.Abs((float) (a - b).TotalDays)));
		}

		public static double GetDays(DateTime time) {
			return time.ToUniversalTime().Subtract(StartingTime).TotalDays;
		}
		
		public static double GetHours(DateTime time) {
			return time.ToUniversalTime().Subtract(StartingTime).TotalHours;
		}
		
		public static double GetMinutes(DateTime time) {
			return time.ToUniversalTime().Subtract(StartingTime).TotalMinutes;
		}
		
		public static double GetSeconds(DateTime time) {
			return time.ToUniversalTime().Subtract(StartingTime).TotalSeconds;
		}
	}
}