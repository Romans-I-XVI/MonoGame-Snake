using System.Collections.Generic;
using System.Linq;
using MonoEngine;

namespace Snake.Rooms
{
	public class RoomMain : Room
	{
		public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
			Engine.ChangeRoom<RoomPlay>();
		}

		public override void onSwitchAway(Room next_room) {}
	}
}
