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

			var timer = new GameTimeSpan();
			Action initialize = () => {
#if ADS
#if ANDROID
#if AMAZON
				Engine.SpawnInstance<AmazonUpgrade>();
#endif
#endif
#endif

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

				// Preload in game assemblies
				for (int i = 0; i < this.AssembliesToPreload.Length; i++) {
					try {
						System.Reflection.Assembly.Load(this.AssembliesToPreload[i]);
					} catch {
						Debug.WriteLine("Unable to preload: " + this.AssembliesToPreload[i]);
					}
				}

				// If time remains after doing these operations set the time to allow splash screen to remain
				int remaining_time_to_wait = RoomInit.MinimumSplashDuration - (int)timer.TotalMilliseconds;
				if (remaining_time_to_wait < 0)
					remaining_time_to_wait = 0;

				// Setup switching to RoomMain after timeout
				Engine.SpawnInstance(new TimedExecution(remaining_time_to_wait, () => Engine.ChangeRoom<RoomMain>()));
			};

			Engine.SpawnInstance(new TimedExecution(500, initialize));
		}

		public override void onSwitchAway(Room next_room) {
		}

		private readonly string[] AssembliesToPreload = {
			"System.ObjectModel",
			"netstandard",
			"System.Runtime.Serialization.Formatters",
			"System.Diagnostics.TraceSource",
			"System.Linq.Expressions",
			"System.ComponentModel.TypeConverter",
			"System.Runtime.Numerics",
			"System.Collections.Specialized",
			"System.Collections.Concurrent",
			"System.Drawing.Primitives",
			"System.Runtime.Serialization.Primitives",
			"System.Data.Common",
			"System.Xml.ReaderWriter",
			"System.Private.Xml",
			"System.ComponentModel.Primitives",
			"System.Reflection.Emit.ILGeneration",
			"System.Reflection.Primitives",
			"System.Reflection.Emit.Lightweight",
			"System.Threading.Tasks",
			"Newtonsoft.Json"
		};
	}
}
