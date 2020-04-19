using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using MonoEngine;
using Newtonsoft.Json;

namespace Snake
{
	public class LevelData
	{
		[JsonProperty(PropertyName = "food_spawn")]
		public Point? FoodSpawn;

		[JsonProperty(PropertyName = "snake_spawn")]
		public Point? SnakeSpawn;

		[JsonProperty(PropertyName = "wall_spawns")]
		public WallSpawn[] WallSpawns;

		[JsonProperty(PropertyName = "portal_spawns")]
		public PortalSpawn[] PortalSpawns;
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

	public class PortalSpawn
	{
		[JsonProperty(PropertyName = "x")]
		public readonly int X;

		[JsonProperty(PropertyName = "y")]
		public readonly int Y;

		[JsonProperty(PropertyName = "id")]
		public readonly int ID;

		[JsonProperty(PropertyName = "goto_up")]
		public readonly int GotoUp;

		[JsonProperty(PropertyName = "goto_down")]
		public readonly int GotoDown;

		[JsonProperty(PropertyName = "goto_left")]
		public readonly int GotoLeft;

		[JsonProperty(PropertyName = "goto_right")]
		public readonly int GotoRight;

		[JsonProperty(PropertyName = "reverse_direction")]
		public readonly ReadOnlyDictionary<Directions, bool> ReverseDirection;

		public PortalSpawn(int x, int y, int id, int goto_up, int goto_down, int goto_left, int goto_right, ReadOnlyDictionary<Directions, bool> reverse_direction = null) {
			this.X = x;
			this.Y = y;
			this.ID = id;
			this.GotoUp = goto_up;
			this.GotoRight = goto_right;
			this.GotoLeft = goto_left;
			this.GotoDown = goto_down;
			this.ReverseDirection = reverse_direction;
		}
	}
}
