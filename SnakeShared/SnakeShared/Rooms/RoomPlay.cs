using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoEngine;
using Snake.Entities;
using Snake.Entities.Controls;

namespace Snake.Rooms
{
	public class RoomPlay : Room
	{
		public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
			int start_delay = 0;
			if (args != null && args.ContainsKey("start_delay")) {
				start_delay = (int)args["start_delay"];
			}

			Engine.SpawnInstance(new TimedExecution(start_delay, () => {
				MediaPlayer.IsRepeating = true;
				MediaPlayer.Play(ContentHolder.Get(AvailableMusic.background_music));
			}));
			Engine.SpawnInstance(new Entities.Snake(start_delay));
			Engine.SpawnInstance<ControlPause>();
			Engine.SpawnInstance<ControlFoodSpawner>();
			Engine.SpawnInstance<Background>();

			Engine.SpawnInstance(new ControlReset());
		}

		public override void onSwitchAway(Room next_room) {
		}

		private class ControlReset : Entity
		{
			public override void onKeyDown(KeyboardEventArgs e) {
				base.onKeyDown(e);

				if (e.Key == Keys.R || e.Key == Keys.Space) {
					Engine.ResetRoom();
				}
			}

			public override void onButtonDown(GamePadEventArgs e) {
				base.onButtonDown(e);

				if (e.Button == Buttons.Y) {
					Engine.ResetRoom();
				}
			}
		}
	}
}
