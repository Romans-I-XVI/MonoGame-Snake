using Microsoft.Xna.Framework;
using MonoEngine;

namespace Snake.Entities.UI
{
	public class ButtonMedium : Button
	{
		public ButtonMedium() : base(Engine.Game.CanvasWidth - 180, Engine.Game.CanvasHeight / 2 + 20, 215, 80) {}

		protected override DrawLocations[] DrawData { get; } = {
			new DrawLocations(DrawDataTextures.Snake, 0.5f, new[] {
				new Vector2(197, 48),
				new Vector2(197, 40),
				new Vector2(197, 32),
				new Vector2(197, 24),
				new Vector2(189, 30),
				new Vector2(184, 38),
				new Vector2(179, 30),
				new Vector2(171, 24),
				new Vector2(171, 32),
				new Vector2(171, 40),
				new Vector2(171, 48),
				new Vector2(159, 24),
				new Vector2(159, 32),
				new Vector2(159, 40),
				new Vector2(152, 48),
				new Vector2(144, 48),
				new Vector2(137, 40),
				new Vector2(137, 32),
				new Vector2(137, 24),
				new Vector2(125, 48),
				new Vector2(117, 48),
				new Vector2(109, 48),
				new Vector2(117, 40),
				new Vector2(117, 32),
				new Vector2(125, 24),
				new Vector2(117, 24),
				new Vector2(109, 24),
				new Vector2(92, 48),
				new Vector2(84, 48),
				new Vector2(99, 40),
				new Vector2(99, 32),
				new Vector2(92, 24),
				new Vector2(84, 24),
				new Vector2(76, 48),
				new Vector2(76, 40),
				new Vector2(76, 32),
				new Vector2(76, 24),
				new Vector2(64, 48),
				new Vector2(56, 48),
				new Vector2(56, 36),
				new Vector2(64, 24),
				new Vector2(56, 24),
				new Vector2(48, 48),
				new Vector2(48, 40),
				new Vector2(48, 32),
				new Vector2(48, 24),
				new Vector2(36, 48),
				new Vector2(36, 40),
				new Vector2(36, 32),
				new Vector2(36, 24),
				new Vector2(28, 30),
				new Vector2(23, 38),
				new Vector2(18, 30),
				new Vector2(10, 24),
				new Vector2(10, 32),
				new Vector2(10, 40),
				new Vector2(10, 48),
			})
		};
	}
}
