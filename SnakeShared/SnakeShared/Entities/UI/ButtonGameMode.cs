using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine;

namespace Snake.Entities.UI
{
	public abstract class ButtonGameMode : Entity
	{
		protected abstract Vector2[] LogoLocations { get; }
		protected abstract Vector2 LogoLocationsScale { get; }
		protected abstract Vector2[] SnakeLocations { get; }
		protected abstract Vector2[] WallLocations { get; }
		protected abstract Vector2? FoodLocation { get; }
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
			var snake_texture = ContentHolder.Get(Settings.CurrentSnake);
			var food_texture = ContentHolder.Get(Settings.CurrentFood);
			var wall_texture = ContentHolder.Get(Settings.CurrentWall);
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

			// Draw food if exists
			if (this.FoodLocation != null) {
				sprite_batch.Draw(food_texture, scaled_pos + FoodLocation.Value * this.Scale, null, Color.White, 0, vector_zero, food_scale * this.Scale, SpriteEffects.None, 0);
			}

			// Draw snake texture at all logo locations
			for (int i = 0; i < this.LogoLocations.Length; i++) {
				var pos = this.LogoLocations[i];
				sprite_batch.Draw(snake_texture, scaled_pos + pos * this.Scale, null, Color.White, 0, vector_zero, this.LogoLocationsScale * this.Scale, SpriteEffects.None, 0);
			}

			// Draw all wall locations
			for (int i = 0; i < this.WallLocations.Length; i++) {
				var pos = this.WallLocations[i];
				sprite_batch.Draw(wall_texture, scaled_pos + pos * this.Scale, null, Color.White, 0, vector_zero, wall_scale * this.Scale, SpriteEffects.None, 0);
			}

			// Draw all snake locations
			for (int i = 0; i < this.SnakeLocations.Length; i++) {
				var pos = this.SnakeLocations[i];
				sprite_batch.Draw(snake_texture, scaled_pos + pos * this.Scale, null, Color.White, 0, vector_zero, snake_scale * this.Scale, SpriteEffects.None, 0);
			}
		}

	}
}
