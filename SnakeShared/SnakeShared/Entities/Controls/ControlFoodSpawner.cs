using System.Linq;
using System.Numerics;
using Microsoft.Xna.Framework;
using MonoEngine;
using Snake.SnakeShared.GameEvents;

namespace Snake.Entities.Controls
{
	public class ControlFoodSpawner : Entity
	{
		public ControlFoodSpawner() {
			Engine.SpawnInstance(new Food(854 / 2, 480 / 2));
		}

		public override void onGameEvent(GameEvent game_event) {
			base.onGameEvent(game_event);
			if (game_event is FoodEatenEvent) {
				this.SpawnFood();
			}
		}

		private void SpawnFood() {
			new_random_position:
			int x = 93 + 5 + Engine.Random.Next(658) + 1;
			int y = 71 + 5 + Engine.Random.Next(328) + 1;
			if (this.IsPositionInCollision(x, y))
				goto new_random_position;

			Engine.SpawnInstance(new Food(x, y));
		}

		private bool IsPositionInCollision(int x, int y) {
			var rect = new Rectangle(x - 5, y - 5, 10, 10);

			var entities = Engine.GetAllInstances<Entity>();
			for (int i = 0; i < entities.Count; i++) {
				if (entities[i].IsExpired)
					continue;;
				
				for (int j = 0; j < entities[i].Colliders.Count; j++) {
					var collider = entities[i].Colliders[j];
					if (!collider.Enabled)
						continue;

					if (collider is ColliderCircle) {
						if (CollisionChecking.CircleRect(((ColliderCircle)collider).Circle, rect)) {
							return true;
						}
					} else if (collider is ColliderRectangle) {
						if (CollisionChecking.RectRect(((ColliderRectangle)collider).Rectangle, rect))
							return true;
					}
				}
			}

			return false;
		}
	}
}
