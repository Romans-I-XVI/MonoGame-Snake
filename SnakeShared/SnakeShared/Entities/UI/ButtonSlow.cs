using Microsoft.Xna.Framework;
using MonoEngine;

namespace Snake.Entities.UI
{
	public class ButtonSlow : Button
	{
		public ButtonSlow() : base(Engine.Game.CanvasWidth - 180, Engine.Game.CanvasHeight / 2 + 20, 215, 80) {}

		protected override DrawLocations[] DrawData { get; } = {
			new DrawLocations(DrawDataTextures.Snake, 0.625f, new[] {
				new Vector2(155, 20),
				new Vector2(155, 30),
				new Vector2(155, 40),
				new Vector2(155, 50),
				new Vector2(165, 43),
				new Vector2(171, 33),
				new Vector2(178, 43),
				new Vector2(188, 50),
				new Vector2(188, 40),
				new Vector2(188, 30),
				new Vector2(188, 20),
				new Vector2(112, 30),
				new Vector2(112, 40),
				new Vector2(121, 50),
				new Vector2(131, 50),
				new Vector2(140, 40),
				new Vector2(140, 30),
				new Vector2(131, 20),
				new Vector2(121, 20),
				new Vector2(95, 50),
				new Vector2(85, 50),
				new Vector2(75, 50),
				new Vector2(65, 50),
				new Vector2(65, 40),
				new Vector2(65, 30),
				new Vector2(65, 20),
				new Vector2(29, 20),
				new Vector2(39, 20),
				new Vector2(49, 20),
				new Vector2(19, 28),
				new Vector2(29, 35),
				new Vector2(39, 35),
				new Vector2(49, 43),
				new Vector2(39, 50),
				new Vector2(29, 50),
				new Vector2(19, 50),
			})
		};
	}
}
