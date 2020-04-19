using System.Collections.Generic;
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

			int start_delay = 0;
			if (args != null && args.ContainsKey("start_delay")) {
				start_delay = (int)args["start_delay"];
			}

			LevelData level_data = null;
			if (Settings.CurrentGameRoom == GameRooms.Classic) {
				level_data = new ClassicLevelData();
			}

			if (level_data != null) {
				if (previous_room is RoomPlay) {
					if (level_data.WallSpawns != null) {
						foreach (var spawn in level_data.WallSpawns) {
							Engine.SpawnInstance(new Wall(spawn.X, spawn.Y, spawn.Scale));
						}
					}
				} else {
					start_delay += ControlLevelConstructor.TotalTimeToSpawnLevel(level_data);
					Engine.SpawnInstance(new ControlLevelConstructor(level_data));
				}
			}

			if (level_data?.FoodSpawn != null)
				Engine.SpawnInstance(new ControlFoodSpawner(level_data.FoodSpawn.X, level_data.FoodSpawn.Y));
			else
				Engine.SpawnInstance<ControlFoodSpawner>();

			Engine.SpawnInstance(new TimedExecution(start_delay, () => {
				MediaPlayer.IsRepeating = true;
				MediaPlayer.Play(ContentHolder.Get(AvailableMusic.background_music));
			}));
			Engine.SpawnInstance(new Entities.Snake(start_delay));
		}

		public override void onSwitchAway(Room next_room) {
			MediaPlayer.Stop();
		}
	}
}
