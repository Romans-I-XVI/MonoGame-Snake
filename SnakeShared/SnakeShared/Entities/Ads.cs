#if ADS
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using MonoEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Newtonsoft.Json;

namespace Snake
{
	public abstract class Ads : Entity
	{
		public enum AdState
		{
			Done,
			Loading,
			Playing,
			NoAdsFound
		}

		public readonly GameTimeSpan AdTimer = new GameTimeSpan();
		public readonly GameTimeSpan NoAdsFoundTimer = new GameTimeSpan();
		public readonly string GameName;
		public readonly string GameNameInternal;
		public AdState State { get; protected set; }
		public abstract string AdUrl { get; }

		protected const string AdTimeSavePath = "ad_timer.txt";
		protected int _ad_interval = 300000;
		protected int _saved_ad_time;

		protected Ads(string game_name, string game_name_internal) {
			this.Depth = -int.MaxValue;
			this.State = AdState.Done;
			this.IsPersistent = true;
			this.IsPauseable = false;
			this.GameName = game_name;
			this.GameNameInternal = game_name_internal;

			this.FetchAdInterval(game_name_internal);
			this._saved_ad_time = this.LoadSavedAdTime();
		}

		public void Check() {
			if (!Upgrade.IsUpgraded) {
				int current_time = (int)this.AdTimer.TotalMilliseconds + this._saved_ad_time;
				this.SaveCurrentAdTime(current_time);
				Debug.WriteLine("Ads::Check() - current_time = " + current_time + " _ad_interval = " + this._ad_interval);
				if (current_time >= this._ad_interval) {
					this.onShowBegin();
					this.Show();
				}
			}
		}

		public abstract void Show();

		public virtual void onShowBegin() {
			Engine.Pause();
			Utilities.Try(() => MediaPlayer.Pause());
		}

		public virtual void onShowEnd() {
			Engine.Resume();
			Utilities.Try(() => MediaPlayer.Resume());
			this._saved_ad_time = 0;
			this.SaveCurrentAdTime(0);
			this.AdTimer.Mark();
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);
			if (this.State == AdState.Loading || this.State == AdState.Playing || this.State == AdState.NoAdsFound) {
				var rect = new Rectangle(0, 0, Engine.Game.CanvasWidth, Engine.Game.CanvasHeight);
				RectangleDrawer.Draw(sprite_batch, rect, new Color(0X04, 0X04, 0X04), layerDepth: 0.001f);

				var font = ContentHolder.Get(AvailableFonts.retro_computer);
				var font_color = new Color(0XCC, 0XCC, 0XCC);
				var splash_texture = ContentHolder.Get(AvailableTextures.splash_ad_buffer);
				var splash_position = new Vector2(Engine.Game.CanvasWidth / 2 - splash_texture.Width / 2, 57);
				sprite_batch.Draw(splash_texture, splash_position, Color.White);
				sprite_batch.DrawString(font, "Snake Will Be Right Back", splash_position + new Vector2(splash_texture.Width / 2f, 188), font_color, scale: 0.4375f, draw_from: DrawFrom.TopCenter);

				string text;
				if (this.State == AdState.Loading)
					text = "Fetching Ad Data...";
				else if (this.State == AdState.Playing)
					text = "Loading Ad";
				else
					text = "No Ads Found";
				sprite_batch.DrawString(font, text, new Vector2(Engine.Game.CanvasWidth / 2f, Engine.Game.CanvasHeight - 80), font_color, scale: 0.5f, draw_from: DrawFrom.TopCenter);
			}
		}

		protected int LoadSavedAdTime() {
			string loaded_ad_time = SaveDataHandler.LoadData(Ads.AdTimeSavePath);
			if (loaded_ad_time != null) {
				Debug.WriteLine("Ads::LoadSavedAdTime() - loaded_ad_time = " + loaded_ad_time);
				return int.Parse(loaded_ad_time);
			}

			this.SaveCurrentAdTime(0);
			return 0;
		}

		protected void SaveCurrentAdTime(int time) {
			SaveDataHandler.SaveData(time.ToString(), Ads.AdTimeSavePath);
		}

		protected async void FetchAdInterval(string game_name, int default_interval = 300000) {
			try {
				var http_client = new HttpClient();
				string res = await http_client.GetStringAsync("http://romansixvigaming.com/ads/firetv_ad_intervals.json");
				var data = JsonConvert.DeserializeObject<Dictionary<string, int>>(res);
				if (data.ContainsKey(game_name))
					this._ad_interval = data[game_name];
				Debug.WriteLine("Ads::FetchAdInterval() - ad_interval = " + this._ad_interval);
			} catch {
				this._ad_interval = default_interval;
			}
		}
	}
}
#endif
