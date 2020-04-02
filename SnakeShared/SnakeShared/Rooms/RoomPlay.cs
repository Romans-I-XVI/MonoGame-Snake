using System.Collections.Generic;
using MonoEngine;

namespace Snake.Rooms
{
	public class RoomPlay : Room
	{
		public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
			Engine.SpawnInstance<Entities.Snake>();
		}

		public override void onSwitchAway(Room next_room) {
		}
	}
}
