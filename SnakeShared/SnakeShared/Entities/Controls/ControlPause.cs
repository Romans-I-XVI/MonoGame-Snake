using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);

			if (Engine.IsPaused()) {
				var texture = ContentHolder.Get(Settings.CurrentFood);
				for (int i = 0; i < ControlPause.PausedLogoLocations.Length; i++) {
					var pos = ControlPause.PausedLogoLocations[i];
					sprite_batch.Draw(texture, pos, Color.White);
				}
			}
		}

		public override void onKeyDown(KeyboardEventArgs e) {
			base.onKeyDown(e);
			if (e.Key == Keys.P || e.Key == Keys.MediaPlayPause || e.Key == Keys.Space )
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

		public void TryPause() {
			if (!Engine.IsPaused()) {
				this.PauseResume();
			}
		}

		private void PauseResume() {
#if DEBUG
			var debugger_with_terminal = Engine.GetFirstInstanceByType<DebuggerWithTerminal>();
			if (debugger_with_terminal != null && debugger_with_terminal.ConsoleOpen)
				return;
#endif

#if ADS
			var ads_object = Engine.GetFirstInstanceByType<Ads>();
			if (ads_object != null) {
				if (ads_object.State != Ads.AdState.Done) {
					return;
				}
			}
#endif

			if (!Engine.IsPaused()) {
				Engine.Pause();
				if (MediaPlayer.State == MediaState.Playing) {
					MediaPlayer.Pause();
				}
			} else {
				Engine.Resume();
				if (MediaPlayer.State == MediaState.Paused) {
					MediaPlayer.Resume();
				}
			}
		}

		private void Dispose() {
			if (Engine.IsPaused())
				Engine.Resume();
		}

		private static readonly Vector2[] PausedLogoLocations = {
			// P
			new Vector2(273, 230),
			new Vector2(283, 230),
			new Vector2(293, 230),
			new Vector2(263, 250),
			new Vector2(263, 240),
			new Vector2(263, 230),
			new Vector2(263, 220),
			new Vector2(263, 210),
			new Vector2(273, 210),
			new Vector2(283, 210),
			new Vector2(293, 210),
			new Vector2(303, 220),
			// A
			new Vector2(319, 250),
			new Vector2(319, 240),
			new Vector2(359, 250),
			new Vector2(359, 240),
			new Vector2(359, 230),
			new Vector2(359, 220),
			new Vector2(349, 230),
			new Vector2(339, 230),
			new Vector2(329, 230),
			new Vector2(319, 230),
			new Vector2(319, 220),
			new Vector2(349, 210),
			new Vector2(339, 210),
			new Vector2(329, 210),
			// U
			new Vector2(412, 210),
			new Vector2(412, 220),
			new Vector2(412, 230),
			new Vector2(412, 240),
			new Vector2(404, 250),
			new Vector2(394, 250),
			new Vector2(384, 250),
			new Vector2(376, 240),
			new Vector2(376, 230),
			new Vector2(376, 220),
			new Vector2(376, 210),
			// S
			new Vector2(430, 250),
			new Vector2(440, 250),
			new Vector2(450, 250),
			new Vector2(460, 250),
			new Vector2(469, 240),
			new Vector2(460, 230),
			new Vector2(450, 230),
			new Vector2(440, 230),
			new Vector2(430, 220),
			new Vector2(439, 210),
			new Vector2(449, 210),
			new Vector2(459, 210),
			new Vector2(469, 210),
			// E
			new Vector2(516, 230),
			new Vector2(506, 230),
			new Vector2(496, 230),
			new Vector2(526, 250),
			new Vector2(516, 250),
			new Vector2(506, 250),
			new Vector2(496, 250),
			new Vector2(486, 250),
			new Vector2(486, 240),
			new Vector2(486, 230),
			new Vector2(486, 220),
			new Vector2(526, 210),
			new Vector2(516, 210),
			new Vector2(506, 210),
			new Vector2(496, 210),
			new Vector2(486, 210),
			// D
			new Vector2(552, 250),
			new Vector2(562, 250),
			new Vector2(572, 250),
			new Vector2(582, 240),
			new Vector2(582, 230),
			new Vector2(582, 220),
			new Vector2(572, 210),
			new Vector2(562, 210),
			new Vector2(552, 210),
			new Vector2(542, 250),
			new Vector2(542, 240),
			new Vector2(542, 230),
			new Vector2(542, 220),
			new Vector2(542, 210),
		};
	}
}
