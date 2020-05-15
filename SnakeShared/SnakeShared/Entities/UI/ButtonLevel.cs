using Snake.Enums;

namespace Snake.Entities.UI
{
	public class ButtonLevel : Button
	{
		public readonly GameRooms GameRoom;

		public ButtonLevel(int x, int y, GameRooms game_room) : base(x, y, 150, 84) {
			this.GameRoom = game_room;
		}

		protected override DrawLocations[] DrawData { get; } = new DrawLocations[0];


	}
}
