using Microsoft.Xna.Framework;
using MonoEngine;

namespace Snake.Entities
{
	public class Wall : Entity
	{
		public Wall(int x, int y, float scale = 1) {
			this.Depth = 1;
			this.Position = new Vector2(x, y);
			var texture = ContentHolder.Get(Settings.CurrentWall);
			var region = new Region(texture, 0, 0, texture.Width, texture.Height, 0, 0);
			var sprite = new Sprite(region) {
				Scale = new Vector2(scale)
			};
			this.AddSprite("main", sprite);
			this.AddColliderRectangle("main", 0, 0, (int)(texture.Width * scale), (int)(texture.Height * scale));
		}
	}
}
