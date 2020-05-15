using Microsoft.Xna.Framework.Input;
using MonoEngine;

namespace Snake
{
	public class VirtualInputButtons
	{
		public readonly VirtualButton ButtonLeft = new VirtualButton();
		public readonly VirtualButton ButtonRight = new VirtualButton();
		public readonly VirtualButton ButtonUp = new VirtualButton();
		public readonly VirtualButton ButtonDown = new VirtualButton();
		public readonly VirtualButton ButtonSelect = new VirtualButton();

		public VirtualInputButtons() {
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
	}
}
