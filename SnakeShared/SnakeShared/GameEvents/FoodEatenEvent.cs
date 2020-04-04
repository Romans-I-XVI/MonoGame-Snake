using MonoEngine;

namespace Snake.GameEvents
{
	public class FoodEatenEvent : GameEvent
	{
		public readonly int X;
		public readonly int Y;

		public FoodEatenEvent(int x, int y) {
			this.X = x;
			this.Y = y;
		}
	}
}
