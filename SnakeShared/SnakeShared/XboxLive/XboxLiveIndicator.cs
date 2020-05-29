#if XBOX_LIVE
using MonoEngine;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Text.RegularExpressions;

namespace Snake
{
	public class XboxLiveIndicator : Entity
	{
		private readonly Regex _gamertagRegex = new Regex("[^a-zA-Z0-9 -]");
		const float FontScale = 0.3125f;

		public XboxLiveIndicator() {
			this.Depth = -int.MaxValue + 1;
			this.Position = new Vector2(40 - 15, 22 - 13);
		}

		public override void onKeyDown(KeyboardEventArgs e) {
			base.onKeyDown(e);
			if ((XboxLiveObject.CurrentUser == null || !XboxLiveObject.CurrentUser.IsSignedIn) && e.Key == Keys.X)
				XboxLiveObject.SignIn(false);
		}

		public override void onButtonDown(GamePadEventArgs e) {
			base.onButtonDown(e);
			if ((XboxLiveObject.CurrentUser == null || !XboxLiveObject.CurrentUser.IsSignedIn) && e.Button == Buttons.X)
				XboxLiveObject.SignIn(false);
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);

			if (XboxLiveObject.CurrentUser != null && XboxLiveObject.CurrentUser.IsSignedIn) {
				string gamertag_string = this._gamertagRegex.Replace(XboxLiveObject.CurrentUser.Gamertag, "");
				sprite_batch.DrawString(ContentHolder.Get(AvailableFonts.retro_computer), gamertag_string, this.Position, Color.Black, scale: XboxLiveIndicator.FontScale);
			} else {
				const string sign_in_string = "Sign In [X]";
				sprite_batch.DrawString(ContentHolder.Get(AvailableFonts.retro_computer), sign_in_string, this.Position, Color.Black, scale: XboxLiveIndicator.FontScale);
			}
		}
	}
}
#endif