#if ADS
using System;
using System.Collections.Generic;
using System.Text;
using MonoEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Snake.Entities.UI;
using Snake.Rooms;
using Microsoft.Xna.Framework.Input;
using Snake.Entities;

namespace Snake
{
	public abstract class Upgrade : Entity
	{
		public static bool IsUpgraded { get; protected set; } = false;
		public bool FetchingIsUpgraded { get; protected set; } = true;
		protected Selector Selector = null;
		protected int SelectorIndex = 0;
		protected Rectangle SelectorRect0 => new Rectangle((int)this.Position.X - 145, (int)this.Position.Y + 80, 120, 80);
		protected Rectangle SelectorRect1 => new Rectangle((int)this.Position.X + 25, (int)this.Position.Y + 80, 120, 80);
		protected int Timout = 10000;
		protected GameTimeSpan TimoutTimer = new GameTimeSpan();
		protected readonly VirtualButton UpgradeButton = new VirtualButton();

		protected Upgrade() {
			this.IsPersistent = true;
			this.Depth = -int.MaxValue + 1;
			this.Position = new Vector2(52 + Wall.Size / 2, 30 + Wall.Size / 2);
			this.UpgradeButton.AddKey(Keys.Help);
		}

		public abstract void DoUpgrade();

		public override void onUpdate(float dt) {
			base.onUpdate(dt);
			if (!(Engine.Room is RoomMain))
				return;

			if (Upgrade.IsUpgraded)
				this.Destroy();
			else if (!this.FetchingIsUpgraded && this.UpgradeButton.IsPressed())
				this.DoUpgrade();
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);
			if (!(Engine.Room is RoomMain))
				return;

			if (!Upgrade.IsUpgraded && !this.FetchingIsUpgraded) {
				if (Settings.CurrentTheme == 6 || Settings.CurrentTheme == 7) {
					Color wall_color;
					if (Settings.CurrentTheme == 6)
						wall_color = new Color(0xdf, 0x5a, 0x1f);
					else
						wall_color = new Color(0x84, 0xb0, 0x4e);
					RectangleDrawer.Draw(sprite_batch, this.Position.X - Wall.Size / 2f, this.Position.Y - Wall.Size / 2f, Wall.Size, Wall.Size, wall_color);
				}

				var background_color = Color.White;
				if (Settings.CurrentTheme < this.BackgroundColors.Length)
					background_color = this.BackgroundColors[Settings.CurrentTheme];
				RectangleDrawer.Draw(sprite_batch, this.Position.X - Wall.Size / 2f + 4, this.Position.Y - Wall.Size / 2f + 4, Wall.Size - 8, Wall.Size - 8, background_color);

				var text_color = Color.Black;
				if (Settings.CurrentTheme < this.FoodColors.Length)
					text_color = this.FoodColors[Settings.CurrentTheme];
				for (int i = 0; i < 3; i++)
					RectangleDrawer.Draw(sprite_batch, this.Position.X - 8, this.Position.Y - 1.5f - 5 + 5 * i, 16, 3, text_color);
				sprite_batch.DrawString(ContentHolder.Get(AvailableFonts.retro_computer), "Upgrade", this.Position + new Vector2(Wall.Size / 2 + 3, 0), text_color, scale: 0.3125f, draw_from: DrawFrom.LeftCenter);
			}
		}

		private readonly Color[] BackgroundColors = {
			new Color(0xe1, 0xfc, 0xd3),
			new Color(0x7f, 0xd0, 0xd6),
			new Color(0xc5, 0xff, 0xfd),
			new Color(0x26, 0x26, 0x26),
			new Color(0x3a, 0x41, 0x44),
			new Color(0xff, 0xe7, 0xbd),
			new Color(0xe9, 0xb8, 0x64),
			new Color(0xd7, 0xff, 0xa6),
			new Color(0x81, 0xc6, 0xdd)
		};

		private readonly Color[] FoodColors = {
			new Color(0x37, 0x53, 0x2a),
			new Color(0x01, 0x6f, 0x82),
			new Color(0xfe, 0x95, 0x00),
			new Color(0xe9, 0x00, 0xfe),
			new Color(0xff, 0xff, 0xff),
			new Color(0xbf, 0x80, 0x4f),
			new Color(0x2e, 0x06, 0x04),
			new Color(0x1a, 0x22, 0x0f),
			new Color(0xe4, 0x87, 0x43)
		};
	}
}
#endif
