using Microsoft.Xna.Framework;
using MonoEngine;

namespace Snake.GameEvents
{
	public class SnakeMoveEvent : GameEvent
	{
		public readonly Directions Direction;
		public readonly int X;
		public readonly int Y;

		public SnakeMoveEvent(int x, int y, Directions direction) {
			this.Direction = direction;
			this.X = x;
			this.Y = y;
		}
	}
}
