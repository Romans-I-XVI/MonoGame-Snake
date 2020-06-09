#if ADS
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Snake
{
    public class AdEventListener : Java.Lang.Object, IAdErrorEventAdErrorListener, IAdEventAdEventListener, IAdsLoaderAdsLoadedListener
    {
        internal AndroidAds AndroidAds;

        internal AdEventListener(AndroidAds android_ads)
        {
            AndroidAds = android_ads;
        }

        public void OnAdError(IAdErrorEvent error_event)
        {
            AndroidAds.OnAdError(error_event);
        }

        public void OnAdEvent(IAdEvent ad_event)
        {
            AndroidAds.OnAdEvent(ad_event);
        }

        public void OnAdsManagerLoaded(IAdsManagerLoadedEvent loaded_event)
        {
            AndroidAds.OnAdsManagerLoaded(loaded_event);
        }
    }

    public class AndroidAds : Ads
    {
        public static Context Context;
        public static ViewGroup ViewGroup;
        public static IAdsManager AdsManager;
        private const string CustomGUIDSavePath = "CustomAndroidGUID.txt";

        private string _adUrl = null;
        public override string AdUrl {
            get {
                if (string.IsNullOrEmpty(_adUrl))
                    this.getAdUrl("snake");
                return _adUrl;
            } 
        }

        private ImaSdkFactory mSdkFactory;
        private IAdDisplayContainer mAdDisplayContainer;
        private AdEventListener mAdEventListener;
        private bool _is_ad_paused = false;

        public AndroidAds() : base("Snake")
        {
            mAdEventListener = new AdEventListener(this);
            mSdkFactory = ImaSdkFactory.Instance;
            mAdDisplayContainer = mSdkFactory.CreateAdDisplayContainer();
            mAdDisplayContainer.AdContainer = ViewGroup;
            Engine.SpawnInstance(new TimedExecution(15000, () => {
                this.getAdUrl("snake");
            }, true, false));
        }

        public override void onButtonDown(GamePadEventArgs e)
        {
            base.onButtonDown(e);
            if (e.Button == Buttons.Start)
                pause_resume();
        }

        public override void onKeyDown(KeyboardEventArgs e)
        {
            base.onKeyDown(e);
            if (e.Key == Keys.P || e.Key == Keys.Pause || e.Key == Keys.MediaPlayPause)
                pause_resume();
        }

        public override void Show()
        {
            if (string.IsNullOrEmpty(AdUrl))
                return;

            State = AdState.Loading;
            IAdsLoader mAdsLoader = mSdkFactory.CreateAdsLoader(Context);
            mAdsLoader.AddAdErrorListener(mAdEventListener);
            mAdsLoader.AddAdsLoadedListener(mAdEventListener);

            IAdsRequest request = mSdkFactory.CreateAdsRequest();
            request.AdTagUrl = AdUrl;
            request.AdDisplayContainer = mAdDisplayContainer;
            mAdsLoader.RequestAds(request);
        }

        private int getDoNotTrack()
        {
            int dnt = 0;

            try
            {
                var cr = Android.App.Application.Context.ContentResolver;
                dnt = Android.Provider.Settings.Secure.GetInt(cr, "limit_ad_tracking");
            }
            catch
            {
            }

            return dnt;
        }

        private string getAdvertiserID()
        {
            string id = null;
            var cr = Android.App.Application.Context.ContentResolver;

            // If do not track is on return empty advertiser id
            if (getDoNotTrack() == 1)
            {
                return "00000000-0000-0000-0000-000000000000";
            }

            // First try to get the advertising_id (from on FireTV) if available
            try
            {
                id = Android.Provider.Settings.Secure.GetString(cr, "advertising_id");
            }
            catch
            {
            }

            // Next use android_id if available
            if (string.IsNullOrWhiteSpace(id))
            {
                try
                {
                    id = Android.Provider.Settings.Secure.GetString(cr, Android.Provider.Settings.Secure.AndroidId);
                }
                catch
                {
                }
            }

            // Next use Serial if available (exists on API Level 9)
            if (string.IsNullOrWhiteSpace(id))
            {
                id = Android.OS.Build.Serial;
            }

            // Finally if we still don't have an ad id generate a uuid and save it to file.
            if (string.IsNullOrWhiteSpace(id))
            {
                id = SaveDataHandler.LoadData(AndroidAds.CustomGUIDSavePath);
                if (string.IsNullOrWhiteSpace(id))
                {
                    id = System.Guid.NewGuid().ToString();
                    SaveDataHandler.SaveData(id, AndroidAds.CustomGUIDSavePath);
                }
            }

            return id;
        }

        async private void getAdUrl(string game_name) {
            try
            {
                var httpClient = new System.Net.Http.HttpClient();
                string res = await httpClient.GetStringAsync("http://romansixvigaming.com/ads/firetv_ad_urls.json");
                var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(res);
                if (data.ContainsKey(game_name))
                    _adUrl = data[game_name];

                _adUrl = _adUrl.Replace("ADS_LIMIT_TRACKING", this.getDoNotTrack().ToString());
                _adUrl = _adUrl.Replace("ADS_TRACKING_ID", this.getAdvertiserID());
            }
            catch
            {
                _adUrl = null;
            }
        }

        private void finish()
        {
            if (AdsManager != null)
            {
                AdsManager.Destroy();
                AdsManager = null;
            }
            State = AdState.Done;
            AdTimer.Mark();
            onShowEnd();
        }

        private void pause_resume()
        {
            if (AdsManager != null)
            {
                if (_is_ad_paused)
                {
                    AdsManager.Resume();
                }
                else
                {
                    AdsManager.Pause();
                }
            }
        }

        public void OnAdError(IAdErrorEvent error_event)
        {
            // If an error occurs just destroy the AdsManager and set the state to Done to resume gameplay.
            System.Diagnostics.Debug.WriteLine("Error: " + error_event.Error);
            if (error_event.Error.ErrorCode == AdError.AdErrorCode.VastEmptyResponse)
            {
                NoAdsFoundTimer.Mark();
                State = AdState.NoAdsFound;
                Action exec = () =>
                {
                    finish();
                };
                var timed_execution = new TimedExecution(2100, exec, true);
                timed_execution.IsPauseable = false;
            }
            else
            {
                finish();
            }
        }

        public void OnAdEvent(IAdEvent ad_event)
        {
            System.Diagnostics.Debug.WriteLine("Event: " + ad_event.Type);

            // These are the suggested event types to handle. For full list of all ad event
            // types, see the documentation for AdEvent.AdEventType.
            if (ad_event.Type == AdEventAdEventType.Loaded)
            {
                if (AdsManager != null)
                {
                    _is_ad_paused = false;
                    AdsManager.Start();
                    this.State = AdState.Playing;
                }
            }
            else if (ad_event.Type == AdEventAdEventType.Paused)
            {
                _is_ad_paused = true;
            }
            else if (ad_event.Type == AdEventAdEventType.Resumed)
            {
                _is_ad_paused = false;
            }
            else if (ad_event.Type == AdEventAdEventType.AllAdsCompleted)
            {
                // If the ads are done playing destroy the AdsManager and set the state to Done
                finish();
            }
        }

        public void OnAdsManagerLoaded(IAdsManagerLoadedEvent loaded_event)
        {
            AdsManager = loaded_event.AdsManager;
            AdsManager.AddAdErrorListener(mAdEventListener);
            AdsManager.AddAdEventListener(mAdEventListener);
            AdsManager.Init();
        }
    }
}
#endif