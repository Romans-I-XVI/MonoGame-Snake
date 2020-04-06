using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace Snake.Entities.UI
{
	public class DeveloperLogo : Entity
	{
		public DeveloperLogo() {
			this.Depth = -int.MaxValue + 2;
			this.Position.X = 150;
			this.Position.Y = 425;
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);

			var texture = ContentHolder.Get(Settings.CurrentFood);
			var scale = new Vector2(5 / (float)texture.Width);

			for (int i = 0; i < DeveloperLogo.DeveloperLogoLocations.Length; i++) {
				var pos = DeveloperLogo.DeveloperLogoLocations[i];
				sprite_batch.Draw(texture, this.Position + pos, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
			}
		}

    private static readonly Vector2[] DeveloperLogoLocations = new[] {
		// D
		new Vector2(1, 5),
		new Vector2(1, 10),
		new Vector2(1, 15),
		new Vector2(6, 15),
		new Vector2(11, 15),
		new Vector2(16, 10),
		new Vector2(16, 5),
		new Vector2(11, 0),
		new Vector2(6, 0),
		new Vector2(1, 0),
		// E
		new Vector2(35, 7),
		new Vector2(30, 7),
		new Vector2(40, 15),
		new Vector2(35, 15),
		new Vector2(30, 15),
		new Vector2(25, 15),
		new Vector2(25, 10),
		new Vector2(25, 5),
		new Vector2(40, 0),
		new Vector2(35, 0),
		new Vector2(30, 0),
		new Vector2(25, 0),
		// V
		new Vector2(63, 0),
		new Vector2(63, 5),
		new Vector2(61, 10),
		new Vector2(51, 10),
		new Vector2(56, 15),
		new Vector2(49, 5),
		new Vector2(49, 0),
		// E
		new Vector2(83, 7),
		new Vector2(78, 7),
		new Vector2(88, 15),
		new Vector2(83, 15),
		new Vector2(78, 15),
		new Vector2(73, 15),
		new Vector2(73, 10),
		new Vector2(73, 5),
		new Vector2(88, 0),
		new Vector2(83, 0),
		new Vector2(78, 0),
		new Vector2(73, 0),
		// L
		new Vector2(112, 15),
		new Vector2(107, 15),
		new Vector2(102, 15),
		new Vector2(97, 15),
		new Vector2(97, 10),
		new Vector2(97, 5),
		new Vector2(97, 0),
		// O
		new Vector2(120, 5),
		new Vector2(120, 10),
		new Vector2(125, 15),
		new Vector2(130, 15),
		new Vector2(135, 10),
		new Vector2(135, 5),
		new Vector2(130, 0),
		new Vector2(125, 0),
		// P
		new Vector2(145, 15),
		new Vector2(145, 10),
		new Vector2(150, 8),
		new Vector2(155, 8),
		new Vector2(160, 4),
		new Vector2(155, 0),
		new Vector2(150, 0),
		new Vector2(145, 5),
		new Vector2(145, 0),
		// E
		new Vector2(184, 15),
		new Vector2(179, 15),
		new Vector2(174, 15),
		new Vector2(179, 7),
		new Vector2(174, 7),
		new Vector2(169, 15),
		new Vector2(169, 10),
		new Vector2(169, 5),
		new Vector2(184, 0),
		new Vector2(179, 0),
		new Vector2(174, 0),
		new Vector2(169, 0),
		// D
		new Vector2(208, 5),
		new Vector2(208, 10),
		new Vector2(203, 15),
		new Vector2(198, 15),
		new Vector2(193, 15),
		new Vector2(193, 10),
		new Vector2(193, 5),
		new Vector2(203, 0),
		new Vector2(198, 0),
		new Vector2(193, 0),
		// B
		new Vector2(243, 11),
		new Vector2(243, 3),
		new Vector2(238, 7),
		new Vector2(233, 7),
		new Vector2(238, 15),
		new Vector2(233, 15),
		new Vector2(238, 0),
		new Vector2(233, 0),
		new Vector2(228, 15),
		new Vector2(228, 10),
		new Vector2(228, 5),
		new Vector2(228, 0),
		// Y
		new Vector2(266, 0),
		new Vector2(262, 5),
		new Vector2(256, 5),
		new Vector2(259, 10),
		new Vector2(259, 15),
		new Vector2(252, 0),
		// :
		new Vector2(276, 14),
		new Vector2(276, 3),
		// R
		new Vector2(311, 15),
		new Vector2(311, 10),
		new Vector2(311, 3),
		new Vector2(306, 8),
		new Vector2(301, 8),
		new Vector2(306, 0),
		new Vector2(301, 0),
		new Vector2(296, 15),
		new Vector2(296, 10),
		new Vector2(296, 5),
		new Vector2(296, 0),
		// O
		new Vector2(320, 5),
		new Vector2(320, 10),
		new Vector2(325, 15),
		new Vector2(330, 15),
		new Vector2(335, 10),
		new Vector2(335, 5),
		new Vector2(330, 0),
		new Vector2(325, 0),
		// M
		new Vector2(360, 15),
		new Vector2(360, 10),
		new Vector2(360, 5),
		new Vector2(360, 0),
		new Vector2(355, 3),
		new Vector2(352, 8),
		new Vector2(349, 3),
		new Vector2(344, 15),
		new Vector2(344, 10),
		new Vector2(344, 5),
		new Vector2(344, 0),
		// A
		new Vector2(383, 15),
		new Vector2(383, 10),
		new Vector2(368, 10),
		new Vector2(368, 15),
		new Vector2(373, 7),
		new Vector2(378, 7),
		new Vector2(383, 5),
		new Vector2(368, 5),
		new Vector2(378, 0),
		new Vector2(373, 0),
		// N
		new Vector2(407, 0),
		new Vector2(407, 15),
		new Vector2(407, 5),
		new Vector2(407, 10),
		new Vector2(402, 10),
		new Vector2(397, 5),
		new Vector2(392, 0),
		new Vector2(392, 5),
		new Vector2(392, 10),
		new Vector2(392, 15),
		// S
		new Vector2(426, 7),
		new Vector2(421, 7),
		new Vector2(416, 4),
		new Vector2(421, 0),
		new Vector2(426, 0),
		new Vector2(431, 0),
		new Vector2(431, 11),
		new Vector2(426, 15),
		new Vector2(421, 15),
		new Vector2(416, 15),
		// I
		new Vector2(466, 15),
		new Vector2(461, 15),
		new Vector2(456, 15),
		new Vector2(451, 15),
		new Vector2(458, 10),
		new Vector2(458, 5),
		new Vector2(466, 0),
		new Vector2(461, 0),
		new Vector2(456, 0),
		new Vector2(451, 0),
		// X
		new Vector2(501, 11),
		new Vector2(501, 4),
		new Vector2(496, 8),
		new Vector2(491, 11),
		new Vector2(491, 4),
		new Vector2(506, 0),
		new Vector2(506, 15),
		new Vector2(486, 15),
		new Vector2(486, 0),
		// V
		new Vector2(530, 0),
		new Vector2(530, 5),
		new Vector2(527, 10),
		new Vector2(523, 15),
		new Vector2(519, 10),
		new Vector2(516, 5),
		new Vector2(516, 0),
		// I
		new Vector2(555, 15),
		new Vector2(550, 15),
		new Vector2(545, 15),
		new Vector2(540, 15),
		new Vector2(547, 10),
		new Vector2(547, 5),
		new Vector2(555, 0),
		new Vector2(550, 0),
		new Vector2(545, 0),
		new Vector2(540, 0),
    };
  }
}
