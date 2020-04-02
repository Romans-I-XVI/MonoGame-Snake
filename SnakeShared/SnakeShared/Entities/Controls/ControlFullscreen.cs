using Microsoft.Xna.Framework.Input;
using MonoEngine;

namespace Snake.Entities.Controls
{
	public class ControlFullscreen : Entity
	{
		public ControlFullscreen() {
			this.IsPersistent = true;
			this.IsPauseable = false;
			this.InputLayer = InputLayer.One | InputLayer.Two | InputLayer.Three | InputLayer.Four | InputLayer.Five;
		}

		public override void onKeyDown(KeyboardEventArgs e) {
			base.onKeyDown(e);
			if (e.Key == Keys.F11)
				ControlFullscreen.ToggleFullscreen();
		}

		private static void ToggleFullscreen() {
			Engine.Game.Graphics.ToggleFullScreen();
		}
	}
}
