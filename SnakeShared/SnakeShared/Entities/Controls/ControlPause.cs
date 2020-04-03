using Microsoft.Xna.Framework.Input;
using MonoEngine;

namespace Snake.Entities.Controls
{
	public class ControlPause : Entity
	{
		public ControlPause() {
			this.Depth = -int.MaxValue + 1;
			this.IsPauseable = false;
			this.InputLayer = InputLayer.One | InputLayer.Two | InputLayer.Three | InputLayer.Four | InputLayer.Five;
		}

		public override void onKeyDown(KeyboardEventArgs e) {
			base.onKeyDown(e);
			if (e.Key == Keys.P)
				this.PauseResume();
		}

		public override void onButtonDown(GamePadEventArgs e) {
			base.onButtonDown(e);
			if (e.Button == Buttons.Start)
				this.PauseResume();
		}

		public override void onDestroy() {
			base.onDestroy();
			this.Dispose();
		}

		public override void onChangeRoom(Room previous_room, Room next_room) {
			base.onChangeRoom(previous_room, next_room);
			this.Dispose();
		}

		private void PauseResume() {
#if DEBUG
			var debugger_with_terminal = Engine.GetFirstInstanceByType<DebuggerWithTerminal>();
			if (debugger_with_terminal != null && debugger_with_terminal.ConsoleOpen)
				return;
#endif

			if (!Engine.IsPaused())
				Engine.Pause();
			else
				Engine.Resume();
		}

		private void Dispose() {
			if (Engine.IsPaused())
				Engine.Resume();
		}
	}
}
