using System.Collections.Generic;
using System.Linq;
using MonoEngine;
using Snake.Entities.Controls;
using Snake.Entities.UI;

namespace Snake.Rooms
{
	public class RoomMain : Room
	{
		public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
			Engine.SpawnInstance<TitleLogo>();
			Engine.SpawnInstance<DeveloperLogo>();
			Engine.SpawnInstance<WallDecoration>();
			Engine.SpawnInstance<MainMenuUI>();

#if XBOX_LIVE
			Engine.SpawnInstance<XboxLiveIndicator>();
#endif
		}

		public override void onSwitchAway(Room next_room) {}
	}
}
