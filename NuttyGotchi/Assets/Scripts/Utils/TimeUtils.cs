using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace  Utils {
	public static class TimeUtils {
		private static readonly DateTime startingTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		public static DateTime dateTimeFromULongInLocalTime(ulong unixTimeStamp) {
			
			TimeZone zone = TimeZone.CurrentTimeZone;
			DateTime date = startingTime.AddSeconds(unixTimeStamp);
			TimeSpan offset = zone.GetUtcOffset(DateTime.Now);
			DateTime localDate = date + offset;
			//return TimeZoneInfo.ConvertTimeFromUtc(date, TimeZoneInfo.Local);
			return localDate;
		}

		public static ulong uLongFromDateTime(DateTime dateTime) {
			DateTime utcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTime);
			ulong unixTimeStamp = (ulong) (utcDateTime.Subtract(startingTime).TotalSeconds);
			return unixTimeStamp;
		}
		
		public static int differenceInSeconds(DateTime a, DateTime b) {
			return System.Convert.ToInt32(Mathf.Round(Mathf.Abs((float) (a - b).TotalSeconds)));
		}
		
		public static int differenceInMinutes(DateTime a, DateTime b) {
			return System.Convert.ToInt32(Mathf.Round(Mathf.Abs((float) (a - b).TotalMinutes)));
		}
		
		public static int differenceInHours(DateTime a, DateTime b) {
			return System.Convert.ToInt32(Mathf.Round(Mathf.Abs((float) (a - b).TotalHours)));
		}
		
		public static int differenceInDays(DateTime a, DateTime b) {
			return System.Convert.ToInt32(Mathf.Round(Mathf.Abs((float) (a - b).TotalDays)));
		}

		public static double getDays(DateTime time) {
			return time.ToUniversalTime().Subtract(startingTime).TotalDays;
		}
		
		public static double getHours(DateTime time) {
			return time.ToUniversalTime().Subtract(startingTime).TotalHours;
		}
		
		public static double getMinutes(DateTime time) {
			return time.ToUniversalTime().Subtract(startingTime).TotalMinutes;
		}
		
		public static double getSeconds(DateTime time) {
			return time.ToUniversalTime().Subtract(startingTime).TotalSeconds;
		}
	}
}

