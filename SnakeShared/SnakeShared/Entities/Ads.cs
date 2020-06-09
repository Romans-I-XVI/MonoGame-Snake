#if ADS
using System;
using System.Collections.Generic;
using System.Text;
using MonoEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public string GameName { get; protected set; }
        public AdState State { get; protected set; }
        public abstract string AdUrl { get; }

        protected const string AdTimeSavePath = "ad_timer.txt";
        protected int _ad_interval = 300000;
        protected int _saved_ad_time = 0;

        public Ads(string game_name)
        {
            State = AdState.Done;
            this.IsPersistent = true;
            this.IsPauseable = false;
            this.GameName = game_name;

            fetchAdInterval("snake");
            _saved_ad_time = loadSavedAdTime();
        }

        public void Check()
        {
            if (!Upgrade.IsUpgraded)
            {
                int current_time = (int)AdTimer.TotalMilliseconds + _saved_ad_time;
                saveCurrentAdTime(current_time);
                System.Diagnostics.Debug.WriteLine("Ads::Check() - current_time = " + current_time + " _ad_interval = " + _ad_interval);
                if (current_time >= _ad_interval)
                {
                    onShowBegin();
                    Show();
                }
            }
        }

        public abstract void Show();

        public virtual void onShowBegin()
        {
            Engine.Pause();
            Utilities.Try(() => Microsoft.Xna.Framework.Media.MediaPlayer.Pause());
        }

        public virtual void onShowEnd()
        {
            Engine.Resume();
            Utilities.Try(() => Microsoft.Xna.Framework.Media.MediaPlayer.Resume());
            _saved_ad_time = 0;
            saveCurrentAdTime(0);
            AdTimer.Mark();
        }

        public override void onDraw(SpriteBatch spriteBatch)
        {
            base.onDraw(spriteBatch);
            if (State == AdState.Loading || State == AdState.Playing || State == AdState.NoAdsFound)
            {
                Rectangle rect = new Rectangle(0, 0, Engine.Game.CanvasWidth, Engine.Game.CanvasHeight);
                RectangleDrawer.Draw(spriteBatch, rect, new Color(0X04, 0X04, 0X04), layerDepth: 0.001f);

                var splash_texture = ContentHolder.Get(AvailableTextures.splash_ad_buffer);
                var splash_position = new Vector2(Engine.Game.CanvasWidth / 2 - splash_texture.Width / 2, 86);
                spriteBatch.Draw(splash_texture, position: splash_position, layerDepth: 0.0001f);

                string text;
                if (State == AdState.Loading)
                {
                    text = "Fetching Ad Data...";
                }
                else if (State == AdState.Playing)
                {
                    text = "Loading Ad";
                }
                else
                {
                    text = "No Ads Found";
                }
                spriteBatch.DrawString(ContentHolder.Get(AvailableFonts.retro_computer), text, new Vector2(Engine.Game.CanvasWidth / 2, Engine.Game.CanvasHeight - 120), new Color(0XCC, 0XCC, 0XCC), draw_from: DrawFrom.TopCenter);
            }
        }

        protected int loadSavedAdTime()
        {
            var loaded_ad_time = SaveDataHandler.LoadData(Ads.AdTimeSavePath);
            if (loaded_ad_time != null)
            {
                return int.Parse(loaded_ad_time);
            }
            else
            {
                saveCurrentAdTime(0);
                return 0;
            }
        }

        protected void saveCurrentAdTime(int time)
        {
            SaveDataHandler.SaveData(time.ToString(), Ads.AdTimeSavePath);
        }

        async protected void fetchAdInterval(string game_name, int default_interval = 300000)
        {
            try
            {
                var httpClient = new System.Net.Http.HttpClient();
                string res = await httpClient.GetStringAsync("http://romansixvigaming.com/ads/firetv_ad_intervals.json");
                var data = JsonConvert.DeserializeObject<Dictionary<string, int>>(res);
                if (data.ContainsKey(game_name))
                    _ad_interval = data[game_name];
            }
            catch
            {
                _ad_interval = default_interval;
            }
        }
    }
}
#endif
