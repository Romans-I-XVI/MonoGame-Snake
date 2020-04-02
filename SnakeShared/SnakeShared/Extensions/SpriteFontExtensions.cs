using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace Snake
{
	public static class SpriteFontExtensions
	{
		public static Vector2 DrawFromOrigin(this SpriteFont font, string text, DrawFrom draw_from) {
			Vector2 origin;
			var text_size = font.MeasureString(text);
			switch (draw_from) {
			case DrawFrom.TopLeft:
				origin = Vector2.Zero;
				break;
			case DrawFrom.TopCenter:
				origin = new Vector2(text_size.X / 2, 0);
				break;
			case DrawFrom.TopRight:
				origin = new Vector2(text_size.X, 0);
				break;
			case DrawFrom.BottomLeft:
				origin = new Vector2(0, text_size.Y);
				break;
			case DrawFrom.BottomCenter:
				origin = new Vector2(text_size.X / 2, text_size.Y);
				break;
			case DrawFrom.BottomRight:
				origin = new Vector2(text_size.X, text_size.Y);
				break;
			case DrawFrom.Center:
				origin = new Vector2(text_size.X / 2, text_size.Y / 2);
				break;
			case DrawFrom.RightCenter:
				origin = new Vector2(text_size.X, text_size.Y / 2);
				break;
			case DrawFrom.LeftCenter:
				origin = new Vector2(0, text_size.Y / 2);
				break;
			default:
				origin = Vector2.Zero;
				break;
			}

			return origin;
		}
	}
}
