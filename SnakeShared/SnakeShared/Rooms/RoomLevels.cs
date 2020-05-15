using System.Collections.Generic;
using MonoEngine;
using Snake.Entities.UI;

namespace Snake.Rooms
{
	public class RoomLevels : Room
	{
		public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
			Engine.SpawnInstance<TitleLogo>();
			Engine.SpawnInstance<WallDecoration>();
			var level_select_ui = new LevelSelectUI();
			Engine.SpawnInstance(level_select_ui);
			var level_requirements_logo = new LevelRequirementsLogo(level_select_ui);
			Engine.SpawnInstance(level_requirements_logo);
		}

		public override void onSwitchAway(Room next_room) {}
	}
}
