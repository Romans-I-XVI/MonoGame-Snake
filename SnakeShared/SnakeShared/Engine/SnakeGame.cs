using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoEngine;
using Snake.Entities;
using Snake.Entities.Controls;
using Snake.Enums;
using Snake.Rooms;

namespace Snake
{
	public class SnakeGame : EngineGame
	{
		private const int FrameRateLimit = 360;
		public bool ExitGame = false;
		public delegate void dgExitEvent();
		public event dgExitEvent ExitEvent;
#if ANDROID
        public static Microsoft.Devices.Sensors.Accelerometer Accelerometer;
        public static Android.OS.Vibrator Vibrator;
#endif

		public SnakeGame() : base(854, 480, 0, 0) {
#if !NETFX_CORE
			this.Graphics.SynchronizeWithVerticalRetrace = false;
#endif
			this.IsFixedTimeStep = false;
			this.IsMouseVisible = false;
			this.Content.RootDirectory = "Content";

			this.Graphics.IsFullScreen = true;
#if !ANDROID && !IOS && !PLAYSTATION4
			this.Graphics.HardwareModeSwitch = false;
#endif

#if NETFX_CORE
            Windows.UI.ViewManagement.ApplicationView.PreferredLaunchViewSize = new Windows.Foundation.Size(854, 480);
            Windows.UI.ViewManagement.ApplicationView.PreferredLaunchWindowingMode = Windows.UI.ViewManagement.ApplicationViewWindowingMode.PreferredLaunchViewSize;
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().FullScreenSystemOverlayMode = Windows.UI.ViewManagement.FullScreenSystemOverlayMode.Minimal;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (object sender, Windows.UI.Core.BackRequestedEventArgs args) => { args.Handled = true; };
#endif
#if !PLAYSTATION4
			this.Window.AllowUserResizing = true;
#endif
			this.Window.AllowAltF4 = true;

			this.SetFrameRateLimit(SnakeGame.FrameRateLimit);
		}

		protected override void Initialize() {
			base.Initialize();

#if NETFX_CORE
			// This fixes a bug with Xbox not being set to fullscreen properly
			this.Graphics.PreferredBackBufferWidth = 1920;
			this.Graphics.PreferredBackBufferHeight = 1080;
			this.Graphics.ApplyChanges();
#endif

			// Setting to default values in case a previous game existed and was disposed
			Settings.CurrentTheme = 0;
			Settings.CurrentGameplaySpeed = GameplaySpeeds.Medium;
			Settings.CurrentGameRoom = GameRooms.Open;

#if ANDROID
			SnakeGame.Accelerometer = new Microsoft.Devices.Sensors.Accelerometer();
            if (SnakeGame.Accelerometer.State != Microsoft.Devices.Sensors.SensorState.Ready && SnakeGame.Accelerometer.State != Microsoft.Devices.Sensors.SensorState.NotSupported)
            {
                SnakeGame.Accelerometer.Start();
            }
#endif
		}

		protected override void LoadContent() {
			base.LoadContent();

			ContentHolder.Init(this, CustomContentLocations.TextureLocations);
			Levels.Init();
#if !ANDROID && !IOS
			Engine.SpawnInstance<ControlFullscreen>();
#endif
			Engine.SpawnInstance<ControlBack>();
			Engine.SpawnInstance<Background>();
#if DEBUG
			Engine.SpawnInstance(new DebuggerWithTerminal(ContentHolder.Get(AvailableFonts.retro_computer)));
#endif

#if ADS
#if ANDROID
#if AMAZON
			Engine.SpawnInstance<AndroidAds>();
#endif
#endif
#endif

#if XBOX_LIVE
			XboxLiveObject.SignOutCompleted += XboxLiveStatsManager.OnSignOutCompleted;
			XboxLiveObject.SignInCompleted += XboxLiveStatsManager.OnSignInCompleted;
			ExitEvent += XboxLiveStatsManager.OnExitGame;
			XboxLiveObject.SignIn();
#endif

			Engine.ChangeRoom<RoomInit>();
		}

		protected override void Update(GameTime game_time) {
			base.Update(game_time);

#if ANDROID
			if (MainActivity.FixingSurfaceState) {
				MainActivity.FixingSurfaceState = false;
			}
#endif

			if (this.ExitGame) {
				this.ExitGame = false;
				this.ExitEvent?.Invoke();
#if !ANDROID && !IOS && !PLAYSTATION4
				this.Exit();
#endif
			}
		}

		public void SetFrameRateLimit(int frame_rate_limit) {
#if NETCOREAPP
			if (frame_rate_limit <= 0) {
				this.IsFixedTimeStep = false;
				this.TargetElapsedTime = TimeSpan.FromTicks(166667L); // This is the default MonoGame uses
			} else {
				this.IsFixedTimeStep = true;
				this.TargetElapsedTime = TimeSpan.FromTicks((int)((1000 / (float)frame_rate_limit) * 10000));
			}
#else
			this.IsFixedTimeStep = false;
#endif
		}
	}
}
