using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoEngine;
using Snake.Enums;
using Snake.GameEvents;
using Snake.Rooms;

namespace Snake.Entities
{
	public class Snake : Entity
	{
#region Constructors
		public const int Size = 16;
		public const int SwitchbackBuffer = 0; // This was set to 2 in the original Roku game

		private int CurrentSpeed => this.MoveSpeeds[Settings.CurrentGameplaySpeed];
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

		private States State;
		private readonly string MouthColliderName = "mouth";
		private readonly int InitialWaitDelay = 0;
		private readonly GameTimeSpan InitialWaitTimer = new GameTimeSpan();
		private int DeathPartsDestroyed = 0;
		private float DeathDestroyPartDelay;
		private bool DeathSpawnedScoreboard = false;
		private readonly GameTimeSpan DeathSequenceTimer = new GameTimeSpan();
		private Directions Direction;
		private Vector2 InternalLocation;
		private Point CurrentLocation;
		private Point DirectionChangeLocation;

		public Snake(int start_delay) : this(start_delay, new Point(170 - Snake.Size / 2, Engine.Game.CanvasHeight / 2)) {}

		public Snake(int start_delay, Point position) {
			this.InitialWaitDelay = start_delay;
			this.CurrentLocation = position;
			this.DirectionChangeLocation = this.CurrentLocation;
			this.InternalLocation = position.ToVector2();
			this.Position = this.InternalLocation;
			this.Direction = Directions.Right;
			this.State = (this.InitialWaitDelay == 0) ? States.Alive : States.Waiting;

			var texture = ContentHolder.Get(Settings.CurrentSnake);
			var region = new Region(texture, 0, 0, Snake.Size, Snake.Size, Snake.Size / 2, Snake.Size / 2);
			var sprite = new Sprite(region);
			this.AddSprite("main", sprite);

			this.AddColliderRectangle(Directions.Up.ToString(), -Snake.Size / 2, -Snake.Size / 2, Snake.Size, 1, false);
			this.AddColliderRectangle(Directions.Down.ToString(), -Snake.Size / 2, Snake.Size / 2 - 1, Snake.Size, 1, false);
			this.AddColliderRectangle(Directions.Left.ToString(), -Snake.Size / 2, -Snake.Size / 2, 1, Snake.Size, false);
			this.AddColliderRectangle(Directions.Right.ToString(), Snake.Size / 2 - 1, -Snake.Size / 2, 1, Snake.Size, true);
			this.AddColliderRectangle(this.MouthColliderName, -Snake.Size / 2, -Snake.Size / 2, Snake.Size, Snake.Size, true);

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
#endregion

#region OverrideMethods
		public override void onUpdate(float dt) {
			base.onUpdate(dt);

			// Call the appropriate onUpdate depending on the current state
			if (this.State == States.Waiting)
				this.onUpdate_Waiting(dt);
			else if (this.State == States.Alive)
				this.onUpdate_Alive(dt);
			else
				this.onUpdate_Dead(dt);
		}

		private void onUpdate_Waiting(float dt) {
			// Wait to switch state to Alive
			if (this.InitialWaitTimer.TotalMilliseconds >= this.InitialWaitDelay) {
				this.State = States.Alive;
			}
		}

		private void onUpdate_Alive(float dt) {
			// Process queued input if it exists
			if (this.QueuedInput.Count > 0 && this.IsReadyToChangeDirections()) {
				this.ChangeDirection(this.QueuedInput[0]);
				this.QueuedInput.RemoveAt(0);
			}

			// Update the float position based on current speed and delta time
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

			// If the float position is greater than 1 in any direction update the integer position
			// this is all in order to maintain an up to date array of integer positions with which to update the tail
			while (this.InternalLocation.X <= this.CurrentLocation.X - 1 ||
				this.InternalLocation.X >= this.CurrentLocation.X + 1 ||
				this.InternalLocation.Y <= this.CurrentLocation.Y - 1 ||
				this.InternalLocation.Y >= this.CurrentLocation.Y + 1) {

				// Increment/Decrement current location
				if (this.InternalLocation.X <= this.CurrentLocation.X - 1)
					this.CurrentLocation.X -= 1;
				else if (this.InternalLocation.X >= this.CurrentLocation.X + 1)
					this.CurrentLocation.X += 1;

				if (this.InternalLocation.Y <= this.CurrentLocation.Y - 1)
					this.CurrentLocation.Y -= 1;
				else if (this.InternalLocation.Y >= this.CurrentLocation.Y + 1)
					this.CurrentLocation.Y += 1;

				// Move snake to other side of screen if traversing off screen
				if (this.CurrentLocation.X - Snake.Size / 2 < 0) {
					this.CurrentLocation.X = Engine.Game.CanvasWidth - Snake.Size / 2;
					this.InternalLocation.X = this.CurrentLocation.X;
				}
				else if (this.CurrentLocation.X + Snake.Size / 2 > Engine.Game.CanvasWidth) {
					this.CurrentLocation.X = Snake.Size / 2;
					this.InternalLocation.X = this.CurrentLocation.X;
				}

				if (this.CurrentLocation.Y - Snake.Size / 2 < 0) {
					this.CurrentLocation.Y = Engine.Game.CanvasHeight - Snake.Size / 2;
					this.InternalLocation.Y = this.CurrentLocation.Y;
				}
				else if (this.CurrentLocation.Y + Snake.Size / 2 > Engine.Game.CanvasHeight) {
					this.CurrentLocation.Y = Snake.Size / 2;
					this.InternalLocation.Y = this.CurrentLocation.Y;
				}

				// Add the new integer position to the SnakeLocations array and update the draw Position
				this.SnakeLocations["x"].Insert(0, this.CurrentLocation.X);
				this.SnakeLocations["y"].Insert(0, this.CurrentLocation.Y);
				this.Position = this.CurrentLocation.ToVector2();

				// Post event stating snake incremented its position
				Engine.PostGameEvent(new SnakeMoveEvent(this.CurrentLocation.X, this.CurrentLocation.Y, this.Direction));
			}

			// Update each tail position if position exists in array
			for (int i = 0; i < this.Tail.Count; i++) {
				int array_pos = (i + 1) * Snake.Size;
				if (this.SnakeLocations["x"].Count > array_pos) {
					this.Tail[i].Position.X = this.SnakeLocations["x"][array_pos];
					this.Tail[i].Position.Y = this.SnakeLocations["y"][array_pos];
				}
			}

			// Discard unused positions when position array has grown longer than snake tail
			while (this.SnakeLocations["x"].Count > this.Tail.Count * Snake.Size) {
				this.SnakeLocations["x"].RemoveAt(this.SnakeLocations["x"].Count - 1);
				this.SnakeLocations["y"].RemoveAt(this.SnakeLocations["y"].Count - 1);
			}
		}

		private void onUpdate_Dead(float dt) {
			float current_time = this.DeathSequenceTimer.TotalMilliseconds;
			const int initial_delay = 1000;
			const int blink_duration = 300;
			const int final_delay = 200;
			const int blink_count = 7;
			const int blinking_finish_time = initial_delay + blink_duration * (blink_count - 1) + final_delay;

			// Spawn scoreboard after doing initial pause, blink and delay
			if (!this.DeathSpawnedScoreboard && current_time >= blinking_finish_time - final_delay) {
				Engine.SpawnInstance(new Scoreboard());
				this.DeathSpawnedScoreboard = true;
			}

			// Handle enabling/disabling of sprites while doing blink animation
			if (current_time < blinking_finish_time) {
				for (int i = 0; i < blink_count; i++) {
					if (current_time < initial_delay + blink_duration * i) {
						if (i == 0)
							break;

						bool enable_drawing = (i % 2 == 0);

						this.GetSprite("main").Enabled = enable_drawing;
						foreach (var tail in this.Tail) {
							tail.GetSprite("main").Enabled = enable_drawing;
						}

						break;
					}
				}
			} else {
				// Extra checking that sprites are on in case of hangup
				if (!this.GetSprite("main").Enabled) {
					this.GetSprite("main").Enabled = true;
					foreach (var tail in this.Tail) {
						tail.GetSprite("main").Enabled = true;
					}
				}

				// Handle sequentially destroying each snake part and finally restarting room
				if (current_time >= blinking_finish_time + this.DeathDestroyPartDelay * this.DeathPartsDestroyed) {
					this.DeathPartsDestroyed++;
					SFXPlayer.Play(AvailableSounds.destroy_part, 0.75f);

					if (this.Tail.Count > 0) {
						this.Tail[this.Tail.Count - 1].Destroy();
						this.Tail.RemoveAt(this.Tail.Count - 1);
						Engine.PostGameEvent(new SnakePartDestroyedEvent());
					} else {
						Engine.PostGameEvent(new SnakePartDestroyedEvent());
						Engine.PostGameEvent(new SnakeDestructionDoneEvent());
						Engine.SpawnInstance(new TimedExecution(2000, () => {
							Engine.ChangeRoom<RoomPlay>(new Dictionary<string, object> {
								["start_delay"] = 1000
							});

#if ADS
							var ad_object = Engine.GetFirstInstanceByType<Ads>();
							ad_object?.Check();
#endif
						}));
						this.Destroy();
					}
				}
			}
		}

		public override void onCollision(Collider collider, Collider other_collider, Entity other_instance) {
			base.onCollision(collider, other_collider, other_instance);

			// If the snake is not alive simply return, no collisions should be processed
			if (this.State != States.Alive)
				return;

			// If collided with food add to snake, play sound, post event, and destroy food.
			if (other_instance is Food && collider.Name == this.MouthColliderName) {
				this.AddToSnake();
				SFXPlayer.Play(AvailableSounds.eat, 0.7f);
				Engine.PostGameEvent(new FoodEatenEvent((int)other_instance.Position.X, (int)other_instance.Position.Y));
				other_instance.IsExpired = true;

			// Else if collided with tail stop music, play sound, and begin death sequence
			} else if ((other_instance is SnakeTail || other_instance is Wall) && collider.Name != this.MouthColliderName) {
				MediaPlayer.Stop();
				SFXPlayer.Play(AvailableSounds.death_hit, 0.75f);
				Engine.SpawnInstance(new TimedExecution(1000, () => SFXPlayer.Play(AvailableSounds.death, 0.75f)));
				this.BeginDeath();

			// Else if collided with Portal handle porting to new location
			} else if (other_instance is Portal && collider.Name != this.MouthColliderName) {
				var start_pos = this.CurrentLocation.ToVector2();
				var entrance_portal = (Portal)other_instance;
				var exit_portal = entrance_portal.GetDestination(this.Direction);
				var original_direction = this.Direction;
				this.Direction = entrance_portal.GetDirection(this.Direction);

				// Move the snake to the portal exit and reverse direction if necessary
				float travel_x = exit_portal.Position.X - entrance_portal.Position.X;
				float travel_y = exit_portal.Position.Y - entrance_portal.Position.Y;
				if (this.Direction == original_direction) {
					if (this.Direction == Directions.Up) {
						travel_y -= Portal.Size + Snake.Size;
					} else if (this.Direction == Directions.Down) {
						travel_y += Portal.Size + Snake.Size;
					} else if (this.Direction == Directions.Left) {
						travel_x -= Portal.Size + Snake.Size;
					} else if (this.Direction == Directions.Right) {
						travel_x += Portal.Size + Snake.Size;
					}
				} else {
					foreach (var c in this.Colliders) {
						c.Enabled = (c.Name == this.Direction.ToString() || c.Name == this.MouthColliderName);
					}
				}

				this.InternalLocation += new Vector2(travel_x, travel_y);
				this.Position = this.InternalLocation;
				this.CurrentLocation = this.InternalLocation.ToPoint();

				// Do extra checking to see if snake is immediately colliding with wall when exiting portal and adjust if so
				var head_collider = (ColliderRectangle)this.GetCollider(this.Direction.ToString());
				var walls = Engine.GetAllInstances<Wall>();
				for (int i = 0; i < walls.Count; i++) {
					var wall_collider = (ColliderRectangle)walls[i].GetCollider("main");
					if (CollisionChecking.Check(head_collider.Rectangle, wall_collider.Rectangle)) {
						if (this.Direction == Directions.Right || this.Direction == Directions.Left) {
							if (wall_collider.Rectangle.Center.Y < exit_portal.Position.Y) {
								this.InternalLocation.Y = wall_collider.Rectangle.Bottom + Snake.Size / 2;
							} else {
								this.InternalLocation.Y = wall_collider.Rectangle.Top - Snake.Size / 2;
							}
						} else {
							if (wall_collider.Rectangle.Center.X < exit_portal.Position.X) {
								this.InternalLocation.X = wall_collider.Rectangle.Right + Snake.Size / 2;
							} else {
								this.InternalLocation.X = wall_collider.Rectangle.Left - Snake.Size / 2;
							}
						}

						this.Position = this.InternalLocation;
						this.CurrentLocation = this.InternalLocation.ToPoint();
						break;
					}
				}

				// Spawn a new snake portal traversal animation entity
				var portal_animation = new SnakePortalAnimation(
					start_pos,
					this.CurrentLocation.ToVector2(),
					entrance_portal.Position + new Vector2(Portal.Size / 2f),
					exit_portal.Position + new Vector2(Portal.Size / 2f),
					this.Tail.Count);
				Engine.SpawnInstance(portal_animation);
			}
		}

		public override void onKeyDown(KeyboardEventArgs e) {
			base.onKeyDown(e);

			// Handle all input from keyboard
			switch (e.Key) {
				case Keys.W:
				case Keys.Up:
					this.OnInputDirection(Directions.Up);
					break;
				case Keys.S:
				case Keys.Down:
					this.OnInputDirection(Directions.Down);
					break;
				case Keys.A:
				case Keys.Left:
					this.OnInputDirection(Directions.Left);
					break;
				case Keys.D:
				case Keys.Right:
					this.OnInputDirection(Directions.Right);
					break;
			}
		}

		public override void onButtonDown(GamePadEventArgs e) {
			base.onButtonDown(e);

			// Handle all input from gamepad
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

		public override void onResume(int pause_time) {
			base.onResume(pause_time);
			// Remove pause time from running timers on resume
			this.DeathSequenceTimer.RemoveTime(pause_time);
			this.InitialWaitTimer.RemoveTime(pause_time);
		}
#endregion

#region FunctionalityMethods
		private void OnInputDirection(Directions direction) {
			// Determine if new direction is valid
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

			// Turn immediately if ready, otherwise add to queue
			if (can_change) {
				if (this.IsReadyToChangeDirections()) {
					this.ChangeDirection(direction);
				} else {
					this.QueuedInput.Add(direction);
				}
			}
		}

		private void ChangeDirection(Directions direction) {
			// Change direction, normalize turn position, and update active directional collider
			this.Direction = direction;
			this.InternalLocation = this.CurrentLocation.ToVector2();
			this.DirectionChangeLocation = this.CurrentLocation;

			foreach (var c in this.Colliders) {
				c.Enabled = (c.Name == this.Direction.ToString() || c.Name == this.MouthColliderName);
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

		private void BeginDeath() {
			this.State = States.Dead;
			Engine.PostGameEvent(new SnakeDiedEvent());
			this.DeathDestroyPartDelay = 2000 / (float)(this.Tail.Count + 1);
			if (this.DeathDestroyPartDelay < 1000 / 60f)
				this.DeathDestroyPartDelay = 1000 / 60f;
			this.DeathSequenceTimer.Mark();
		}

		private bool IsReadyToChangeDirections() {
			return (Math.Abs(this.CurrentLocation.X - this.DirectionChangeLocation.X) >= Snake.Size + Snake.SwitchbackBuffer || Math.Abs(this.CurrentLocation.Y - this.DirectionChangeLocation.Y) >= Snake.Size + Snake.SwitchbackBuffer);
		}
#endregion

#region EnumsAndClasses
		internal enum States {
			Waiting,
			Alive,
			Dead
		}

		internal class SnakeTail : Entity
		{
			internal SnakeTail() {
				var texture = ContentHolder.Get(Settings.CurrentSnake);
				var region = new Region(texture, 0, 0, Snake.Size, Snake.Size, Snake.Size / 2, Snake.Size / 2);
				var sprite = new Sprite(region);
				this.AddSprite("main", sprite);
				this.AddColliderRectangle("main", -Snake.Size / 2, -Snake.Size / 2, Snake.Size, Snake.Size);
			}
		}
#endregion
	}
}
