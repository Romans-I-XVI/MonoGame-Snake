using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace Snake.Entities.UI
{
	public class Selector : Entity
	{
		private Vector2 Size;
		private Vector2 PreviousSize;
		private Vector2 DestSize;
		private Vector2 PreviousPosition;
		private Vector2 DestPosition;
		private int MoveDuration;
		private Tween MoveTween;
		private readonly GameTimeSpan Timer = new GameTimeSpan();

		public override void onUpdate(float dt) {
			base.onUpdate(dt);

			float current_time = this.Timer.TotalMilliseconds;
			this.Position.X = Tweening.SwitchTween(this.MoveTween, this.PreviousPosition.X, this.DestPosition.X, current_time, this.MoveDuration);
			this.Position.Y = Tweening.SwitchTween(this.MoveTween, this.PreviousPosition.Y, this.DestPosition.Y, current_time, this.MoveDuration);
			this.Size.X = Tweening.SwitchTween(this.MoveTween, this.PreviousSize.X, this.DestSize.X, current_time, this.MoveDuration);
			this.Size.Y = Tweening.SwitchTween(this.MoveTween, this.PreviousSize.Y, this.DestSize.Y, current_time, this.MoveDuration);
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);

			// Set up variables for drawing selector
			float x = this.Position.X;
			float y = this.Position.Y;
			float width = this.Size.X;
			float height = this.Size.Y;
			var texture = ContentHolder.Get(Settings.CurrentSnake);
			var scale = new Vector2(0.3125f);
			float draw_width = texture.Width * scale.X;
			float draw_height = texture.Height * scale.Y;

			// Draw horizontal selector parts
			float draw_x = x;
			while (draw_x < x + width) {
				sprite_batch.Draw(texture, new Vector2(draw_x, y - draw_height), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
				sprite_batch.Draw(texture, new Vector2(draw_x, y + height), null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
				draw_x += draw_width;
			}

			// Draw vertical selector parts
			float draw_start_y = y - draw_height;
			float draw_y = draw_start_y;
			float total_height = draw_start_y + height + draw_height * 2;
			while (draw_y < total_height) {
				Rectangle? source_rectangle = null;
				if (draw_y + draw_height > total_height) {
					float remaining_space = total_height - draw_y;
					float draw_percent = remaining_space / draw_height;
					source_rectangle = new Rectangle(0, 0, texture.Width, (int)(texture.Height * draw_percent));
				}

				sprite_batch.Draw(texture, new Vector2(x - draw_width, draw_y), source_rectangle, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
				sprite_batch.Draw(texture, new Vector2(x + width, draw_y), source_rectangle, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 1);
				draw_y += draw_height;
			}
		}

		public void Move(float x, float y, float width, float height, int duration, Tween tween) {
			this.PreviousPosition = this.Position;
			this.PreviousSize = this.Size;
			this.DestPosition = new Vector2(x, y);
			this.DestSize = new Vector2(width, height);
			this.MoveDuration = duration;
			this.MoveTween = tween;
			this.Timer.Mark();

			if (duration == 0) {
				this.Position = new Vector2(x, y);
				this.Size = new Vector2(width, height);
			}
		}
	}
}
