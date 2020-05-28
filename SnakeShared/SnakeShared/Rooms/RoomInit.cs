using System;
using MonoEngine;
using System.Collections.Generic;
using System.Diagnostics;
using Snake.Entities;
using Snake.Enums;

namespace Snake.Rooms
{
	class RoomInit : Room
	{
		private const int MinimumSplashDuration = 2500;

		public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
			Engine.SpawnInstance<LoadingSplash>();

			// Load all save data in to cache
			var timer = new GameTimeSpan();
			Action initialize_data = () => {
#if XBOX_LIVE
				// Wait for Xbox Live login to complete if attempting
				const int xbox_live_timeout = 5000;
				while (XboxLiveObject.CurrentlyAttemptingSignIn && timer.TotalMilliseconds <= xbox_live_timeout) {
					System.Threading.Thread.Sleep(10);
				}
#endif

				// Load all game data to initialize the SaveDataHandler cache
				Debug.WriteLine("########### Loading Save Files In To Cache ##########");
				foreach (GameRooms game_room in Enum.GetValues(typeof(GameRooms))) {
					foreach (GameplaySpeeds gameplay_speed in Enum.GetValues(typeof(GameplaySpeeds))) {
						string data = SaveDataHandler.LoadData(Settings.GetSaveFilePath(game_room, gameplay_speed));
						Debug.WriteLine(game_room + " - " + gameplay_speed + ": " + data);
					}
				}
				Debug.WriteLine("########### Loading Save Files In To Cache ##########");

				// Wait until initial data loading occurs to spawn persistent stat tracker entity
				Engine.SpawnInstance<StatTracker>();

				// If time remains after doing these operations set the time to allow splash screen to remain
				int remaining_time_to_wait = RoomInit.MinimumSplashDuration - (int)timer.TotalMilliseconds;
				if (remaining_time_to_wait < 0)
					remaining_time_to_wait = 0;

				// Setup switching to RoomMain after timeout
				Engine.SpawnInstance(new TimedExecution(remaining_time_to_wait, () => Engine.ChangeRoom<RoomMain>()));
			};

			Engine.SpawnInstance(new TimedExecution(100, initialize_data));
		}

		public override void onSwitchAway(Room next_room) {
		}
	}
}
