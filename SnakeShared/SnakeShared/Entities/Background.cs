using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace Snake.Entities
{
	public class Background : Entity
	{
		public Background() {
			this.IsPersistent = true;
			this.Depth = int.MaxValue;
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);
			sprite_batch.Draw(ContentHolder.Get(Settings.CurrentBackground), this.Position, Color.White);
		}
	}
}
