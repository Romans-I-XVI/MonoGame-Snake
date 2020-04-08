using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoEngine;
using Snake.Entities.UI;
using Snake.Rooms;

namespace Snake.Entities.Controls
{
	public class MainMenuUI : Entity
	{
		private Rolodex[] Rolodexes = new Rolodex[3];
		private int Index = 1;
		private VirtualButton ButtonLeft = new VirtualButton();
		private VirtualButton ButtonRight = new VirtualButton();
		private VirtualButton ButtonUp = new VirtualButton();
		private VirtualButton ButtonDown = new VirtualButton();
		private VirtualButton ButtonSelect = new VirtualButton();

		public MainMenuUI() {
			var themes = new Button[] {
				new ButtonTheme(0),
			};
			foreach (var button in themes) {
				Engine.SpawnInstance(button);
			}
			this.Rolodexes[0] = new Rolodex(themes);

			var game_modes = new Button[] {
				new ButtonOpen()
			};
			foreach (var button in game_modes) {
				Engine.SpawnInstance(button);
			}
			this.Rolodexes[1] = new Rolodex(game_modes);

			var speeds = new Button[] {
				new ButtonFast()
			};
			foreach (var button in speeds) {
				Engine.SpawnInstance(button);
			}
			this.Rolodexes[2] = new Rolodex(speeds);

			this.ButtonLeft.AddKey(Keys.Left);
			this.ButtonLeft.AddKey(Keys.A);
			this.ButtonLeft.AddButton(Buttons.DPadLeft);
			this.ButtonLeft.AddButton(Buttons.LeftThumbstickLeft);

			this.ButtonRight.AddKey(Keys.Right);
			this.ButtonRight.AddKey(Keys.D);
			this.ButtonRight.AddButton(Buttons.DPadRight);
			this.ButtonRight.AddButton(Buttons.LeftThumbstickRight);

			this.ButtonUp.AddKey(Keys.Up);
			this.ButtonUp.AddKey(Keys.W);
			this.ButtonUp.AddButton(Buttons.DPadUp);
			this.ButtonUp.AddButton(Buttons.LeftThumbstickUp);

			this.ButtonDown.AddKey(Keys.Down);
			this.ButtonDown.AddKey(Keys.S);
			this.ButtonDown.AddButton(Buttons.DPadDown);
			this.ButtonDown.AddButton(Buttons.LeftThumbstickDown);

			this.ButtonSelect.AddKey(Keys.Space);
			this.ButtonSelect.AddKey(Keys.Enter);
			this.ButtonSelect.AddButton(Buttons.A);
			this.ButtonSelect.AddButton(Buttons.Start);
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);

			if (this.ButtonUp.IsPressed()) {
				this.Rolodexes[this.Index].IncrementIndex();
			}

			if (this.ButtonDown.IsPressed()) {
				this.Rolodexes[this.Index].DecrementIndex();
			}

			if (this.ButtonRight.IsPressed()) {
				if (this.Index < this.Rolodexes.Length - 1) {
					this.Index++;
				}
			}

			if (this.ButtonLeft.IsPressed()) {
				if (this.Index > 0) {
					this.Index--;
				}
			}

			if (this.ButtonSelect.IsPressed()) {
				var current_button = this.Rolodexes[this.Index].CurrentButton;
				if (current_button is ButtonOpen) {
					Engine.ChangeRoom<RoomPlay>(new Dictionary<string, object> {
						["mode"] = "open"
					});
				} else if (current_button is ButtonClassic) {
					Engine.ChangeRoom<RoomPlay>(new Dictionary<string, object> {
						["mode"] = "classic"
					});
				} else if (current_button is ButtonLevels) {
					Engine.ChangeRoom<RoomLevels>();
				}
			}
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);

			var current_button = this.Rolodexes[this.Index].CurrentButton;
			float width = current_button.BaseWidth;
			float height = current_button.BaseHeight;
			float x = current_button.Position.X - width / 2;
			float y = current_button.Position.Y - height / 2;

			RectangleDrawer.DrawAround(sprite_batch, x, y, width, height, Color.Black * 0.5f, 5);
		}

		internal class Rolodex
		{
			private readonly Button[] Buttons;
			private int Index;
			internal Button CurrentButton => this.Buttons[this.Index];

			internal Rolodex(Button[] buttons, int starting_index = 0) {
				this.Index = starting_index;
				this.Buttons = buttons;
			}

			internal void IncrementIndex() {
				if (this.Index < this.Buttons.Length - 1) {
					this.Index++;
				} else {
					this.Index = 0;
				}
			}

			internal void DecrementIndex() {
				if (this.Index > 0) {
					this.Index--;
				} else {
					this.Index = this.Buttons.Length - 1;
				}
			}
		}
	}
}
