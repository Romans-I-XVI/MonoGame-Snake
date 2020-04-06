using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine;

namespace Snake.Entities.UI
{
	public abstract class ButtonGameMode : Entity
	{
		protected abstract DrawLocations[] DrawData { get; }
		protected const int BaseWidth = 215;
		protected const int BaseHeight = 120;
		protected readonly int BaseX = Engine.Game.CanvasWidth / 2;
		protected readonly int BaseY = Engine.Game.CanvasHeight / 2 + 20;
		protected float Scale = 1f;

		protected ButtonGameMode() {
			this.Position = new Vector2(this.BaseX, this.BaseY);
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);

			var scaled_pos = new Vector2(
				this.Position.X - (ButtonGameMode.BaseWidth / 2f) * this.Scale,
				this.Position.Y - (ButtonGameMode.BaseHeight / 2f) * this.Scale);
			float scaled_width = ButtonGameMode.BaseWidth * this.Scale;
			float scaled_height = ButtonGameMode.BaseHeight * this.Scale;
			var vector_zero = Vector2.Zero;
			var textures = new Dictionary<DrawDataTextures, Texture2D> {
				[DrawDataTextures.Snake] = ContentHolder.Get(Settings.CurrentSnake),
				[DrawDataTextures.Food] = ContentHolder.Get(Settings.CurrentFood),
				[DrawDataTextures.Wall] = ContentHolder.Get(Settings.CurrentWall),
			};
			var food_scale = new Vector2(0.5f);
			var wall_scale = new Vector2(1f / 3f);
			var snake_scale = new Vector2(0.5f);

			// Cut out the base position from background and redraw (applicable for user imported themes)
			var background = new Region(ContentHolder.Get(Settings.CurrentBackground), this.BaseX - ButtonGameMode.BaseWidth / 2, this.BaseY - ButtonGameMode.BaseHeight / 2, ButtonGameMode.BaseWidth, ButtonGameMode.BaseHeight, 0, 0);
			sprite_batch.Draw(background, Vector2.Zero, Color.White);

			// Draw transparent overlay and borders
			float edge = 2 * this.Scale;
			const float alpha = (28 / 255f);
			RectangleDrawer.Draw(sprite_batch, scaled_pos.X, scaled_pos.Y, scaled_width, scaled_height, Color.Black * alpha);
			RectangleDrawer.Draw(sprite_batch, scaled_pos.X, scaled_pos.Y, scaled_width, edge, Color.Black * alpha);
			RectangleDrawer.Draw(sprite_batch, scaled_pos.X, scaled_pos.Y + scaled_height - edge, scaled_width, edge, Color.Black * alpha);
			RectangleDrawer.Draw(sprite_batch, scaled_pos.X, scaled_pos.Y + edge, edge, scaled_height - edge * 2, Color.Black * alpha);
			RectangleDrawer.Draw(sprite_batch, scaled_pos.X + scaled_width - edge, scaled_pos.Y + edge, edge, scaled_height - edge * 2, Color.Black * alpha);

			for (int i = 0; i < this.DrawData.Length; i++) {
				var data = this.DrawData[i];
				var texture = textures[data.DrawDataTexture];
				for (int j = 0; j < data.Locations.Length; j++) {
					var pos = data.Locations[j];
					sprite_batch.Draw(texture, scaled_pos + pos * this.Scale, null, Color.White, 0, vector_zero, data.Scale * this.Scale, SpriteEffects.None, 0);
				}
			}
		}

		protected class DrawLocations
		{
			internal readonly DrawDataTextures DrawDataTexture;
			internal readonly float Scale;
			internal readonly Vector2[] Locations;

			internal DrawLocations(DrawDataTextures draw_data_texture, float scale, Vector2[] locations) {
				this.DrawDataTexture = draw_data_texture;
				this.Scale = scale;
				this.Locations = locations;
			}
		}

		protected enum DrawDataTextures
		{
			Snake,
			Wall,
			Food,
		}
	}
}
