using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using MonoEngine;
using Snake.Entities;
using Snake.Entities.Controls;

namespace Snake.Rooms
{
	public class RoomPlay : Room
	{
		public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
			Engine.SpawnInstance<Entities.Snake>();
			Engine.SpawnInstance<ControlPause>();
			Engine.SpawnInstance<ControlFoodSpawner>();
			Engine.SpawnInstance<Background>();

#if DEBUG
			Engine.SpawnInstance(new ControlReset());
#endif
		}

		public override void onSwitchAway(Room next_room) {
		}

		private class ControlReset : Entity
		{
			public override void onKeyDown(KeyboardEventArgs e) {
				base.onKeyDown(e);

				if (e.Key == Keys.R) {
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
