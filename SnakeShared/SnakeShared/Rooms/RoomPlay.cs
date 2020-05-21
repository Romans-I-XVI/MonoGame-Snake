using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using MonoEngine;
using Snake.Entities;
using Snake.Entities.Controls;
using Snake.Enums;

namespace Snake.Rooms
{
	public class RoomPlay : Room
	{
		public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
			Engine.SpawnInstance<ControlPause>();
			Engine.SpawnInstance<ControlPortalAnimation>();

			// Set start delay if passed in through args
			int start_delay = 0;
			if (args != null && args.ContainsKey("start_delay")) {
				start_delay = (int)args["start_delay"];
			}

			// Load level data if called for by current game mode
			LevelData level_data = null;
			if (Settings.CurrentGameRoom == GameRooms.Classic) {
				level_data = new ClassicLevelData();
			} else if (Settings.CurrentGameRoom != GameRooms.Open) {
				level_data = Levels.Load((int)Settings.CurrentGameRoom);
			}

			// If level data exists either build level instantly if replaying room or spawn constructor if first entering room
			if (level_data != null) {
				if (previous_room is RoomPlay) {
					if (level_data.WallSpawns != null) {
						foreach (var spawn in level_data.WallSpawns) {
							Engine.SpawnInstance(new Wall(spawn.X, spawn.Y, spawn.Scale));
						}
					}
					if (level_data.PortalSpawns != null) {
						foreach (var spawn in level_data.PortalSpawns) {
							Engine.SpawnInstance(new Portal(spawn));
						}
					}
				} else {
					start_delay += ControlLevelConstructor.TotalTimeToSpawnLevel(level_data);
					Engine.SpawnInstance(new ControlLevelConstructor(level_data));
				}
			}

			// Spawn first food piece
			if (level_data?.FoodSpawn != null) {
				var spawn = level_data.FoodSpawn.Value;
				Engine.SpawnInstance(new ControlFoodSpawner(spawn.X, spawn.Y));
			} else {
				Engine.SpawnInstance<ControlFoodSpawner>();
			}

			// Spawn snake
			if (level_data?.SnakeSpawn != null) {
				var spawn = level_data.SnakeSpawn.Value;
				Engine.SpawnInstance(new Entities.Snake(start_delay, spawn));
			} else {
				Engine.SpawnInstance(new Entities.Snake(start_delay));
			}

			// Spawn timed execution to start music at snake movement start
			Engine.SpawnInstance(new TimedExecution(start_delay, () => {
				MediaPlayer.IsRepeating = true;
				MediaPlayer.Play(ContentHolder.Get(AvailableMusic.background_music));
			}));
		}

		public override void onSwitchAway(Room next_room) {
			MediaPlayer.Stop();
		}
	}
}
