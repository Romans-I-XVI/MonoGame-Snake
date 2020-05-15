using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace Snake.Entities.UI
{
	public class WallDecoration : Entity
	{
		private readonly Vector2[] DrawPositions = new[] {
			new Vector2(52, 30),
			new Vector2(854 - 30 - 52, 30),
			new Vector2(52, 480 - 30 - 30),
			new Vector2(854 - 30 - 52, 480 - 30 - 30)
		};

		public WallDecoration() {
			this.Depth = -int.MaxValue + 3;
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);

			var texture = ContentHolder.Get(Settings.CurrentWall);

			foreach (var position in this.DrawPositions) {
				sprite_batch.Draw(texture, position, Color.White);
			}
		}
	}
}
