using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace Snake.Entities.UI
{
	public class LevelRequirementsLogo : Entity
	{
		private readonly LevelSelectUI LevelSelectUI;

		public LevelRequirementsLogo(LevelSelectUI level_select_ui) {
			this.LevelSelectUI = level_select_ui;
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);

			int? score_to_unlock_next_level = this.GetScoreToUnlockNextLevel();
			if (score_to_unlock_next_level != null) {
				var texture = ContentHolder.Get(Settings.CurrentSnake);
				const float scale = 0.3125f;

				for (int i = 0; i < LevelRequirementsLogo.BaseTextLocations.Length; i++) {
					var pos = LevelRequirementsLogo.BaseTextLocations[i];
					sprite_batch.Draw(texture, pos, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
				}

				string score_string = score_to_unlock_next_level.ToString();
				const int start_x = 240 + 32 - 10 - 11 - 6;
				const int start_y = 480 - 60 + 5;
				const int spread_x = 23;
				for (int i = 0; i < score_string.Length; i++) {
					int integer = int.Parse(score_string[i].ToString());
					var base_pos = new Vector2(start_x + spread_x * i, start_y);
					foreach (var pos in LogoDrawData.ScoreNumberLocations[integer]) {
						sprite_batch.Draw(texture, base_pos + pos, null, Color.White, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
					}
				}
			}
		}

		private int? GetScoreToUnlockNextLevel() {
			var current_level_button = this.LevelSelectUI.CurrentButton;
			var next_level_button = this.LevelSelectUI.NextLevelButton;
			if (current_level_button.IsUnlocked && next_level_button != null && !next_level_button.IsUnlocked) {
				return Settings.LevelScoreRequirements[(int)current_level_button.GameRoom];
			}

			return null;
		}

		private static readonly Vector2[] BaseTextLocations = new[] {
			new Vector2(641, 440),
			new Vector2(636, 440),
			new Vector2(631, 440),
			new Vector2(626, 440),
			new Vector2(626, 435),
			new Vector2(626, 430),
			new Vector2(626, 425),
			new Vector2(659, 432),
			new Vector2(654, 432),
			new Vector2(664, 440),
			new Vector2(659, 440),
			new Vector2(654, 440),
			new Vector2(649, 440),
			new Vector2(649, 435),
			new Vector2(649, 430),
			new Vector2(664, 425),
			new Vector2(659, 425),
			new Vector2(654, 425),
			new Vector2(649, 425),
			new Vector2(687, 425),
			new Vector2(687, 430),
			new Vector2(685, 435),
			new Vector2(675, 435),
			new Vector2(680, 440),
			new Vector2(673, 430),
			new Vector2(673, 425),
			new Vector2(707, 432),
			new Vector2(702, 432),
			new Vector2(712, 440),
			new Vector2(707, 440),
			new Vector2(702, 440),
			new Vector2(697, 440),
			new Vector2(697, 435),
			new Vector2(697, 430),
			new Vector2(712, 425),
			new Vector2(707, 425),
			new Vector2(702, 425),
			new Vector2(697, 425),
			new Vector2(736, 440),
			new Vector2(731, 440),
			new Vector2(726, 440),
			new Vector2(721, 440),
			new Vector2(721, 435),
			new Vector2(721, 430),
			new Vector2(721, 425),
			new Vector2(599, 440),
			new Vector2(599, 435),
			new Vector2(599, 430),
			new Vector2(606, 425),
			new Vector2(601, 425),
			new Vector2(596, 425),
			new Vector2(591, 425),
			new Vector2(577, 436),
			new Vector2(577, 429),
			new Vector2(572, 433),
			new Vector2(567, 436),
			new Vector2(567, 429),
			new Vector2(582, 425),
			new Vector2(582, 440),
			new Vector2(562, 440),
			new Vector2(562, 425),
			new Vector2(548, 432),
			new Vector2(543, 432),
			new Vector2(553, 440),
			new Vector2(548, 440),
			new Vector2(543, 440),
			new Vector2(538, 440),
			new Vector2(538, 435),
			new Vector2(538, 430),
			new Vector2(553, 425),
			new Vector2(548, 425),
			new Vector2(543, 425),
			new Vector2(538, 425),
			new Vector2(529, 425),
			new Vector2(529, 440),
			new Vector2(529, 430),
			new Vector2(529, 435),
			new Vector2(524, 435),
			new Vector2(519, 430),
			new Vector2(514, 425),
			new Vector2(514, 430),
			new Vector2(514, 435),
			new Vector2(514, 440),
			new Vector2(494, 440),
			new Vector2(489, 437),
			new Vector2(494, 425),
			new Vector2(489, 429),
			new Vector2(484, 433),
			new Vector2(479, 440),
			new Vector2(479, 435),
			new Vector2(479, 430),
			new Vector2(479, 425),
			new Vector2(455, 430),
			new Vector2(455, 435),
			new Vector2(460, 440),
			new Vector2(465, 440),
			new Vector2(470, 437),
			new Vector2(470, 428),
			new Vector2(465, 425),
			new Vector2(460, 425),
			new Vector2(431, 430),
			new Vector2(431, 435),
			new Vector2(436, 440),
			new Vector2(441, 440),
			new Vector2(446, 435),
			new Vector2(446, 430),
			new Vector2(441, 425),
			new Vector2(436, 425),
			new Vector2(423, 440),
			new Vector2(418, 440),
			new Vector2(413, 440),
			new Vector2(408, 440),
			new Vector2(408, 435),
			new Vector2(408, 430),
			new Vector2(408, 425),
			new Vector2(399, 425),
			new Vector2(399, 440),
			new Vector2(399, 430),
			new Vector2(399, 435),
			new Vector2(394, 435),
			new Vector2(389, 430),
			new Vector2(384, 425),
			new Vector2(384, 430),
			new Vector2(384, 435),
			new Vector2(384, 440),
			new Vector2(376, 425),
			new Vector2(376, 430),
			new Vector2(376, 435),
			new Vector2(373, 440),
			new Vector2(368, 440),
			new Vector2(363, 440),
			new Vector2(360, 435),
			new Vector2(360, 430),
			new Vector2(360, 425),
			new Vector2(325, 430),
			new Vector2(325, 435),
			new Vector2(330, 440),
			new Vector2(335, 440),
			new Vector2(340, 435),
			new Vector2(340, 430),
			new Vector2(335, 425),
			new Vector2(330, 425),
			new Vector2(309, 440),
			new Vector2(309, 435),
			new Vector2(309, 430),
			new Vector2(316, 425),
			new Vector2(311, 425),
			new Vector2(306, 425),
			new Vector2(301, 425),
			new Vector2(219, 432),
			new Vector2(214, 432),
			new Vector2(224, 440),
			new Vector2(219, 440),
			new Vector2(214, 440),
			new Vector2(209, 440),
			new Vector2(209, 435),
			new Vector2(209, 430),
			new Vector2(224, 425),
			new Vector2(219, 425),
			new Vector2(214, 425),
			new Vector2(209, 425),
			new Vector2(200, 440),
			new Vector2(200, 435),
			new Vector2(200, 428),
			new Vector2(195, 433),
			new Vector2(190, 433),
			new Vector2(195, 425),
			new Vector2(190, 425),
			new Vector2(185, 440),
			new Vector2(185, 435),
			new Vector2(185, 430),
			new Vector2(185, 425),
			new Vector2(162, 430),
			new Vector2(162, 435),
			new Vector2(167, 440),
			new Vector2(172, 440),
			new Vector2(177, 435),
			new Vector2(177, 430),
			new Vector2(172, 425),
			new Vector2(167, 425),
			new Vector2(138, 430),
			new Vector2(138, 435),
			new Vector2(143, 440),
			new Vector2(148, 440),
			new Vector2(153, 437),
			new Vector2(153, 428),
			new Vector2(148, 425),
			new Vector2(143, 425),
			new Vector2(124, 432),
			new Vector2(119, 432),
			new Vector2(114, 429),
			new Vector2(119, 425),
			new Vector2(124, 425),
			new Vector2(129, 425),
			new Vector2(129, 436),
			new Vector2(124, 440),
			new Vector2(119, 440),
			new Vector2(114, 440),
		};
	}
}
