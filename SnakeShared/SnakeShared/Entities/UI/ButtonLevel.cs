namespace Snake.Entities.UI
{
	public class ButtonLevel : Button
	{
		public ButtonLevel(int x, int y, int width, int height) : base(x, y, width, height) {}

		protected override DrawLocations[] DrawData { get; }
	}
}
