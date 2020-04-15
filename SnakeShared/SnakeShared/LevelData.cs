namespace Snake
{
	public class LevelData
	{
		public ObjectSpawn[] ObjectSpawns;
	}

	public class ObjectSpawn {}

	public class WallSpawn : ObjectSpawn
	{
		public readonly int X;
		public readonly int Y;
		public readonly float Scale;

		public WallSpawn(int x, int y, float scale) {
			this.X = x;
			this.Y = y;
			this.Scale = scale;
		}
	}
}
