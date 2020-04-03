using System.Collections.Generic;
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
		}

		public override void onSwitchAway(Room next_room) {
		}
	}
}
