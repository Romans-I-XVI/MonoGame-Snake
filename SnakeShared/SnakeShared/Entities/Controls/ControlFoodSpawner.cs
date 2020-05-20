using Microsoft.Xna.Framework;
using MonoEngine;
using Snake.GameEvents;

namespace Snake.Entities.Controls
{
	public class ControlFoodSpawner : Entity
	{
		private const int MinSpawnX = 93 + Food.Size / 2;
		private const int MinSpawnY = 71 + Food.Size / 2;
		private const int MaxSpawnX = 854 - 93 - Food.Size / 2;
		private const int MaxSpawnY = 480 - 71 - Food.Size / 2;

		public ControlFoodSpawner(int first_x, int first_y) {
			Engine.SpawnInstance(new Food(first_x, first_y));
		}

		public ControlFoodSpawner() : this(854 / 2, 480 / 2) {}

		public override void onGameEvent(GameEvent game_event) {
			base.onGameEvent(game_event);
			if (game_event is FoodEatenEvent) {
				this.SpawnFood();
			}
		}

		private void SpawnFood() {
			new_random_position:
			int x = ControlFoodSpawner.MinSpawnX + Engine.Random.Next(ControlFoodSpawner.MaxSpawnX - ControlFoodSpawner.MinSpawnX + 1);
			int y = ControlFoodSpawner.MinSpawnY + Engine.Random.Next(ControlFoodSpawner.MaxSpawnY - ControlFoodSpawner.MinSpawnY + 1);
			if (this.IsPositionInCollision(x, y))
				goto new_random_position;

			Engine.SpawnInstance(new Food(x, y));
		}

		private bool IsPositionInCollision(int x, int y) {
			var rect = new Rectangle(x - Food.Size / 2, y - Food.Size / 2, Food.Size, Food.Size);

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
