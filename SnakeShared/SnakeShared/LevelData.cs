using Newtonsoft.Json;

namespace Snake
{
	public class LevelData
	{
		[JsonProperty(PropertyName = "food_spawn")]
		public FoodSpawn FoodSpawn;

		[JsonProperty(PropertyName = "wall_spawns")]
		public WallSpawn[] WallSpawns;
	}

	public class FoodSpawn
	{
		[JsonProperty(PropertyName = "x")]
		public readonly int X;

		[JsonProperty(PropertyName = "y")]
		public readonly int Y;

		public FoodSpawn(int x, int y) {
			this.X = x;
			this.Y = y;
		}
	}

	public class WallSpawn
	{
		[JsonProperty(PropertyName = "x")]
		public readonly int X;

		[JsonProperty(PropertyName = "y")]
		public readonly int Y;

		[JsonProperty(PropertyName = "scale")]
		public readonly float Scale;

		public WallSpawn(int x, int y, float scale) {
			this.X = x;
			this.Y = y;
			this.Scale = scale;
		}
	}
}
