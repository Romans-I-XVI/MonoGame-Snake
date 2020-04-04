using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace Snake.Entities
{
	public class TitleLogo : Entity
	{
		public TitleLogo() {
			this.Depth = -int.MaxValue + 2;
			this.Position.X = 185;
			this.Position.Y = 30;
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);

			var texture = ContentHolder.Get(Settings.CurrentSnake);
			for (int i = 0; i < LogoDrawData.SnakeLogoLocations.Length; i++) {
				var pos = LogoDrawData.SnakeLogoLocations[i];
				sprite_batch.Draw(texture, this.Position + pos, Color.White);
			}
		}
	}
}
