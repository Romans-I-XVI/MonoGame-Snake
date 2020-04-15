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
			int start_delay = 0;
			if (args != null && args.ContainsKey("start_delay")) {
				start_delay = (int)args["start_delay"];
			}

			Engine.SpawnInstance(new TimedExecution(start_delay, () => {
				MediaPlayer.IsRepeating = true;
				MediaPlayer.Play(ContentHolder.Get(AvailableMusic.background_music));
			}));
			Engine.SpawnInstance(new Entities.Snake(start_delay));
			Engine.SpawnInstance<ControlPause>();
			Engine.SpawnInstance<ControlFoodSpawner>();

			LevelData level_data = null;
			if (Settings.CurrentGameRoom == GameRooms.Classic) {
				level_data = new ClassicLevelData();
			}

			if (level_data != null) {
				if (level_data.ObjectSpawns != null) {
					foreach (var spawn in level_data.ObjectSpawns) {
						if (spawn is WallSpawn) {
							var wall_spawn = (WallSpawn)spawn;
							Engine.SpawnInstance(new Wall(wall_spawn.X, wall_spawn.Y, wall_spawn.Scale));
						}
					}
				}
			}
		}

		public override void onSwitchAway(Room next_room) {
			MediaPlayer.Stop();
		}
	}
}
