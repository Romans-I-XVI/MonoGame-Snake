using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;
using Snake.GameEvents;

namespace Snake.Entities
{
	public class SnakePortalAnimation : Entity
	{
		private readonly Vector2 InSnakePos;
		private readonly Vector2 OutSnakePos;
		private readonly Vector2 InPortalPos;
		private readonly Vector2 OutPortalPos;
		private int SnakeLength;
		private int TailIndex = 0;
		private int AnimationTick = -1;

		public SnakePortalAnimation(Vector2 in_snake_pos, Vector2 out_snake_pos, Vector2 in_portal_pos, Vector2 out_portal_pos, int snake_length) {
			this.Depth = -1;
			this.InSnakePos = in_snake_pos;
			this.OutSnakePos = out_snake_pos;
			this.InPortalPos = in_portal_pos;
			this.OutPortalPos = out_portal_pos;
			this.SnakeLength = snake_length;
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);

			// This animation processing is locked to the on screen movement of the the snake position (updated through posting of game events).
			// So where tweening would normally be done with time and duration, here it is done with movement ticks related to the snake part size.
			var texture = ContentHolder.Get(Settings.CurrentSnake);
			float t = this.AnimationTick;
			const float d = Snake.Size - 1;

			float in_x = Tweening.LinearTween(this.InSnakePos.X, this.InPortalPos.X, t, d);
			float in_y = Tweening.LinearTween(this.InSnakePos.Y, this.InPortalPos.Y, t, d);
			float out_x = Tweening.LinearTween( this.OutPortalPos.X, this.OutSnakePos.X, t, d);
			float out_y = Tweening.LinearTween(this.OutPortalPos.Y, this.OutSnakePos.Y, t, d);
			float shrink_scale = Tweening.LinearTween(1, 0, t, d);
			float grow_scale = Tweening.LinearTween(0, 1, t, d);

			sprite_batch.Draw(texture, new Vector2(in_x, in_y), null, Color.White, 0, new Vector2(Snake.Size / 2f), new Vector2(shrink_scale), SpriteEffects.None, 1);
			if (this.TailIndex < this.SnakeLength)
				sprite_batch.Draw(texture, new Vector2(out_x, out_y), null, Color.White, 0, new Vector2(Snake.Size / 2f), new Vector2(grow_scale), SpriteEffects.None, 1);
		}

		public override void onGameEvent(GameEvent game_event) {
			base.onGameEvent(game_event);

			if (game_event is FoodEatenEvent) {
				this.SnakeLength++;
			} else if (game_event is SnakeDiedEvent) {
				this.Destroy();
			} else if (game_event is SnakeMoveEvent) {
				this.AnimationTick++;
				if (this.AnimationTick > Snake.Size - 1) {
					this.AnimationTick = 0;
					this.TailIndex++;
					if (this.TailIndex > this.SnakeLength) {
						this.Destroy();
					}
				}
			}
		}
	}
}
