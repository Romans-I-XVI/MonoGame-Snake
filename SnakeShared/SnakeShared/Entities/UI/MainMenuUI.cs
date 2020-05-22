using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine;
using Snake.Entities.UI;
using Snake.Enums;
using Snake.Rooms;

namespace Snake.Entities.Controls
{
	public class MainMenuUI : Entity
	{
		internal const int UIMovementDuration = 200;
		internal const Tween UIMovementTween = Tween.SquareEaseOut;
		private int Index = 1;
		private readonly Rolodex[] Rolodexes = new Rolodex[3];
		private readonly VirtualInputButtons Input = new VirtualInputButtons();
		private readonly Selector Selector;

		public MainMenuUI() {
			int rolodex_y = Engine.Game.CanvasHeight / 2 + 20;

			// Spawn themes rolodex of buttons
			var themes = new RolodexEntry[] {
				new RolodexEntry(new ButtonTheme(0), 180, rolodex_y, 1, 0),
				new RolodexEntry(new ButtonTheme(1), 180, rolodex_y + 90, 0.5f, 1),
				new RolodexEntry(new ButtonTheme(2), 180, rolodex_y + 70, 0.4f, 2),
				new RolodexEntry(new ButtonTheme(3), 180, rolodex_y + 44, 0.3f, 3),
				new RolodexEntry(new ButtonTheme(4), 180, rolodex_y + 26, 0.2f, 4),
				// new RolodexEntry(new ButtonTheme(0), 180, rolodex_y + 0, 0.1f, 5),
				new RolodexEntry(new ButtonTheme(5), 180, rolodex_y - 26, 0.2f, 4),
				new RolodexEntry(new ButtonTheme(6), 180, rolodex_y - 44, 0.3f, 3),
				new RolodexEntry(new ButtonTheme(7), 180, rolodex_y - 70, 0.4f, 2),
				new RolodexEntry(new ButtonTheme(8), 180, rolodex_y - 90, 0.5f, 1),
			};
			foreach (var entry in themes) {
				Engine.SpawnInstance(entry.Button);
			}
			this.Rolodexes[0] = new Rolodex(themes, Settings.CurrentTheme);
			this.Rolodexes[0].Collapse(0);

			// Spawn game modes rolodex of buttons
			var game_modes = new RolodexEntry[] {
				new RolodexEntry(new ButtonClassic(), Engine.Game.CanvasWidth / 2, rolodex_y, 1, 0),
				new RolodexEntry(new ButtonOpen(), Engine.Game.CanvasWidth / 2, rolodex_y + 100, 0.5f, 1),
				new RolodexEntry(new ButtonLevels(), Engine.Game.CanvasWidth / 2, rolodex_y - 100, 0.5f, 1),
			};
			foreach (var rolodex_entry in game_modes) {
				Engine.SpawnInstance(rolodex_entry.Button);
			}
			int game_mode_index;
			if (Settings.CurrentGameRoom == GameRooms.Classic)
				game_mode_index = 0;
			else if (Settings.CurrentGameRoom == GameRooms.Open)
				game_mode_index = 1;
			else
				game_mode_index = 2;
			this.Rolodexes[1] = new Rolodex(game_modes, game_mode_index);

			// Spawn speed select rolodex of buttons
			var speeds = new RolodexEntry[] {
				new RolodexEntry(new ButtonMedium(), Engine.Game.CanvasWidth - 180, rolodex_y, 1, 0),
				new RolodexEntry(new ButtonFast(), Engine.Game.CanvasWidth - 180, rolodex_y + 80, 0.5125f, 1),
				new RolodexEntry(new ButtonSlow(), Engine.Game.CanvasWidth - 180, rolodex_y - 80, 0.5125f, 1),
			};
			foreach (var entry in speeds) {
				Engine.SpawnInstance(entry.Button);
			}
			int speed_index;
			if (Settings.CurrentGameplaySpeed == GameplaySpeeds.Slow)
				speed_index = 2;
			else if (Settings.CurrentGameplaySpeed == GameplaySpeeds.Fast)
				speed_index = 1;
			else
				speed_index = 0;
			this.Rolodexes[2] = new Rolodex(speeds, speed_index);
			this.Rolodexes[2].Collapse(0);

			// Spawn the selector and move it instantly to the starting position
			this.Selector = Engine.SpawnInstance<Selector>();
			this.MoveSelector(0);
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);

			// Roll rolodex if up or down is press and handle results
			if (this.Input.ButtonUp.IsPressed() || this.Input.ButtonDown.IsPressed()) {
				if (this.Input.ButtonUp.IsPressed()) {
					SFXPlayer.Play(AvailableSounds.navsingle);
					this.Rolodexes[this.Index].RollDown();
				}

				if (this.Input.ButtonDown.IsPressed()) {
					SFXPlayer.Play(AvailableSounds.navsingle);
					this.Rolodexes[this.Index].RollUp();
				}

				var current_button = this.Rolodexes[this.Index].CurrentButton;
				if (current_button is ButtonFast) {
					Settings.CurrentGameplaySpeed = GameplaySpeeds.Fast;
				} else if (current_button is ButtonMedium) {
					Settings.CurrentGameplaySpeed = GameplaySpeeds.Medium;
				} else if (current_button is ButtonSlow) {
					Settings.CurrentGameplaySpeed = GameplaySpeeds.Slow;
				} else if (current_button is ButtonTheme) {
					Settings.CurrentTheme = ((ButtonTheme)current_button).Theme;
				} else if (current_button is ButtonClassic) {
					Settings.CurrentGameRoom = GameRooms.Classic;
				} else if (current_button is ButtonOpen) {
					Settings.CurrentGameRoom = GameRooms.Open;
				} else if (current_button is ButtonLevels) {
					Settings.CurrentGameRoom = GameRooms.Level1;
				}
			}

			// Move selector and handle rolodex collapse if pressing left/right
			if (this.Input.ButtonRight.IsPressed()) {
				if (this.Index < this.Rolodexes.Length - 1) {
					SFXPlayer.Play(AvailableSounds.navsingle);
					this.Index++;
					this.MoveSelector();
					if (this.Index == 2) {
						this.Rolodexes[2].UnCollapse();
					} else if (this.Index == 1) {
						this.Rolodexes[0].Collapse();
					}
				}
			}

			if (this.Input.ButtonLeft.IsPressed()) {
				if (this.Index > 0) {
					SFXPlayer.Play(AvailableSounds.navsingle);
					this.Index--;
					this.MoveSelector();
					if (this.Index == 1) {
						this.Rolodexes[2].Collapse();
					} else if (this.Index == 0) {
						this.Rolodexes[0].UnCollapse();
					}
				}
			}

			// Handle select pressed if hovering on game mode buttons
			if (this.Input.ButtonSelect.IsPressed()) {
				var current_button = this.Rolodexes[this.Index].CurrentButton;
				if (current_button is ButtonOpen) {
					Engine.ChangeRoom<RoomPlay>();
				} else if (current_button is ButtonClassic) {
					Engine.ChangeRoom<RoomPlay>(new Dictionary<string, object> {
						["start_delay"] = 250
					});
				} else if (current_button is ButtonLevels) {
					SFXPlayer.Play(AvailableSounds.navsingle);
					Engine.ChangeRoom<RoomLevels>();
				}
			}
		}

		public override void onDestroy() {
			base.onDestroy();
			this.Selector.Destroy();
		}

		private void MoveSelector(int duration = MainMenuUI.UIMovementDuration) {
			var current_button = this.Rolodexes[this.Index].CurrentButton;
			float dest_width = current_button.BaseWidth;
			float dest_height = current_button.BaseHeight;
			float dest_x = current_button.DestPosition.X - dest_width / 2;
			float dest_y = current_button.DestPosition.Y - dest_height / 2;
			this.Selector.Move(dest_x, dest_y, dest_width, dest_height, duration, MainMenuUI.UIMovementTween);
		}

		internal class Rolodex
		{
			private readonly float CenterX;
			private readonly float CenterY;
			private readonly int LowestDepth;
			private readonly RolodexEntry[] Entries;
			internal Button CurrentButton => this.Entries[0].Button;

			internal Rolodex(RolodexEntry[] entries, int initialize_to_index = 0) {
				this.Entries = entries;
				this.CenterX = this.Entries[0].X;
				this.CenterY = this.Entries[0].Y;
				this.LowestDepth = this.Entries[0].Depth;
				foreach (var entry in this.Entries) {
					if (entry.Depth > this.LowestDepth)
						this.LowestDepth = entry.Depth;
				}

				for (int i = 0; i < initialize_to_index; i++) {
					this.RollUp();
				}
				this.AdjustAllButtons(0);
			}

			internal void RollDown() {
				var last_button = this.Entries[this.Entries.Length - 1].Button;
				for (int i = this.Entries.Length - 1; i >= 1; i--) {
					this.Entries[i].Button = this.Entries[i - 1].Button;
				}

				this.Entries[0].Button = last_button;
				this.AdjustAllButtons(MainMenuUI.UIMovementDuration);
			}

			internal void RollUp() {
				var first_button = this.Entries[0].Button;
				for (int i = 0; i <= this.Entries.Length - 2; i++) {
					this.Entries[i].Button = this.Entries[i + 1].Button;
				}

				this.Entries[this.Entries.Length - 1].Button = first_button;
				this.AdjustAllButtons(MainMenuUI.UIMovementDuration);
			}

			internal void Collapse(int duration = MainMenuUI.UIMovementDuration) {
				foreach (var entry in this.Entries) {
					entry.Button.Adjust(new Vector2(this.CenterX, this.CenterY), entry.Scale, duration, MainMenuUI.UIMovementTween);
				}
			}

			internal void UnCollapse(int duration = MainMenuUI.UIMovementDuration) {
				this.AdjustAllButtons(duration);
			}

			private void AdjustAllButtons(int duration) {
				foreach (var entry in this.Entries) {
					entry.Button.Adjust(new Vector2(entry.X, entry.Y), entry.Scale, duration, MainMenuUI.UIMovementTween);

					float start_y = entry.Button.StartPosition.Y;
					float dest_y = entry.Button.DestPosition.Y;
					entry.Button.Depth = entry.Depth;
					if (entry.Button.Depth == this.LowestDepth && ((start_y > this.CenterY && dest_y <= this.CenterY) || (start_y < this.CenterY && dest_y >= this.CenterY)))
						entry.Button.Depth += 1;
				}
			}
		}

		internal class RolodexEntry
		{
			internal readonly int X;
			internal readonly int Y;
			internal readonly float Scale;
			internal readonly int Depth;
			internal Button Button;

			internal RolodexEntry(Button button, int x, int y, float scale, int depth) {
				this.Button = button;
				this.X = x;
				this.Y = y;
				this.Scale = scale;
				this.Depth = depth;
			}
		}
	}
}
