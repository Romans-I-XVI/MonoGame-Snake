using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Snake.Enums;

namespace Snake.Entities.UI
{
	public class ButtonLevel : Button
	{
		public readonly GameRooms GameRoom;
		public readonly bool IsUnlocked;

		public ButtonLevel(int x, int y, GameRooms game_room) : base(x, y, 150, 84) {
			this.ShouldDrawBorder = false;
			this.GameRoom = game_room;
			this.IsUnlocked = this.DetermineIsUnlocked();
		}

		private DrawLocations[] _drawData = null;
		protected override DrawLocations[] DrawData {
			get {
				if (this._drawData == null) {
					if (this.IsUnlocked) {
						const float master_scale = 0.1792f;
						const float offset_mod_x = -1.5f;
						const float offset_mod_y = -1f;
						var draw_data = new List<DrawLocations>();
						var level_data = Levels.Load((int)this.GameRoom);

						// Add the wall draw data
						var wall_spawns_by_scale = new Dictionary<float, List<Vector2>>();
						if (level_data.WallSpawns != null) {
							foreach (var wall_spawn in level_data.WallSpawns) {
								if (!wall_spawns_by_scale.ContainsKey(wall_spawn.Scale))
									wall_spawns_by_scale[wall_spawn.Scale] = new List<Vector2>();
								wall_spawns_by_scale[wall_spawn.Scale].Add(new Vector2(wall_spawn.X * master_scale + offset_mod_x, wall_spawn.Y * master_scale + offset_mod_y));
							}

							foreach (var kv in wall_spawns_by_scale) {
								draw_data.Add(new DrawLocations(DrawDataTextures.Wall, kv.Key * master_scale, kv.Value.ToArray()));
							}
						}

						// Add the portal draw data
						if (level_data.PortalSpawns != null) {
							var portal_spawn_positions = new List<Vector2>();
							foreach (var portal_spawn in level_data.PortalSpawns) {
								portal_spawn_positions.Add(new Vector2(portal_spawn.X * master_scale + offset_mod_x, portal_spawn.Y * master_scale + offset_mod_y));
							}
							draw_data.Add(new DrawLocations(DrawDataTextures.Portal, master_scale, portal_spawn_positions.ToArray()));
						}

						this._drawData = draw_data.ToArray();
					} else {
						this._drawData = new [] {
							new DrawLocations(DrawDataTextures.Snake, 0.3125f, this.GetLockDrawLocations()),
						};
					}
				}

				return this._drawData;
			}
		}

		private bool DetermineIsUnlocked() {
			if (this.GameRoom == GameRooms.Level1)
				return true;

			var previous_level_game_room = this.GameRoom - 1;
			int required_score = Settings.LevelScoreRequirements[(int)previous_level_game_room];
			int highest_score = 0;
			var speeds = new [] {
				GameplaySpeeds.Slow,
				GameplaySpeeds.Medium,
				GameplaySpeeds.Fast
			};

			foreach (var speed in speeds) {
				string save_file_path = Settings.GetSaveFilePath(previous_level_game_room, speed);
				string data = SaveDataHandler.LoadData(save_file_path);
				int score = 0;
				bool success = (data != null && int.TryParse(data, out score));
				if (success && score > highest_score)
					highest_score = score;
			}

			return (highest_score >= required_score);
		}

		private Vector2[] GetLockDrawLocations() {
			return new[] {
				new Vector2(58, 64),
				new Vector2(63, 64),
				new Vector2(68, 64),
				new Vector2(78, 64),
				new Vector2(83, 64),
				new Vector2(88, 64),
				new Vector2(53, 59),
				new Vector2(73, 39),
				new Vector2(78, 44),
				new Vector2(68, 44),
				new Vector2(68, 44),
				new Vector2(58, 59),
				new Vector2(63, 59),
				new Vector2(68, 59),
				new Vector2(78, 59),
				new Vector2(83, 59),
				new Vector2(88, 59),
				new Vector2(93, 59),
				new Vector2(53, 54),
				new Vector2(58, 54),
				new Vector2(63, 54),
				new Vector2(68, 54),
				new Vector2(78, 54),
				new Vector2(83, 54),
				new Vector2(88, 54),
				new Vector2(93, 54),
				new Vector2(53, 49),
				new Vector2(58, 49),
				new Vector2(63, 49),
				new Vector2(68, 49),
				new Vector2(78, 49),
				new Vector2(83, 49),
				new Vector2(88, 49),
				new Vector2(93, 49),
				new Vector2(53, 44),
				new Vector2(58, 44),
				new Vector2(63, 44),
				new Vector2(83, 44),
				new Vector2(88, 44),
				new Vector2(93, 44),
				new Vector2(73, 64),
				new Vector2(73, 59),
				new Vector2(53, 39),
				new Vector2(58, 39),
				new Vector2(63, 39),
				new Vector2(68, 39),
				new Vector2(78, 39),
				new Vector2(83, 39),
				new Vector2(88, 39),
				new Vector2(93, 39),
				new Vector2(58, 34),
				new Vector2(63, 34),
				new Vector2(68, 34),
				new Vector2(73, 34),
				new Vector2(78, 34),
				new Vector2(83, 34),
				new Vector2(88, 34),
				new Vector2(88, 29),
				new Vector2(83, 29),
				new Vector2(63, 29),
				new Vector2(58, 29),
				new Vector2(58, 24),
				new Vector2(63, 24),
				new Vector2(88, 24),
				new Vector2(83, 24),
				new Vector2(83, 19),
				new Vector2(78, 19),
				new Vector2(73, 19),
				new Vector2(68, 19),
				new Vector2(78, 14),
				new Vector2(73, 14),
				new Vector2(68, 14),
				new Vector2(63, 19),
			};
		}
	}
}
