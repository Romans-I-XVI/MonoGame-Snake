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
		public float StartScale = 1f;
		public float DestScale = 1f;
		public Vector2 StartPosition;
		public Vector2 DestPosition;
		public GameTimeSpan AdjustTimer = new GameTimeSpan();
		public int AdjustDuration = 0;
		public Tween AdjustTween = Tween.LinearTween;
		protected abstract DrawLocations[] DrawData { get; }
		protected virtual Texture2D BackgroundTexture => ContentHolder.Get(Settings.CurrentBackground);
		protected virtual Texture2D WallTexture => ContentHolder.Get(Settings.CurrentWall);
		protected virtual Texture2D SnakeTexture => ContentHolder.Get(Settings.CurrentSnake);
		protected virtual Texture2D FoodTexture => ContentHolder.Get(Settings.CurrentFood);
		protected Action<SpriteBatch> ExtraDrawingBegin = null;
		protected Action<SpriteBatch> ExtraDrawingEnd = null;
		protected float Scale = 1f;
		protected bool ShouldDrawBorder = true;

		protected Button(int x, int y, int width, int height) {
			this.BaseX = x;
			this.BaseY = y;
			this.BaseWidth = width;
			this.BaseHeight = height;
			this.Position = new Vector2(this.BaseX, this.BaseY);
			this.StartPosition = this.Position;
			this.DestPosition = this.Position;
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);

			float current_time = this.AdjustTimer.TotalMilliseconds;
			this.Scale = Tweening.SwitchTween(this.AdjustTween, this.StartScale, this.DestScale, current_time, this.AdjustDuration);
			this.Position.X = Tweening.SwitchTween(this.AdjustTween, this.StartPosition.X, this.DestPosition.X, current_time, this.AdjustDuration);
			this.Position.Y = Tweening.SwitchTween(this.AdjustTween, this.StartPosition.Y, this.DestPosition.Y, current_time, this.AdjustDuration);
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
				[DrawDataTextures.Portal] = ContentHolder.Get(AvailableTextures.portal_0)
			};

			// Cut out the base position from background and redraw (applicable for user imported themes)
			var background = new Region(this.BackgroundTexture, this.BaseX - this.BaseWidth / 2, this.BaseY - this.BaseHeight / 2, this.BaseWidth, this.BaseHeight, 0, 0);
			sprite_batch.Draw(background, scaled_pos, Color.White, 0, new Vector2(this.Scale));

			// Draw transparent overlay and borders
			float edge = 2 * this.Scale;
			const float alpha = (28 / 255f);
			RectangleDrawer.Draw(sprite_batch, scaled_pos.X, scaled_pos.Y, scaled_width, scaled_height, Color.Black * alpha);
			if (this.ShouldDrawBorder) {
				RectangleDrawer.Draw(sprite_batch, scaled_pos.X, scaled_pos.Y, scaled_width, edge, Color.Black * alpha);
				RectangleDrawer.Draw(sprite_batch, scaled_pos.X, scaled_pos.Y + scaled_height - edge, scaled_width, edge, Color.Black * alpha);
				RectangleDrawer.Draw(sprite_batch, scaled_pos.X, scaled_pos.Y + edge, edge, scaled_height - edge * 2, Color.Black * alpha);
				RectangleDrawer.Draw(sprite_batch, scaled_pos.X + scaled_width - edge, scaled_pos.Y + edge, edge, scaled_height - edge * 2, Color.Black * alpha);
			}

			this.ExtraDrawingBegin?.Invoke(sprite_batch);

			for (int i = 0; i < this.DrawData.Length; i++) {
				var data = this.DrawData[i];
				var texture = textures[data.DrawDataTexture];
				for (int j = 0; j < data.Locations.Length; j++) {
					var pos = data.Locations[j];
					var source_rectangle = new Rectangle(0, 0, texture.Width, texture.Height);;

					// Cut off left and top if necessary
					if (pos.X < 0) {
						source_rectangle.X = (int)(-pos.X / data.Scale);
						source_rectangle.Width -= source_rectangle.X;
						pos.X += source_rectangle.X * data.Scale;
					}
					if (pos.Y < 0) {
						source_rectangle.Y = (int)(-pos.Y / data.Scale);
						source_rectangle.Height -= source_rectangle.Y;
						pos.Y += source_rectangle.Y * data.Scale;
					}

					// Cut off right and bottom if necessary
					float right_x = pos.X + source_rectangle.Width * data.Scale;
					if (right_x > this.BaseWidth) {
						source_rectangle.Width -= (int)((right_x - this.BaseWidth) / data.Scale);
					}
					float bottom_y = pos.Y + source_rectangle.Height * data.Scale;
					if (bottom_y > this.BaseHeight) {
						source_rectangle.Height -= (int)((bottom_y - this.BaseHeight) / data.Scale);
					}

					sprite_batch.Draw(texture, scaled_pos + pos * this.Scale, source_rectangle, Color.White, 0, vector_zero, data.Scale * this.Scale, SpriteEffects.None, 0);
				}
			}

			this.ExtraDrawingEnd?.Invoke(sprite_batch);
		}

		public void Adjust(Vector2 position, float scale, int duration, Tween tween) {
			this.StartPosition = this.Position;
			this.DestPosition = position;
			this.StartScale = this.Scale;
			this.DestScale = scale;
			this.AdjustDuration = duration;
			this.AdjustTween = tween;
			this.AdjustTimer.Mark();
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
			Portal
		}
	}
}
