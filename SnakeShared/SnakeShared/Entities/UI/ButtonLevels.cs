using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace Snake.Entities.UI
{
	public class ButtonLevels : Button
	{
		public ButtonLevels() : base(Engine.Game.CanvasWidth / 2, Engine.Game.CanvasHeight / 2 + 20, 215, 120) {
			this.ExtraDrawingBegin = sprite_batch => {
				const float extra_rect_alpha = (48 / 255f);
				const float rect_width = 60;
				const float rect_height = 34;
				var scaled_pos = new Vector2(
					this.Position.X - (this.BaseWidth / 2f) * this.Scale,
					this.Position.Y - (this.BaseHeight / 2f) * this.Scale);
				foreach (var lock_location in ButtonLevels.LockLocations) {
					RectangleDrawer.Draw(sprite_batch, scaled_pos.X + lock_location.X * this.Scale, scaled_pos.Y + lock_location.Y * this.Scale, rect_width * this.Scale, rect_height * this.Scale, Color.Black * extra_rect_alpha);
				}
				RectangleDrawer.Draw(sprite_batch, scaled_pos.X + 9 * this.Scale, scaled_pos.Y + 44 * this.Scale, rect_width * this.Scale, rect_height * this.Scale, Color.Black * extra_rect_alpha);
			};
		}

		private DrawLocations[] _drawData;
		protected override DrawLocations[] DrawData {
			get {
				if (this._drawData == null) {
					var draw_data = new List<DrawLocations> {
						new DrawLocations(DrawDataTextures.Snake, 0.4375f, new[] {
							new Vector2(177, 7),
							new Vector2(184, 7),
							new Vector2(191, 7),
							new Vector2(170, 12),
							new Vector2(177, 18),
							new Vector2(184, 18),
							new Vector2(191, 23),
							new Vector2(184, 28),
							new Vector2(177, 28),
							new Vector2(170, 28),
							new Vector2(160, 28),
							new Vector2(153, 28),
							new Vector2(146, 28),
							new Vector2(139, 28),
							new Vector2(139, 21),
							new Vector2(139, 14),
							new Vector2(139, 7),
							new Vector2(130, 28),
							new Vector2(123, 28),
							new Vector2(116, 28),
							new Vector2(123, 18),
							new Vector2(116, 18),
							new Vector2(130, 7),
							new Vector2(123, 7),
							new Vector2(116, 7),
							new Vector2(109, 28),
							new Vector2(109, 21),
							new Vector2(109, 14),
							new Vector2(109, 7),
							new Vector2(99, 7),
							new Vector2(99, 14),
							new Vector2(96, 21),
							new Vector2(89.5f, 28),
							new Vector2(83, 21),
							new Vector2(80, 14),
							new Vector2(80, 7),
							new Vector2(70, 28),
							new Vector2(63, 28),
							new Vector2(56, 28),
							new Vector2(63, 18),
							new Vector2(56, 18),
							new Vector2(70, 7),
							new Vector2(63, 7),
							new Vector2(56, 7),
							new Vector2(49, 28),
							new Vector2(49, 21),
							new Vector2(49, 14),
							new Vector2(49, 7),
							new Vector2(39, 28),
							new Vector2(32, 28),
							new Vector2(25, 28),
							new Vector2(18, 28),
							new Vector2(18, 21),
							new Vector2(18, 14),
							new Vector2(18, 7),
						}),
						new DrawLocations(DrawDataTextures.Snake, 0.25f, new[] {
							new Vector2(9 + 50, 44 + 3),
							new Vector2(9 + 46, 44 + 3),
							new Vector2(9 + 43, 44 + 3),
							new Vector2(9 + 39, 44 + 3),
							new Vector2(9 + 35, 44 + 3),
							new Vector2(9 + 31, 44 + 3),
							new Vector2(9 + 28, 44 + 3),
							new Vector2(9 + 24, 44 + 3),
							new Vector2(9 + 20, 44 + 3),
							new Vector2(9 + 16, 44 + 3),
							new Vector2(9 + 13, 44 + 3),
							new Vector2(9 + 9, 44 + 3),
							new Vector2(9 + 5, 44 + 3),
							new Vector2(9 + 50, 44 + 26),
							new Vector2(9 + 46, 44 + 26),
							new Vector2(9 + 43, 44 + 26),
							new Vector2(9 + 39, 44 + 26),
							new Vector2(9 + 35, 44 + 26),
							new Vector2(9 + 31, 44 + 26),
							new Vector2(9 + 28, 44 + 26),
							new Vector2(9 + 24, 44 + 26),
							new Vector2(9 + 20, 44 + 26),
							new Vector2(9 + 16, 44 + 26),
							new Vector2(9 + 13, 44 + 26),
							new Vector2(9 + 9, 44 + 26),
							new Vector2(9 + 5, 44 + 26),
							new Vector2(9 + 1, 44 + 18),
							new Vector2(9 + 1, 44 + 22),
							new Vector2(9 + 1, 44 + 26),
							new Vector2(9 + 1, 44 + 15),
							new Vector2(9 + 1, 44 + 11),
							new Vector2(9 + 1, 44 + 7),
							new Vector2(9 + 1, 44 + 3),
							new Vector2(9 + 54, 44 + 18),
							new Vector2(9 + 54, 44 + 22),
							new Vector2(9 + 54, 44 + 26),
							new Vector2(9 + 54, 44 + 15),
							new Vector2(9 + 54, 44 + 11),
							new Vector2(9 + 54, 44 + 7),
							new Vector2(9 + 54, 44 + 3),
						}),
						new DrawLocations(DrawDataTextures.Snake, 0.125f, new[] {
							new Vector2(9 + 23, 44 + 22),
							new Vector2(9 + 23, 44 + 22),
							new Vector2(9 + 9, 44 + 18),
							new Vector2(9 + 9, 44 + 20),
							new Vector2(9 + 21, 44 + 22),
							new Vector2(9 + 19, 44 + 22),
							new Vector2(9 + 17, 44 + 22),
							new Vector2(9 + 15, 44 + 22),
							new Vector2(9 + 13, 44 + 22),
							new Vector2(9 + 11, 44 + 22),
							new Vector2(9 + 9, 44 + 22),
							new Vector2(9 + 34, 44 + 22),
							new Vector2(9 + 34, 44 + 22),
							new Vector2(9 + 48, 44 + 18),
							new Vector2(9 + 48, 44 + 20),
							new Vector2(9 + 36, 44 + 22),
							new Vector2(9 + 38, 44 + 22),
							new Vector2(9 + 40, 44 + 22),
							new Vector2(9 + 42, 44 + 22),
							new Vector2(9 + 44, 44 + 22),
							new Vector2(9 + 46, 44 + 22),
							new Vector2(9 + 48, 44 + 22),
							new Vector2(9 + 34, 44 + 9),
							new Vector2(9 + 34, 44 + 9),
							new Vector2(9 + 48, 44 + 13),
							new Vector2(9 + 48, 44 + 11),
							new Vector2(9 + 36, 44 + 9),
							new Vector2(9 + 38, 44 + 9),
							new Vector2(9 + 40, 44 + 9),
							new Vector2(9 + 42, 44 + 9),
							new Vector2(9 + 44, 44 + 9),
							new Vector2(9 + 46, 44 + 9),
							new Vector2(9 + 48, 44 + 9),
							new Vector2(9 + 23, 44 + 9),
							new Vector2(9 + 23, 44 + 9),
							new Vector2(9 + 9, 44 + 13),
							new Vector2(9 + 9, 44 + 11),
							new Vector2(9 + 21, 44 + 9),
							new Vector2(9 + 19, 44 + 9),
							new Vector2(9 + 17, 44 + 9),
							new Vector2(9 + 15, 44 + 9),
							new Vector2(9 + 13, 44 + 9),
							new Vector2(9 + 11, 44 + 9),
							new Vector2(9 + 9, 44 + 9),
						})
					};

					foreach (var lock_image_location in ButtonLevels.LockLocations) {
						var draw_locations = new List<Vector2>();
						foreach (var pos in ButtonLevels.LockPartLocations) {
							draw_locations.Add(lock_image_location + pos);
						}
						draw_data.Add(new DrawLocations(DrawDataTextures.Snake, 0.125f, draw_locations.ToArray()));
					}

					this._drawData = draw_data.ToArray();
				}

				return this._drawData;
			}
		}

		private static readonly Vector2[] LockLocations = {
			new Vector2(147, 82),
			new Vector2(78, 82),
			new Vector2(9, 82),
			new Vector2(147, 44),
			new Vector2(78, 44),
		};

		private static readonly Vector2[] LockPartLocations = {
			new Vector2(23, 26),
			new Vector2(25, 26),
			new Vector2(27, 26),
			new Vector2(31, 26),
			new Vector2(33, 26),
			new Vector2(35, 26),
			new Vector2(21, 24),
			new Vector2(29, 16),
			new Vector2(31, 18),
			new Vector2(27, 18),
			new Vector2(27, 18),
			new Vector2(23, 24),
			new Vector2(25, 24),
			new Vector2(27, 24),
			new Vector2(31, 24),
			new Vector2(33, 24),
			new Vector2(35, 24),
			new Vector2(37, 24),
			new Vector2(21, 22),
			new Vector2(23, 22),
			new Vector2(25, 22),
			new Vector2(27, 22),
			new Vector2(31, 22),
			new Vector2(33, 22),
			new Vector2(35, 22),
			new Vector2(37, 22),
			new Vector2(21, 20),
			new Vector2(23, 20),
			new Vector2(25, 20),
			new Vector2(27, 20),
			new Vector2(31, 20),
			new Vector2(33, 20),
			new Vector2(35, 20),
			new Vector2(37, 20),
			new Vector2(21, 18),
			new Vector2(23, 18),
			new Vector2(25, 18),
			new Vector2(33, 18),
			new Vector2(35, 18),
			new Vector2(37, 18),
			new Vector2(29, 26),
			new Vector2(29, 24),
			new Vector2(21, 16),
			new Vector2(23, 16),
			new Vector2(25, 16),
			new Vector2(27, 16),
			new Vector2(31, 16),
			new Vector2(33, 16),
			new Vector2(35, 16),
			new Vector2(37, 16),
			new Vector2(23, 14),
			new Vector2(25, 14),
			new Vector2(27, 14),
			new Vector2(29, 14),
			new Vector2(31, 14),
			new Vector2(33, 14),
			new Vector2(35, 14),
			new Vector2(35, 12),
			new Vector2(33, 12),
			new Vector2(25, 12),
			new Vector2(23, 12),
			new Vector2(23, 10),
			new Vector2(25, 10),
			new Vector2(35, 10),
			new Vector2(33, 10),
			new Vector2(33, 8),
			new Vector2(31, 8),
			new Vector2(29, 8),
			new Vector2(27, 8),
			new Vector2(31, 6),
			new Vector2(29, 6),
			new Vector2(27, 6),
			new Vector2(25, 8),
		};
	}
}
