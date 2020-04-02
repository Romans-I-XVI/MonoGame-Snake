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

		private int CurrentSpeed => this.MoveSpeeds[Settings.CurrentGameplaySpeed];
		private Directions Direction;
		private Point CurrentLocation;

		public Snake() : this(new Point(170 - 8, 480 / 2), Directions.Right) {}

		public Snake(Point position, Directions direction) {
			this.CurrentLocation = position;
			this.Position = position.ToVector2();
			this.Direction = direction;

			var texture = ContentHolder.Get(Settings.CurrentSnake);
			var region = new Region(texture, 0, 0, texture.Width, texture.Height, texture.Width / 2, texture.Height / 2);
			var sprite = new Sprite(region);
			this.AddSprite("main", sprite);

			this.AddColliderRectangle(Directions.Up.ToString(), -texture.Width / 2, -texture.Height / 2, texture.Width, 1, false);
			this.AddColliderRectangle(Directions.Down.ToString(), -texture.Width / 2, texture.Height / 2 - 1, texture.Width, 1, false);
			this.AddColliderRectangle(Directions.Left.ToString(), -texture.Width / 2, -texture.Height / 2, 1, texture.Height, false);
			this.AddColliderRectangle(Directions.Right.ToString(), texture.Width / 2 - 1, -texture.Height / 2, 1, texture.Height, false);
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);

			switch (this.Direction) {
				case Directions.Left:
					this.Position.X -= this.CurrentSpeed * 60 * dt;
					break;
				case Directions.Right:
					this.Position.X += this.CurrentSpeed * 60 * dt;
					break;
				case Directions.Up:
					this.Position.Y -= this.CurrentSpeed * 60 * dt;
					break;
				case Directions.Down:
					this.Position.Y += this.CurrentSpeed * 60 * dt;
					break;
			}

			if ((int)this.Position.X != this.CurrentLocation.X || (int)this.Position.Y != this.CurrentLocation.Y) {
				this.CurrentLocation = new Point((int)this.Position.X, (int)this.Position.Y);

				// this.SnakeLocations["x"].Insert(0, this.CurrentLocation.X);
				// this.SnakeLocations["y"].Insert(0, this.CurrentLocation.Y);
			}
		}

		public override void onKeyDown(KeyboardEventArgs e) {
			base.onKeyDown(e);

			switch (e.Key) {
				case Keys.W:
					this.Direction = Directions.Up;
					break;
				case Keys.S:
					this.Direction = Directions.Down;
					break;
				case Keys.A:
					this.Direction = Directions.Left;
					break;
				case Keys.D:
					this.Direction = Directions.Right;
					break;
			}
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
