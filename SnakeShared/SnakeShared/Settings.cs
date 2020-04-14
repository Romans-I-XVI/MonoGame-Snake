using System;
using Snake.Enums;

namespace Snake
{
	public static class Settings
	{
		public static int CurrentTheme = 0;
		public static GameplaySpeeds CurrentGameplaySpeed = GameplaySpeeds.Medium;
		public static GameRooms CurrentGameRoom = GameRooms.Open;

		public static AvailableTextures CurrentBackground => Enum.Parse<AvailableTextures>("theme_" + Settings.CurrentTheme + "_background");
		public static AvailableTextures CurrentSnake => Enum.Parse<AvailableTextures>("theme_" + Settings.CurrentTheme + "_snake");
		public static AvailableTextures CurrentFood => Enum.Parse<AvailableTextures>("theme_" + Settings.CurrentTheme + "_food");
		public static AvailableTextures CurrentWall => Enum.Parse<AvailableTextures>("theme_" + Settings.CurrentTheme + "_wall");
		public static string CurrentSaveFilePath => Settings.CurrentGameRoom + "_" + Settings.CurrentGameplaySpeed + ".txt";
	}
}
