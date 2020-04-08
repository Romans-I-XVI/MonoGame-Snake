using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine;

namespace Snake.Entities.UI
{
	public abstract class Button : Entity
	{
		public readonly int BaseWidth;
		public readonly int BaseHeight;
		public readonly int BaseX;
		public readonly int BaseY;
		protected abstract DrawLocations[] DrawData { get; }
		protected virtual Texture2D BackgroundTexture => ContentHolder.Get(Settings.CurrentBackground);
		protected virtual Texture2D WallTexture => ContentHolder.Get(Settings.CurrentWall);
		protected virtual Texture2D SnakeTexture => ContentHolder.Get(Settings.CurrentSnake);
		protected virtual Texture2D FoodTexture => ContentHolder.Get(Settings.CurrentFood);
		protected Action<SpriteBatch> ExtraDrawingBegin = null;
		protected Action<SpriteBatch> ExtraDrawingEnd = null;
		protected float Scale = 1f;

		protected Button(int x, int y, int width, int height) {
			this.BaseX = x;
			this.BaseY = y;
			this.BaseWidth = width;
			this.BaseHeight = height;
			this.Position = new Vector2(this.BaseX, this.BaseY);
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);

			var scaled_pos = new Vector2(
				this.Position.X - (this.BaseWidth / 2f) * this.Scale,
				this.Position.Y - (this.BaseHeight / 2f) * this.Scale);
			float scaled_width = this.BaseWidth * this.Scale;
			float scaled_height = this.BaseHeight * this.Scale;
			var vector_zero = Vector2.Zero;
			var textures = new Dictionary<DrawDataTextures, Texture2D> {
				[DrawDataTextures.Snake] = this.SnakeTexture,
				[DrawDataTextures.Food] = this.FoodTexture,
				[DrawDataTextures.Wall] = this.WallTexture,
			};

			// Cut out the base position from background and redraw (applicable for user imported themes)
			var background = new Region(this.BackgroundTexture, this.BaseX - this.BaseWidth / 2, this.BaseY - this.BaseHeight / 2, this.BaseWidth, this.BaseHeight, 0, 0);
			sprite_batch.Draw(background, scaled_pos, Color.White, 0, new Vector2(this.Scale));

			// Draw transparent overlay and borders
			float edge = 2 * this.Scale;
			const float alpha = (28 / 255f);
			RectangleDrawer.Draw(sprite_batch, scaled_pos.X, scaled_pos.Y, scaled_width, scaled_height, Color.Black * alpha);
			RectangleDrawer.Draw(sprite_batch, scaled_pos.X, scaled_pos.Y, scaled_width, edge, Color.Black * alpha);
			RectangleDrawer.Draw(sprite_batch, scaled_pos.X, scaled_pos.Y + scaled_height - edge, scaled_width, edge, Color.Black * alpha);
			RectangleDrawer.Draw(sprite_batch, scaled_pos.X, scaled_pos.Y + edge, edge, scaled_height - edge * 2, Color.Black * alpha);
			RectangleDrawer.Draw(sprite_batch, scaled_pos.X + scaled_width - edge, scaled_pos.Y + edge, edge, scaled_height - edge * 2, Color.Black * alpha);

			this.ExtraDrawingBegin?.Invoke(sprite_batch);

			for (int i = 0; i < this.DrawData.Length; i++) {
				var data = this.DrawData[i];
				var texture = textures[data.DrawDataTexture];
				for (int j = 0; j < data.Locations.Length; j++) {
					var pos = data.Locations[j];
					sprite_batch.Draw(texture, scaled_pos + pos * this.Scale, null, Color.White, 0, vector_zero, data.Scale * this.Scale, SpriteEffects.None, 0);
				}
			}

			this.ExtraDrawingEnd?.Invoke(sprite_batch);
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
