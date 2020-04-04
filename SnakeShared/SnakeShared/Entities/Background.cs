using MonoEngine;

namespace Snake.Entities
{
	public class Background : Entity
	{
		public Background() {
			this.Depth = int.MaxValue;
			var texture = ContentHolder.Get(Settings.CurrentBackground);
			var region = new Region(texture, 0, 0, texture.Width, texture.Height, 0, 0);
			var sprite = new Sprite(region);
			this.AddSprite("main", sprite);
		}
	}
}
