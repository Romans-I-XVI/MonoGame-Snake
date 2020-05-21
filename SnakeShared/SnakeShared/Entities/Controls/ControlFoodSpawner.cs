using Microsoft.Xna.Framework;
using MonoEngine;
using Snake.GameEvents;

namespace Snake.Entities.Controls
{
	public class ControlFoodSpawner : Entity
	{
		private const int SideBuffer = 93;
		private const int TopBottomBuffer = 71;
		private readonly int MinSpawnX = ControlFoodSpawner.SideBuffer + Food.Size / 2;
		private readonly int MinSpawnY = ControlFoodSpawner.TopBottomBuffer + Food.Size / 2;
		private readonly int MaxSpawnX = Engine.Game.CanvasWidth - ControlFoodSpawner.SideBuffer - Food.Size / 2;
		private readonly int MaxSpawnY = Engine.Game.CanvasHeight - ControlFoodSpawner.TopBottomBuffer - Food.Size / 2;

		public ControlFoodSpawner(int first_x, int first_y) {
			Engine.SpawnInstance(new Food(first_x, first_y));
		}

		public ControlFoodSpawner() : this(Engine.Game.CanvasWidth / 2, Engine.Game.CanvasHeight / 2) {}

		public override void onGameEvent(GameEvent game_event) {
			base.onGameEvent(game_event);
			if (game_event is FoodEatenEvent) {
				this.SpawnFood();
			}
		}

		private void SpawnFood() {
			new_random_position:
			int x = this.MinSpawnX + Engine.Random.Next(this.MaxSpawnX - this.MinSpawnX + 1);
			int y = this.MinSpawnY + Engine.Random.Next(this.MaxSpawnY - this.MinSpawnY + 1);
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
