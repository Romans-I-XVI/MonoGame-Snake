using Microsoft.Xna.Framework;
using MonoEngine;

namespace Snake
{
	public static class SpriteExtensions
	{
		public static Sprite Copy(this Sprite sprite) {
			Sprite copy;
			if (sprite is AnimatedSprite) {
				var animated_sprite = (AnimatedSprite)sprite;
				var regions = new Region[animated_sprite.Regions.Length];
				for (int i = 0; i < animated_sprite.Regions.Length; i++)
					regions[i] = animated_sprite.Regions[i].Copy();

				copy = new AnimatedSprite(regions, animated_sprite.AnimationSpeed, animated_sprite.ReverseAnimationDirection);
				((AnimatedSprite)copy).Index = animated_sprite.Index;
				((AnimatedSprite)copy).AnimationTween = animated_sprite.AnimationTween;
			} else {
				var region = sprite.Region.Copy();
				copy = new Sprite(region);
			}

			copy.Enabled = sprite.Enabled;
			copy.Alpha = sprite.Alpha;
			copy.Color = sprite.Color;
			copy.Offset = new Vector2(sprite.Offset.X, sprite.Offset.Y);
			copy.Rotation = sprite.Rotation;
			copy.Scale = new Vector2(sprite.Scale.X, sprite.Scale.Y);

			return copy;
		}

		public static Rectangle GetRectangle(this Sprite sprite, Vector2 owner_position) {
			float x = owner_position.X + sprite.Offset.X - sprite.Region.Origin.X * sprite.Scale.X;
			float y = owner_position.Y + sprite.Offset.Y - sprite.Region.Origin.Y * sprite.Scale.Y;
			float width = sprite.Region.GetWidth() * sprite.Scale.X;
			float height = sprite.Region.GetHeight() * sprite.Scale.Y;
			return new Rectangle((int)x, (int)y, (int)width, (int)height);
		}
	}
}
