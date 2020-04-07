using Microsoft.Xna.Framework;

namespace Snake.Entities.UI
{
	public class ButtonClassic : Button
	{
		public ButtonClassic() : base(215, 120) {}

		protected override DrawLocations[] DrawData { get; } = {
			new DrawLocations(DrawDataTextures.Food, 0.5f, new [] {
				new Vector2(106, 75)
			}),
			new DrawLocations(DrawDataTextures.Snake, 0.5f, new [] {
				new Vector2(63, 74),
				new Vector2(71, 74),
				new Vector2(79, 74),
			}),
			new DrawLocations(DrawDataTextures.Wall, (1f / 3f), new [] {
				new Vector2(162, 41),
				new Vector2(152, 41),
				new Vector2(142, 41),
				new Vector2(132, 41),
				new Vector2(122, 41),
				new Vector2(112, 41),
				new Vector2(102, 41),
				new Vector2(92, 41),
				new Vector2(82, 41),
				new Vector2(72, 41),
				new Vector2(62, 41),
				new Vector2(52, 41),
				new Vector2(42, 41),
				new Vector2(162, 101),
				new Vector2(152, 101),
				new Vector2(142, 101),
				new Vector2(132, 101),
				new Vector2(122, 101),
				new Vector2(112, 101),
				new Vector2(102, 101),
				new Vector2(92, 101),
				new Vector2(82, 101),
				new Vector2(72, 101),
				new Vector2(62, 101),
				new Vector2(52, 101),
				new Vector2(42, 101),
				new Vector2(32, 81),
				new Vector2(32, 91),
				new Vector2(32, 101),
				new Vector2(32, 71),
				new Vector2(32, 61),
				new Vector2(32, 51),
				new Vector2(32, 41),
				new Vector2(172, 81),
				new Vector2(172, 91),
				new Vector2(172, 101),
				new Vector2(172, 71),
				new Vector2(172, 61),
				new Vector2(172, 51),
				new Vector2(172, 41),
			}),
			new DrawLocations(DrawDataTextures.Snake, 0.4375f, new [] {
				new Vector2(198, 23),
				new Vector2(191, 28),
				new Vector2(184, 28),
				new Vector2(179, 21),
				new Vector2(198, 11),
				new Vector2(184, 7),
				new Vector2(179, 14),
				new Vector2(191, 7),
				new Vector2(169, 28),
				new Vector2(162, 28),
				new Vector2(155, 28),
				new Vector2(162, 21),
				new Vector2(162, 14),
				new Vector2(169, 7),
				new Vector2(162, 7),
				new Vector2(155, 7),
				new Vector2(131, 7),
				new Vector2(138, 7),
				new Vector2(145, 7),
				new Vector2(124, 12),
				new Vector2(131, 18),
				new Vector2(138, 18),
				new Vector2(145, 23),
				new Vector2(138, 28),
				new Vector2(131, 28),
				new Vector2(124, 28),
				new Vector2(100, 7),
				new Vector2(107, 7),
				new Vector2(114, 7),
				new Vector2(93, 12),
				new Vector2(100, 18),
				new Vector2(107, 18),
				new Vector2(114, 23),
				new Vector2(107, 28),
				new Vector2(100, 28),
				new Vector2(93, 28),
				new Vector2(77, 18),
				new Vector2(70, 18),
				new Vector2(83, 28),
				new Vector2(83, 21),
				new Vector2(83, 14),
				new Vector2(76.5f, 7),
				new Vector2(69.5f, 7),
				new Vector2(63, 14),
				new Vector2(63, 21),
				new Vector2(63, 28),
				new Vector2(46, 28),
				new Vector2(53, 28),
				new Vector2(39, 28),
				new Vector2(39, 21),
				new Vector2(39, 14),
				new Vector2(39, 7),
				new Vector2(29, 23),
				new Vector2(22, 28),
				new Vector2(15, 28),
				new Vector2(10, 21),
				new Vector2(29, 11),
				new Vector2(15, 7),
				new Vector2(10, 14),
				new Vector2(22, 7),
			}),
		};
	}
}
