using Microsoft.Xna.Framework;
using MonoEngine;

namespace Snake.Entities
{
	public class Wall : Entity
	{
		public Wall(int x, int y) {
			this.Depth = 1;
			this.Position = new Vector2(x, y);
			var texture = ContentHolder.Get(Settings.CurrentWall);
			var region = new Region(texture, 0, 0, texture.Width, texture.Height, texture.Width / 2, texture.Height / 2);
			var sprite = new Sprite(region);
			this.AddSprite("main", sprite);
			this.AddColliderRectangle("main", -texture.Width / 2, -texture.Height / 2, texture.Width, texture.Height);
		}
	}
}
