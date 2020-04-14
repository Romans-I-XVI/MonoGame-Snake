using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;

namespace Snake.Entities.UI
{
	public class ButtonTheme : Button
	{
		public readonly int Theme;
		private readonly AvailableTextures _background;
		private readonly AvailableTextures _wall;
		private readonly AvailableTextures _snake;
		private readonly AvailableTextures _food;
		protected override Texture2D BackgroundTexture => ContentHolder.Get(this._background);
		protected override Texture2D WallTexture => ContentHolder.Get(this._wall);
		protected override Texture2D SnakeTexture => ContentHolder.Get(this._snake);
		protected override Texture2D FoodTexture => ContentHolder.Get(this._food);

		public ButtonTheme(int theme) : base(180, Engine.Game.CanvasHeight / 2 + 20, 215, 80) {
			this.Theme = theme;
			this._background = Enum.Parse<AvailableTextures>("theme_" + theme + "_background");
			this._wall = Enum.Parse<AvailableTextures>("theme_" + theme + "_wall");
			this._snake = Enum.Parse<AvailableTextures>("theme_" + theme + "_snake");
			this._food = Enum.Parse<AvailableTextures>("theme_" + theme + "_food");
		}

		protected override DrawLocations[] DrawData { get; } = {
			new DrawLocations(DrawDataTextures.Wall, 2, new[] {new Vector2(22, 40 - 30)}),
			new DrawLocations(DrawDataTextures.Snake, 2, new[] {new Vector2(107, 40 - 16)}),
			new DrawLocations(DrawDataTextures.Food, 2, new[] {new Vector2(167, 40 - 10)})
		};
	}
}
