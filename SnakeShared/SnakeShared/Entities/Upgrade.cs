#if ADS
using System;
using System.Collections.Generic;
using System.Text;
using MonoEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace Snake
{
    public abstract class Upgrade : Entity
    {
        public static bool IsUpgraded { get; protected set; } = false;

        protected Upgrade() {
            this.Depth = -int.MaxValue + 1;
            this.Position.X = Engine.Game.CanvasWidth / 2f;
            this.Position.Y = Engine.Game.CanvasHeight / 2f;
        }

        public abstract void DoUpgrade();

        public override void onDraw(SpriteBatch sprite_batch) {
            base.onDraw(sprite_batch);
            if (!Upgrade.IsUpgraded) {
                RectangleDrawer.Draw(sprite_batch, new Rectangle(0, 0, Engine.Game.CanvasWidth, Engine.Game.CanvasHeight), Color.Black * (216 / 255f));
                var texture = ContentHolder.Get(AvailableTextures.upgrade);
                var pos = this.Position - new Vector2(texture.Width / 2f, texture.Height / 2f);
                sprite_batch.Draw(texture, pos, Color.White);
            }
        }
    }
}
#endif
