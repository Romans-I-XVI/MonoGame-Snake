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
		protected readonly VirtualInputButtons Input = new VirtualInputButtons();

		protected Upgrade() {
			this.IsPersistent = true;
			Engine.SetInputLayer(InputLayer.Two);
			this.Input.ButtonUp.InputLayer = InputLayer.Two;
			this.Input.ButtonDown.InputLayer = InputLayer.Two;
			this.Input.ButtonLeft.InputLayer = InputLayer.Two;
			this.Input.ButtonRight.InputLayer = InputLayer.Two;
			this.Input.ButtonSelect.InputLayer = InputLayer.Two;
			this.Depth = -int.MaxValue + 1;
			this.Position.X = Engine.Game.CanvasWidth / 2f;
			this.Position.Y = Engine.Game.CanvasHeight / 2f;
		}

		public abstract void DoUpgrade();

		public override void onUpdate(float dt) {
			base.onUpdate(dt);
			if (!(Engine.Room is RoomMain))
				return;

			if (Upgrade.IsUpgraded) {
				this.Destroy();
			} else if (!this.FetchingIsUpgraded) {
				if (this.Selector == null) {
					this.Selector = new Selector {
						Depth = -int.MaxValue
					};
					Engine.SpawnInstance(this.Selector);
					var rect = this.SelectorRect0;
					this.Selector.Move(rect.X, rect.Y, rect.Width, rect.Height, 0, Tween.LinearTween);
				}

				if (this.Input.ButtonLeft.IsPressed() && this.SelectorIndex > 0) {
					SFXPlayer.Play(AvailableSounds.navsingle);
					var rect = this.SelectorRect0;
					this.Selector.Move(rect.X, rect.Y, rect.Width, rect.Height, 200, Tween.SquareEaseOut);
					this.SelectorIndex--;
				}


				if (this.Input.ButtonRight.IsPressed() && this.SelectorIndex < 1) {
					SFXPlayer.Play(AvailableSounds.navsingle);
					var rect = this.SelectorRect1;
					this.Selector.Move(rect.X, rect.Y, rect.Width, rect.Height, 200, Tween.SquareEaseOut);
					this.SelectorIndex++;
				}

				if (this.Input.ButtonSelect.IsPressed()) {
					if (this.SelectorIndex == 0) {
						SFXPlayer.Play(AvailableSounds.navsingle);
						this.Destroy();
					} else {
						this.DoUpgrade();
					}
				}
			}

			if (this.FetchingIsUpgraded && this.TimoutTimer.TotalMilliseconds > this.Timout)
				this.Destroy();
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);
			if (!(Engine.Room is RoomMain))
				return;

			if (!Upgrade.IsUpgraded) {
				RectangleDrawer.Draw(sprite_batch, new Rectangle(0, 0, Engine.Game.CanvasWidth, Engine.Game.CanvasHeight), Color.Black * (216 / 255f));

				if (this.FetchingIsUpgraded) {
					sprite_batch.DrawString(ContentHolder.Get(AvailableFonts.retro_computer), "Please Wait", this.Position, new Color(0xC9, 0XC9, 0XC9), scale: 1, draw_from: DrawFrom.BottomCenter);
					sprite_batch.DrawString(ContentHolder.Get(AvailableFonts.retro_computer), "Checking  Upgrade  Status", this.Position, new Color(0xC9, 0XC9, 0XC9), scale: 0.5f, draw_from: DrawFrom.TopCenter);
				}
				else {
					var texture = ContentHolder.Get(AvailableTextures.upgrade);
					var pos = this.Position - new Vector2(texture.Width / 2f, texture.Height / 2f);
					sprite_batch.Draw(texture, pos, Color.White);
				}
			}
		}

		public override void onDestroy() {
			base.onDestroy();
			if (this.Selector != null)
				this.Selector.IsExpired = true;
			Engine.SetInputLayer(InputLayer.One);
		}
	}
}
#endif
