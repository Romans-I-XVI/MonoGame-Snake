using System;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoEngine;
using Snake.Entities.Controls;
using Snake.Rooms;

namespace Snake
{
	public class SnakeGame : EngineGame
	{
		public bool ExitGame = false;
		public delegate void dgExitEvent();
		public event dgExitEvent ExitEvent;
#if ANDROID
        public static Microsoft.Devices.Sensors.Accelerometer Accelerometer = new Microsoft.Devices.Sensors.Accelerometer();
        public static Android.OS.Vibrator Vibrator;
#endif

		public SnakeGame() : base(854, 480, 40, 22) {
#if !NETFX_CORE
			this.Graphics.SynchronizeWithVerticalRetrace = false;
#endif
			this.IsFixedTimeStep = false;
			this.Content.RootDirectory = "Content";

#if DEBUG
			this.Graphics.IsFullScreen = false;
#else
			this.Graphics.IsFullScreen = true;
#endif
#if !ANDROID && !IOS && !PLAYSTATION4
			this.Graphics.HardwareModeSwitch = false;
			this.IsMouseVisible = true;
#endif

#if NETFX_CORE
            Windows.UI.ViewManagement.ApplicationView.PreferredLaunchViewSize = new Windows.Foundation.Size(1920, 1080);
            Windows.UI.ViewManagement.ApplicationView.PreferredLaunchWindowingMode = Windows.UI.ViewManagement.ApplicationViewWindowingMode.PreferredLaunchViewSize;
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().FullScreenSystemOverlayMode = Windows.UI.ViewManagement.FullScreenSystemOverlayMode.Minimal;
            Windows.UI.Core.SystemNavigationManager.GetForCurrentView().BackRequested += (object sender, Windows.UI.Core.BackRequestedEventArgs args) => { args.Handled = true; };
#endif
#if !PLAYSTATION4
			this.Window.AllowUserResizing = true;
#endif
			this.Window.AllowAltF4 = true;
		}

		protected override void Initialize() {
			base.Initialize();
#if ANDROID
            if (SnakeGame.Accelerometer.State != Microsoft.Devices.Sensors.SensorState.Ready && SnakeGame.Accelerometer.State != Microsoft.Devices.Sensors.SensorState.NotSupported)
            {
                SnakeGame.Accelerometer.Start();
            }
#endif
		}

		protected override void LoadContent() {
			base.LoadContent();

			ContentHolder.Init(this, CustomContentLocations.TextureLocations);
#if !ANDROID && !IOS
			Engine.SpawnInstance<ControlFullscreen>();
#endif
			Engine.SpawnInstance<ControlBack>();

			Engine.ChangeRoom<RoomMain>();
		}

		protected override void Update(GameTime game_time) {
			base.Update(game_time);

			if (this.ExitGame)
			{
				this.ExitGame = false;
				this.ExitEvent?.Invoke();
#if !ANDROID && !IOS && !PLAYSTATION4
				this.Exit();
#endif
			}
		}
	}
}
