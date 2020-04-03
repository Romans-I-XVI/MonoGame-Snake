using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoEngine;
using Snake.SnakeShared;
using Snake.SnakeShared.Enums;

namespace Snake.Entities
{
	public class Snake : Entity
	{
		private readonly ReadOnlyDictionary<GameplaySpeeds, int> MoveSpeeds = new ReadOnlyDictionary<GameplaySpeeds, int>(new Dictionary<GameplaySpeeds, int> {
			[GameplaySpeeds.Slow] = 1,
			[GameplaySpeeds.Medium] = 2,
			[GameplaySpeeds.Fast] = 4,
		});
		private ReadOnlyDictionary<string, List<int>> SnakeLocations = new ReadOnlyDictionary<string, List<int>>(new Dictionary<string, List<int>> {
			["x"] = new List<int>(),
			["y"] = new List<int>(),
		});
		private readonly List<SnakeTail> Tail = new List<SnakeTail>();

		private int CurrentSpeed => this.MoveSpeeds[Settings.CurrentGameplaySpeed];
		private Directions Direction;
		private Vector2 InternalLocation;
		private Point CurrentLocation;

		public Snake() : this(new Point(170 - 8, 480 / 2), Directions.Right) {}

		public Snake(Point position, Directions direction) {
			this.CurrentLocation = position;
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
			this.AddColliderRectangle(Directions.Right.ToString(), texture.Width / 2 - 1, -texture.Height / 2, 1, texture.Height, false);

			for (int i = 0; i < 2; i++) {
				var tail = new SnakeTail {
					Position = new Vector2(this.Position.X - (i + 1) * 16, this.Position.Y),
					Depth = this.Depth + 1 + i
				};
				Engine.SpawnInstance(tail);
				this.Tail.Add(tail);
			}

			for (int i = 0; i < this.Tail.Count * this.CurrentSpeed; i++) {
				this.SnakeLocations["x"].Add(this.CurrentLocation.X - (i * this.CurrentSpeed));
				this.SnakeLocations["y"].Add(this.CurrentLocation.Y);
			}
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);

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

			int previous_x = this.CurrentLocation.X;
			int previous_y = this.CurrentLocation.Y;

			if (this.InternalLocation.X <= this.CurrentLocation.X - this.CurrentSpeed)
				this.CurrentLocation.X -= this.CurrentSpeed;
			else if (this.InternalLocation.X >= this.CurrentLocation.X + this.CurrentSpeed)
				this.CurrentLocation.X += this.CurrentSpeed;

			if (this.InternalLocation.Y <= this.CurrentLocation.Y - this.CurrentSpeed)
				this.CurrentLocation.Y -= this.CurrentSpeed;
			else if (this.InternalLocation.Y >= this.CurrentLocation.Y + this.CurrentSpeed)
				this.CurrentLocation.Y += this.CurrentSpeed;

			if (this.CurrentLocation.X != previous_x || this.CurrentLocation.Y != previous_y) {
				this.SnakeLocations["x"].Insert(0, this.CurrentLocation.X);
				this.SnakeLocations["y"].Insert(0, this.CurrentLocation.Y);
				this.Position = this.CurrentLocation.ToVector2();
			}

			for (int i = 0; i < this.Tail.Count; i++) {
				int j = 16 / this.CurrentSpeed;

				int array_pos = (i + 1) * j;
				if (this.SnakeLocations["x"].Count > array_pos) {
					this.Tail[i].Position.X = this.SnakeLocations["x"][array_pos];
					this.Tail[i].Position.Y = this.SnakeLocations["y"][array_pos];
				}
			}

			while (this.SnakeLocations["x"].Count > this.Tail.Count * (16 / this.CurrentSpeed)) {
				this.SnakeLocations["x"].RemoveAt(this.SnakeLocations["x"].Count - 1);
				this.SnakeLocations["y"].RemoveAt(this.SnakeLocations["y"].Count - 1);
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
					this.TryChangeDirection(Directions.Up);
					break;
				case Keys.S:
					this.TryChangeDirection(Directions.Down);
					break;
				case Keys.A:
					this.TryChangeDirection(Directions.Left);
					break;
				case Keys.D:
					this.TryChangeDirection(Directions.Right);
					break;
			}
		}

		private void TryChangeDirection(Directions direction) {
			if (direction == this.Direction)
				return;

			bool can_change;
			if (direction == Directions.Right)
				can_change = this.Direction != Directions.Left;
			else if (direction == Directions.Left)
				can_change = this.Direction != Directions.Right;
			else if (direction == Directions.Up)
				can_change = this.Direction != Directions.Down;
			else
				can_change = this.Direction != Directions.Up;

			if (can_change) {
				this.Direction = direction;
				this.InternalLocation = this.CurrentLocation.ToVector2();
			}
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
