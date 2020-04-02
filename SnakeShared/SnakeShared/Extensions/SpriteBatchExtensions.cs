using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace Snake
{
	public static class SpriteBatchExtensions
	{
		public static void DrawString(this SpriteBatch sprite_batch, SpriteFont sprite_font, string text, Vector2 position, Color color, float rotation = 0, DrawFrom draw_from = DrawFrom.TopLeft, float scale = 1.0f, SpriteEffects effects = SpriteEffects.None, float layer_depth = 0) {
			var origin = sprite_font.DrawFromOrigin(text, draw_from);
			sprite_batch.DrawString(sprite_font, text, position, color, rotation, origin, scale, effects, layer_depth);
		}

		public static void Draw(this SpriteBatch sprite_batch, Region region, Vector2 position, Color color) {
			sprite_batch.Draw(region.Texture, position, region.SourceRectangle, color, 0, region.Origin, Vector2.One, SpriteEffects.None, 0);
		}

		public static void Draw(this SpriteBatch sprite_batch, Region region, Vector2 position, Color color, float rotation, Vector2 scale, SpriteEffects sprite_effects = SpriteEffects.None, float layer_depth = 0) {
			sprite_batch.Draw(region.Texture, position, region.SourceRectangle, color, rotation, region.Origin, scale, sprite_effects, layer_depth);
		}
	}
}
