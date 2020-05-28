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
		public const string StatTrackerSavePath = "game_stats.txt";
		public static string CurrentSaveFilePath => Settings.CurrentGameRoom + "_" + Settings.CurrentGameplaySpeed + ".txt";
		public static string GetSaveFilePath(GameRooms game_room, GameplaySpeeds gameplay_speed) => game_room + "_" + gameplay_speed + ".txt";

		public static readonly int[] LevelScoreRequirements = {50, 50, 50, 50, 40, 40, 30, 30, 30, 30, 20};
	}
}
