using System.Collections.Generic;

namespace Snake
{
	public class ClassicLevelData : LevelData
	{
		public ClassicLevelData() {
			var walls = new List<WallSpawn>();
			for (int i = 0; i < 25; i++) {
				walls.Add(new WallSpawn(52 + 30 * i, 30, 1));
			}
			for (int i = 0; i < 12; i++) {
				walls.Add(new WallSpawn(854 - 52 - 30, 30 + 30 + 30 * i, 1));
			}
			for (int i = 0; i < 24; i++) {
				walls.Add(new WallSpawn(854 - 52 - 30 - 30 * i, 480 - 30 - 30, 1));
			}
			for (int i = 0; i < 13; i++) {
				walls.Add(new WallSpawn(52, 480 - 30 - 30 - 30 * i, 1));
			}

			this.WallSpawns = walls.ToArray();
		}
	}
}
