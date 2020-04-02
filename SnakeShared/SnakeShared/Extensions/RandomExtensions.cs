using System;
using System.Collections.Generic;

namespace Snake
{
	public static class RandomExtensions
	{
		public static bool NextBool(this Random random) {
			return random.Next(2) == 0;
		}

		public static T NextItem<T>(this Random random, T[] array) {
			return array[random.Next(array.Length)];
		}

		public static T NextItem<T>(this Random random, List<T> list) {
			return list[random.Next(list.Count)];
		}

		public static T NextEnum<T>(this Random random) {
			var values = Enum.GetValues(typeof(T));
			return (T)values.GetValue(random.Next(values.Length));
		}

		public static int NextDegree(this Random random) {
			return random.Next(360);
		}
	}
}
