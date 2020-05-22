using System.Collections.Generic;
using Microsoft.Xna.Framework;
using MonoEngine;
using Snake.Entities.Controls;
using Snake.Enums;
using Snake.Rooms;

namespace Snake.Entities.UI
{
	public class LevelSelectUI : Entity
	{
		private const int ButtonSpreadX = 180;
		private const int ButtonSpreadY = 100;
		private const int UIMovementDuration = 200;
		private const Tween UIMovementTween = Tween.SquareEaseOut;
		private Point Index = Point.Zero;
		private readonly ButtonLevel[][] Buttons;
		private readonly Selector Selector;
		private readonly VirtualInputButtons Input = new VirtualInputButtons();
		public ButtonLevel CurrentButton => this.Buttons[this.Index.Y][this.Index.X];
		public ButtonLevel NextLevelButton {
			get {
				int index_x = this.Index.X + 1;
				int index_y = this.Index.Y;
				if (index_x > this.Buttons[this.Index.Y].Length - 1) {
					index_x = 0;
					index_y += 1;
				}

				if (index_y < this.Buttons.Length && index_x < this.Buttons[index_y].Length) {
					return this.Buttons[index_y][index_x];
				}

				return null;
			}
		}

		public LevelSelectUI() {
			// Create the level buttons
			const int start_x = 82 + 150 / 2;
			const int start_y = 126 + 84 / 2;
			const int count_x = 4;
			const int count_y = 3;
			int level = 1;

			this.Buttons = new ButtonLevel[count_y][];
			for (int y = 0; y < this.Buttons.Length; y++) {
				this.Buttons[y] = new ButtonLevel[count_x];
				for (int x = 0; x < this.Buttons[y].Length; x++) {
					var button = new ButtonLevel(start_x + LevelSelectUI.ButtonSpreadX * x, start_y + LevelSelectUI.ButtonSpreadY * y, (GameRooms)(level - 1));
					Engine.SpawnInstance(button);
					this.Buttons[y][x] = button;
					if (Settings.CurrentGameRoom == button.GameRoom)
						this.Index = new Point(x, y);
					level++;
				}
			}

			// Create the selector
			this.Selector = Engine.SpawnInstance<Selector>();
			this.MoveSelector(0);
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);

			var previous_index = new Point(this.Index.X, this.Index.Y);

			if (this.Input.ButtonLeft.IsPressed() && this.Index.X > 0)
				this.Index.X--;
			if (this.Input.ButtonRight.IsPressed() && this.Index.X < this.Buttons[this.Index.Y].Length - 1)
				this.Index.X++;
			if (this.Input.ButtonUp.IsPressed() && this.Index.Y > 0)
				this.Index.Y--;
			if (this.Input.ButtonDown.IsPressed() && this.Index.Y < this.Buttons.Length - 1)
				this.Index.Y++;

			if (previous_index.X != this.Index.X || previous_index.Y != this.Index.Y) {
				SFXPlayer.Play(AvailableSounds.navsingle);
				this.MoveSelector();
			}

			// Update the current game room based on the current hovered level
			Settings.CurrentGameRoom = this.CurrentButton.GameRoom;

			// Change to the level if select button is pressed
			if (this.Input.ButtonSelect.IsPressed()) {
				if (this.CurrentButton.IsUnlocked) {
					Engine.ChangeRoom<RoomPlay>(new Dictionary<string, object> {
						["start_delay"] = 250
					});
				} else {
					SFXPlayer.Play(AvailableSounds.death_hit, 0.75f / 2);
				}
			}
		}

		private void MoveSelector(int duration = LevelSelectUI.UIMovementDuration) {
			var current_button = this.Buttons[this.Index.Y][this.Index.X];
			float dest_width = current_button.BaseWidth;
			float dest_height = current_button.BaseHeight;
			float dest_x = current_button.DestPosition.X - dest_width / 2;
			float dest_y = current_button.DestPosition.Y - dest_height / 2;
			this.Selector.Move(dest_x, dest_y, dest_width, dest_height, duration, LevelSelectUI.UIMovementTween);
		}
	}
}
