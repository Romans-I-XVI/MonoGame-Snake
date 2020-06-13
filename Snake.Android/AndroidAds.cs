#if ADS
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Google.Ads.Interactivemedia.V3.Api;
using Com.Google.Ads.Interactivemedia.V3.Api.Player;
using MonoEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Snake.Entities;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Snake
{
	public class AdEventListener : Java.Lang.Object, IAdErrorEventAdErrorListener, IAdEventAdEventListener, IAdsLoaderAdsLoadedListener
	{
		internal AndroidAds AndroidAds;

		internal AdEventListener(AndroidAds android_ads) {
			this.AndroidAds = android_ads;
		}

		public void OnAdError(IAdErrorEvent error_event) {
			this.AndroidAds.OnAdError(error_event);
		}

		public void OnAdEvent(IAdEvent ad_event) {
			this.AndroidAds.OnAdEvent(ad_event);
		}

		public void OnAdsManagerLoaded(IAdsManagerLoadedEvent loaded_event) {
			this.AndroidAds.OnAdsManagerLoaded(loaded_event);
		}
	}

	public class AndroidAds : Ads
	{
		public static Context Context;
		public static ViewGroup ViewGroup;
		public static IAdsManager AdsManager;
		private const string CustomGUIDSavePath = "CustomAndroidGUID.txt";

		private string _adUrl;
		public override string AdUrl => this._adUrl;

		private GameTimeSpan GetUrlTimer = new GameTimeSpan();
		private float GetUrlRetryDelay = 5000;
		private ImaSdkFactory mSdkFactory;
		private IAdDisplayContainer mAdDisplayContainer;
		private AdEventListener mAdEventListener;
		private bool IsAdPaused = false;

		public AndroidAds() : base("Snake", "snake") {
			this.mAdEventListener = new AdEventListener(this);
			this.mSdkFactory = ImaSdkFactory.Instance;
			this.mAdDisplayContainer = this.mSdkFactory.CreateAdDisplayContainer();
			this.mAdDisplayContainer.AdContainer = AndroidAds.ViewGroup;
		}

		public override void onUpdate(float dt)	{
			base.onUpdate(dt);
			if (string.IsNullOrEmpty(this.AdUrl) && this.GetUrlTimer.TotalMilliseconds >= this.GetUrlRetryDelay)
			{
				this.GetAdUrl(this.GameNameInternal);
				this.GetUrlTimer.Mark();
			}
		}

		public override void onButtonDown(GamePadEventArgs e) {
			base.onButtonDown(e);
			if (e.Button == Buttons.Start)
				this.PauseResume();
		}

		public override void onKeyDown(KeyboardEventArgs e) {
			base.onKeyDown(e);
			if (e.Key == Keys.P || e.Key == Keys.Pause || e.Key == Keys.MediaPlayPause)
				this.PauseResume();
		}

		public override void Show() {
			if (string.IsNullOrEmpty(this.AdUrl))
				return;

			this.State = AdState.Loading;
			var mAdsLoader = this.mSdkFactory.CreateAdsLoader(AndroidAds.Context);
			mAdsLoader.AddAdErrorListener(this.mAdEventListener);
			mAdsLoader.AddAdsLoadedListener(this.mAdEventListener);

			var request = this.mSdkFactory.CreateAdsRequest();
			request.AdTagUrl = this.AdUrl;
			request.AdDisplayContainer = this.mAdDisplayContainer;
			mAdsLoader.RequestAds(request);
		}

		private int GetDoNotTrack() {
			int dnt = 0;

			try {
				var cr = Application.Context.ContentResolver;
				dnt = Android.Provider.Settings.Secure.GetInt(cr, "limit_ad_tracking");
			} catch {}

			return dnt;
		}

		private string GetAdvertiserID() {
			string id = null;
			var cr = Application.Context.ContentResolver;

			// If do not track is on return empty advertiser id
			if (this.GetDoNotTrack() == 1) return "00000000-0000-0000-0000-000000000000";

			// First try to get the advertising_id (from on FireTV) if available
			try {
				id = Android.Provider.Settings.Secure.GetString(cr, "advertising_id");
			} catch {}

			// Next use android_id if available
			if (string.IsNullOrWhiteSpace(id))
				try {
					id = Android.Provider.Settings.Secure.GetString(cr, Android.Provider.Settings.Secure.AndroidId);
				} catch {}

			// Next use Serial if available (exists on API Level 9)
			if (string.IsNullOrWhiteSpace(id)) id = Build.Serial;

			// Finally if we still don't have an ad id generate a uuid and save it to file.
			if (string.IsNullOrWhiteSpace(id)) {
				id = SaveDataHandler.LoadData(AndroidAds.CustomGUIDSavePath);
				if (string.IsNullOrWhiteSpace(id)) {
					id = Guid.NewGuid().ToString();
					SaveDataHandler.SaveData(id, AndroidAds.CustomGUIDSavePath);
				}
			}

			return id;
		}
		private async Task<string> GetPublicIp() {
			try	{
				var http_client = new System.Net.Http.HttpClient();
				string res = await http_client.GetStringAsync("http://api.ipify.org");
				return res;
			} catch	{
				return "";
			}
		}

		private string GetUserAgent() {
			return Android.Webkit.WebSettings.GetDefaultUserAgent(Application.Context);
		}

		private async void GetAdUrl(string game_name) {
			try {
				var http_client = new HttpClient();
				string res = await http_client.GetStringAsync("http://romansixvigaming.com/ads/firetv_ad_urls.json");
				var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(res);
				if (data.ContainsKey(game_name)) this._adUrl = data[game_name];

				if (this._adUrl.Contains("ADS_EXTERNAL_IP")) {
					string external_ip = await this.GetPublicIp();
					this._adUrl = this._adUrl.Replace("ADS_EXTERNAL_IP", external_ip);
				}
				this._adUrl = this._adUrl.Replace("ADS_USER_AGENT", System.Net.WebUtility.UrlEncode(this.GetUserAgent()));
				this._adUrl = this._adUrl.Replace("ADS_DISPLAY_WIDTH", this.mAdDisplayContainer.AdContainer.Width.ToString());
				this._adUrl = this._adUrl.Replace("ADS_DISPLAY_HEIGHT", this.mAdDisplayContainer.AdContainer.Height.ToString());
				this._adUrl = this._adUrl.Replace("ADS_LIMIT_TRACKING", this.GetDoNotTrack().ToString());
				this._adUrl = this._adUrl.Replace("ADS_TRACKING_ID", this.GetAdvertiserID());
				System.Diagnostics.Debug.WriteLine("Set Ad Url: " + this._adUrl);
			} catch {
				this._adUrl = null;
			}
		}

		private void Finish() {
			if (AndroidAds.AdsManager != null) {
				AndroidAds.AdsManager.Destroy();
				AndroidAds.AdsManager = null;
			}

			this.State = AdState.Done;
			this.AdTimer.Mark();
			this.onShowEnd();
		}

		private void PauseResume() {
			if (AndroidAds.AdsManager != null) {
				if (this.IsAdPaused)
					AndroidAds.AdsManager.Resume();
				else
					AndroidAds.AdsManager.Pause();
			}
		}

		public void OnAdError(IAdErrorEvent error_event) {
			// If an error occurs just destroy the AdsManager and set the state to Done to resume gameplay.
			System.Diagnostics.Debug.WriteLine("Error: " + error_event.Error);
			if (error_event.Error.ErrorCode == AdError.AdErrorCode.VastEmptyResponse) {
				this.NoAdsFoundTimer.Mark();
				this.State = AdState.NoAdsFound;
				Action exec = () => { this.Finish(); };
				Engine.SpawnInstance(new TimedExecution(2100, exec, true, false));
			} else {
				this.Finish();
			}
		}

		public void OnAdEvent(IAdEvent ad_event) {
			System.Diagnostics.Debug.WriteLine("Event: " + ad_event.Type);

			// These are the suggested event types to handle. For full list of all ad event
			// types, see the documentation for AdEvent.AdEventType.
			if (ad_event.Type == AdEventAdEventType.Loaded) {
				if (AndroidAds.AdsManager != null) {
					this.IsAdPaused = false;
					AndroidAds.AdsManager.Start();
					this.State = AdState.Playing;
				}
			} else if (ad_event.Type == AdEventAdEventType.Paused) {
				this.IsAdPaused = true;
			} else if (ad_event.Type == AdEventAdEventType.Resumed) {
				this.IsAdPaused = false;
			} else if (ad_event.Type == AdEventAdEventType.AllAdsCompleted) {
				// If the ads are done playing destroy the AdsManager and set the state to Done
				this.Finish();
			}
		}

		public void OnAdsManagerLoaded(IAdsManagerLoadedEvent loaded_event) {
			AndroidAds.AdsManager = loaded_event.AdsManager;
			AndroidAds.AdsManager.AddAdErrorListener(this.mAdEventListener);
			AndroidAds.AdsManager.AddAdEventListener(this.mAdEventListener);
			AndroidAds.AdsManager.Init();
		}
	}
}
#endif