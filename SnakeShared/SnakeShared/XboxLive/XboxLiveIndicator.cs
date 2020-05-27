#if XBOX_LIVE
using MonoEngine;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;

namespace Snake
{
    public class XboxLiveIndicator : Entity
    {
        private readonly Regex _gamertagRegex = new Regex("[^a-zA-Z0-9 -]");
        public XboxLiveIndicator()
        {
            Depth = -int.MaxValue + 1;
            Position = new Vector2(5, 0);
        }

        public override void onKeyDown(KeyboardEventArgs e)
        {
            base.onKeyDown(e);
            if ((XboxLiveObject.CurrentUser == null || !XboxLiveObject.CurrentUser.IsSignedIn) && e.Key == Microsoft.Xna.Framework.Input.Keys.X)
                XboxLiveObject.SignIn(false);
        }

        public override void onButtonDown(GamePadEventArgs e)
        {
            base.onButtonDown(e);
            if ((XboxLiveObject.CurrentUser == null || !XboxLiveObject.CurrentUser.IsSignedIn) && e.Button == Microsoft.Xna.Framework.Input.Buttons.X)
                XboxLiveObject.SignIn(false);
        }

        public override void onDraw(SpriteBatch spriteBatch)
        {
            base.onDraw(spriteBatch);

            if (XboxLiveObject.CurrentUser != null && XboxLiveObject.CurrentUser.IsSignedIn)
            {
                string gamertagString = _gamertagRegex.Replace(XboxLiveObject.CurrentUser.Gamertag, "");
                spriteBatch.DrawString(ContentHolder.Get(AvailableFonts.retro_computer), gamertagString, Position, Color.Black);
            }
            else
            {
                SpriteFont signInFont = ContentHolder.Get(AvailableFonts.retro_computer);
                string signInString = "Sign In";
                string iconString = "[X]";
                Vector2 IconPosition = Position + new Vector2(signInFont.MeasureString(signInString).X + 10, 0);
                spriteBatch.DrawString(signInFont, signInString, Position, Color.Black);
                spriteBatch.DrawString(ContentHolder.Get(AvailableFonts.retro_computer), iconString, IconPosition, Color.Black);
            }

        }
    }
}
#endif