using Snake.Enums;

namespace Snake.Entities.UI
{
	public class ButtonLevel : Button
	{
		public readonly GameRooms GameRoom;
		public readonly bool IsUnlocked;

		public ButtonLevel(int x, int y, GameRooms game_room) : base(x, y, 150, 84) {
			this.GameRoom = game_room;
			this.IsUnlocked = this.DetermineIsUnlocked();
		}

		private DrawLocations[] _drawData = null;
		protected override DrawLocations[] DrawData {
			get {
				if (this._drawData == null) {
					if (this.IsUnlocked) {
						// Do something to set up draw data
					} else {
						// Do something to set up lock data
					}
				}

				// return this._drawData;
				return new DrawLocations[0];
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
	}
}
