using Microsoft.Xna.Framework;
using MonoEngine;

namespace Snake.Entities
{
	public class Food : Entity
	{
		public const int Size = 10;

		public Food(int x, int y) {
			this.Position = new Vector2(x, y);
			var texture = ContentHolder.Get(Settings.CurrentFood);
			var region = new Region(texture, 0, 0, Food.Size, Food.Size, Food.Size / 2, Food.Size / 2);
			var sprite = new Sprite(region);
			this.AddSprite("main", sprite);
			this.AddColliderRectangle("main", -Food.Size / 2, -Food.Size / 2, Food.Size, Food.Size);
		}
	}
}
