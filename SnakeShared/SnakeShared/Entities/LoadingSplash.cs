using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace Snake.Entities
{
	public class LoadingSplash : Entity
	{
		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);

			var scale = new Vector2(2 / 3f);
			sprite_batch.Draw(ContentHolder.Get(AvailableTextures.splash), Vector2.Zero, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
		}
	}
}
