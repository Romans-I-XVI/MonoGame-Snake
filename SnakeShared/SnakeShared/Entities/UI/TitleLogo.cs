using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace Snake.Entities.UI
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
			for (int i = 0; i < TitleLogo.SnakeLogoLocations.Length; i++) {
				var pos = TitleLogo.SnakeLogoLocations[i];
				sprite_batch.Draw(texture, this.Position + pos, Color.White);
			}
		}

		private static readonly Vector2[] SnakeLogoLocations = {
			// S
			new Vector2(0, 64),
			new Vector2(16, 64),
			new Vector2(32, 64),
			new Vector2(48, 64),
			new Vector2(64, 48),
			new Vector2(48, 32),
			new Vector2(32, 32),
			new Vector2(64, 0),
			new Vector2(16, 32),
			new Vector2(16, 32),
			new Vector2(0, 16),
			new Vector2(16, 0),
			new Vector2(32, 0),
			new Vector2(48, 0),
			// N
			new Vector2(165, 64),
			new Vector2(165, 0),
			new Vector2(165, 16),
			new Vector2(165, 32),
			new Vector2(165, 48),
			new Vector2(149, 48),
			new Vector2(133, 32),
			new Vector2(117, 16),
			new Vector2(101, 0),
			new Vector2(101, 16),
			new Vector2(101, 32),
			new Vector2(101, 48),
			new Vector2(101, 64),
			// A
			new Vector2(266, 64),
			new Vector2(266, 48),
			new Vector2(266, 32),
			new Vector2(250, 32),
			new Vector2(234, 32),
			new Vector2(218, 32),
			new Vector2(266, 16),
			new Vector2(250, 0),
			new Vector2(234, 0),
			new Vector2(218, 0),
			new Vector2(202, 16),
			new Vector2(202, 32),
			new Vector2(202, 48),
			new Vector2(202, 64),
			// K
			new Vector2(367, 64),
			new Vector2(351, 48),
			new Vector2(367, 0),
			new Vector2(351, 16),
			new Vector2(335, 32),
			new Vector2(319, 32),
			new Vector2(303, 0),
			new Vector2(303, 16),
			new Vector2(303, 32),
			new Vector2(303, 48),
			new Vector2(303, 64),
			// E
			new Vector2(468, 64),
			new Vector2(452, 64),
			new Vector2(436, 64),
			new Vector2(420, 64),
			new Vector2(452, 32),
			new Vector2(436, 32),
			new Vector2(420, 32),
			new Vector2(468, 0),
			new Vector2(452, 0),
			new Vector2(436, 0),
			new Vector2(420, 0),
			new Vector2(404, 0),
			new Vector2(404, 16),
			new Vector2(404, 32),
			new Vector2(404, 48),
			new Vector2(404, 64),
		};
	}
}
