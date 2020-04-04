using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoEngine;
using Snake.Enums;
using Snake.GameEvents;

namespace Snake.Entities
{
	public class Snake : Entity
	{
		public const int Size = 16;

		private readonly ReadOnlyDictionary<GameplaySpeeds, int> MoveSpeeds = new ReadOnlyDictionary<GameplaySpeeds, int>(new Dictionary<GameplaySpeeds, int> {
			[GameplaySpeeds.Slow] = 1,
			[GameplaySpeeds.Medium] = 2,
			[GameplaySpeeds.Fast] = 4,
		});
		private readonly ReadOnlyDictionary<string, List<int>> SnakeLocations = new ReadOnlyDictionary<string, List<int>>(new Dictionary<string, List<int>> {
			["x"] = new List<int>(),
			["y"] = new List<int>(),
		});
		private readonly List<SnakeTail> Tail = new List<SnakeTail>();
		private readonly List<Directions> QueuedInput = new List<Directions>();

		private bool Alive = true;
		private int CurrentSpeed => this.MoveSpeeds[Settings.CurrentGameplaySpeed];
		private Directions Direction;
		private Vector2 InternalLocation;
		private Point CurrentLocation;
		private Point DirectionChangeLocation;

		public Snake() : this(new Point(170 - Snake.Size / 2, 480 / 2), Directions.Right) {}

		public Snake(Point position, Directions direction) {
			this.CurrentLocation = position;
			this.DirectionChangeLocation = this.CurrentLocation;
			this.InternalLocation = position.ToVector2();
			this.Position = this.InternalLocation;
			this.Direction = direction;

			var texture = ContentHolder.Get(Settings.CurrentSnake);
			var region = new Region(texture, 0, 0, texture.Width, texture.Height, texture.Width / 2, texture.Height / 2);
			var sprite = new Sprite(region);
			this.AddSprite("main", sprite);

			this.AddColliderRectangle(Directions.Up.ToString(), -texture.Width / 2, -texture.Height / 2, texture.Width, 1, false);
			this.AddColliderRectangle(Directions.Down.ToString(), -texture.Width / 2, texture.Height / 2 - 1, texture.Width, 1, false);
			this.AddColliderRectangle(Directions.Left.ToString(), -texture.Width / 2, -texture.Height / 2, 1, texture.Height, false);
			this.AddColliderRectangle(Directions.Right.ToString(), texture.Width / 2 - 1, -texture.Height / 2, 1, texture.Height, true);

			for (int i = 0; i < 2; i++) {
				var tail = new SnakeTail {
					Position = new Vector2(this.Position.X - (i + 1) * Snake.Size, this.Position.Y),
					Depth = this.Depth + 1 + i
				};
				Engine.SpawnInstance(tail);
				this.Tail.Add(tail);
			}

			for (int i = 0; i < this.Tail.Count * Snake.Size; i++) {
				this.SnakeLocations["x"].Add(this.CurrentLocation.X - i);
				this.SnakeLocations["y"].Add(this.CurrentLocation.Y);
			}
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);

			if (this.Alive)
				this.onUpdate_Alive(dt);
			else
				this.onUpdate_Dead(dt);
		}

		private void onUpdate_Alive(float dt) {
			if (this.QueuedInput.Count > 0 && this.IsReadyToChangeDirections()) {
				this.ChangeDirection(this.QueuedInput[0]);
				this.QueuedInput.RemoveAt(0);
			}

			switch (this.Direction) {
				case Directions.Left:
					this.InternalLocation.X -= this.CurrentSpeed * 60 * dt;
					break;
				case Directions.Right:
					this.InternalLocation.X += this.CurrentSpeed * 60 * dt;
					break;
				case Directions.Up:
					this.InternalLocation.Y -= this.CurrentSpeed * 60 * dt;
					break;
				case Directions.Down:
					this.InternalLocation.Y += this.CurrentSpeed * 60 * dt;
					break;
			}

			while (this.InternalLocation.X <= this.CurrentLocation.X - 1 ||
				this.InternalLocation.X >= this.CurrentLocation.X + 1 ||
				this.InternalLocation.Y <= this.CurrentLocation.Y - 1 ||
				this.InternalLocation.Y >= this.CurrentLocation.Y + 1) {

				if (this.InternalLocation.X <= this.CurrentLocation.X - 1)
					this.CurrentLocation.X -= 1;
				else if (this.InternalLocation.X >= this.CurrentLocation.X + 1)
					this.CurrentLocation.X += 1;

				if (this.InternalLocation.Y <= this.CurrentLocation.Y - 1)
					this.CurrentLocation.Y -= 1;
				else if (this.InternalLocation.Y >= this.CurrentLocation.Y + 1)
					this.CurrentLocation.Y += 1;

				if (this.CurrentLocation.X - Snake.Size / 2 < 0) {
					this.CurrentLocation.X = 854 - Snake.Size / 2;
					this.InternalLocation.X = this.CurrentLocation.X;
				}
				else if (this.CurrentLocation.X + Snake.Size / 2 > 854) {
					this.CurrentLocation.X = Snake.Size / 2;
					this.InternalLocation.X = this.CurrentLocation.X;
				}

				if (this.CurrentLocation.Y - Snake.Size / 2 < 0) {
					this.CurrentLocation.Y = 480 - Snake.Size / 2;
					this.InternalLocation.Y = this.CurrentLocation.Y;
				}
				else if (this.CurrentLocation.Y + Snake.Size / 2 > 480) {
					this.CurrentLocation.Y = Snake.Size / 2;
					this.InternalLocation.Y = this.CurrentLocation.Y;
				}

				this.SnakeLocations["x"].Insert(0, this.CurrentLocation.X);
				this.SnakeLocations["y"].Insert(0, this.CurrentLocation.Y);
				this.Position = this.CurrentLocation.ToVector2();
			}

			for (int i = 0; i < this.Tail.Count; i++) {
				int array_pos = (i + 1) * Snake.Size;
				if (this.SnakeLocations["x"].Count > array_pos) {
					this.Tail[i].Position.X = this.SnakeLocations["x"][array_pos];
					this.Tail[i].Position.Y = this.SnakeLocations["y"][array_pos];
				}
			}

			while (this.SnakeLocations["x"].Count > this.Tail.Count * Snake.Size) {
				this.SnakeLocations["x"].RemoveAt(this.SnakeLocations["x"].Count - 1);
				this.SnakeLocations["y"].RemoveAt(this.SnakeLocations["y"].Count - 1);
			}
		}

		private void onUpdate_Dead(float dt) {
			// TODO: Add death sequence
		}

		public override void onCollision(Collider collider, Collider other_collider, Entity other_instance) {
			base.onCollision(collider, other_collider, other_instance);

			if (other_instance is Food) {
				this.AddToSnake();
				Engine.PostGameEvent(new FoodEatenEvent((int)other_instance.Position.X, (int)other_instance.Position.Y));
				other_instance.IsExpired = true;
			} else if (other_instance is SnakeTail) {
				this.Alive = false;
			}
		}

		public override void onKeyDown(KeyboardEventArgs e) {
			base.onKeyDown(e);

#if DEBUG
			if (e.Key == Keys.R) {
				Engine.ResetRoom();
			} else if (e.Key == Keys.Space) {
				this.AddToSnake();
			}
#endif
			switch (e.Key) {
				case Keys.W:
					this.OnInputDirection(Directions.Up);
					break;
				case Keys.S:
					this.OnInputDirection(Directions.Down);
					break;
				case Keys.A:
					this.OnInputDirection(Directions.Left);
					break;
				case Keys.D:
					this.OnInputDirection(Directions.Right);
					break;
			}
		}

		public override void onButtonDown(GamePadEventArgs e) {
			base.onButtonDown(e);
#if DEBUG
			if (e.Button == Buttons.Y) {
				Engine.ResetRoom();
			}
#endif

			switch (e.Button) {
			case Buttons.DPadUp:
				this.OnInputDirection(Directions.Up);
				break;
			case Buttons.DPadDown:
				this.OnInputDirection(Directions.Down);
				break;
			case Buttons.DPadLeft:
				this.OnInputDirection(Directions.Left);
				break;
			case Buttons.DPadRight:
				this.OnInputDirection(Directions.Right);
				break;
			}
		}

		private void OnInputDirection(Directions direction) {
			var previous_direction = (this.QueuedInput.Count > 0) ? this.QueuedInput[this.QueuedInput.Count - 1] : this.Direction;
			if (direction == previous_direction)
				return;

			bool can_change;
			if (direction == Directions.Right)
				can_change = previous_direction != Directions.Left;
			else if (direction == Directions.Left)
				can_change = previous_direction != Directions.Right;
			else if (direction == Directions.Up)
				can_change = previous_direction != Directions.Down;
			else
				can_change = previous_direction != Directions.Up;

			if (can_change) {
				if (this.IsReadyToChangeDirections()) {
					this.ChangeDirection(direction);
				} else {
					this.QueuedInput.Add(direction);
				}
			}
		}

		private void ChangeDirection(Directions direction) {
			this.Direction = direction;
			this.InternalLocation = this.CurrentLocation.ToVector2();
			this.DirectionChangeLocation = this.CurrentLocation;

			foreach (var collider in this.Colliders) {
				collider.Enabled = false;
			}
			this.GetCollider(direction.ToString()).Enabled = true;
		}

		private void AddToSnake() {
			var previous_tail = this.Tail[this.Tail.Count - 1];
			var tail = new SnakeTail() {
				Position = previous_tail.Position,
				Depth = previous_tail.Depth + 1
			};
			Engine.SpawnInstance(tail);
			this.Tail.Add(tail);
		}

		private bool IsReadyToChangeDirections() {
			return (Math.Abs(this.CurrentLocation.X - this.DirectionChangeLocation.X) >= Snake.Size + 2 || Math.Abs(this.CurrentLocation.Y - this.DirectionChangeLocation.Y) >= Snake.Size + 2);
		}

		internal class SnakeTail : Entity
		{
			internal SnakeTail() {
				var texture = ContentHolder.Get(Settings.CurrentSnake);
				var region = new Region(texture, 0, 0, texture.Width, texture.Height, texture.Width / 2, texture.Height / 2);
				var sprite = new Sprite(region);
				this.AddSprite("main", sprite);
				this.AddColliderRectangle("main", -texture.Width / 2, -texture.Height / 2, texture.Width, texture.Height);
			}
		}
	}
}
