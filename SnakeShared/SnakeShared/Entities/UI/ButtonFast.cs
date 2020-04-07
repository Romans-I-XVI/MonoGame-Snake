using Microsoft.Xna.Framework;
using MonoEngine;

namespace Snake.Entities.UI
{
	public class ButtonFast : Button
	{
		public ButtonFast() : base(Engine.Game.CanvasWidth - 180, Engine.Game.CanvasHeight / 2 + 20, 215, 80) {}

		protected override DrawLocations[] DrawData { get; } = {
			new DrawLocations(DrawDataTextures.Snake, 0.625f, new[] {
				new Vector2(170, 50),
				new Vector2(170, 40),
				new Vector2(170, 30),
				new Vector2(185, 20),
				new Vector2(175, 20),
				new Vector2(165, 20),
				new Vector2(155, 20),
				new Vector2(119, 20),
				new Vector2(129, 20),
				new Vector2(139, 20),
				new Vector2(109, 28),
				new Vector2(119, 35),
				new Vector2(129, 35),
				new Vector2(139, 43),
				new Vector2(129, 50),
				new Vector2(119, 50),
				new Vector2(109, 50),
				new Vector2(84, 35),
				new Vector2(74, 35),
				new Vector2(93, 50),
				new Vector2(93, 40),
				new Vector2(93, 30),
				new Vector2(83, 20),
				new Vector2(73, 20),
				new Vector2(64, 30),
				new Vector2(64, 40),
				new Vector2(64, 50),
				new Vector2(39, 35),
				new Vector2(29, 35),
				new Vector2(49, 20),
				new Vector2(39, 20),
				new Vector2(29, 20),
				new Vector2(19, 50),
				new Vector2(19, 40),
				new Vector2(19, 30),
				new Vector2(19, 20),
			})
		};
	}
}
