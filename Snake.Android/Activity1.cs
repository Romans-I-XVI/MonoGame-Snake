using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MonoEngine;
using System.Linq;
using Android.Content;
using Microsoft.Xna.Framework;

namespace Snake
{
	[Activity(Label = "@string/ApplicationName"
		, Icon = "@drawable/icon"
		, RoundIcon = "@drawable/iconround"
		, Theme = "@style/Theme.Splash"
		, AlwaysRetainTaskState = true
		, LaunchMode = LaunchMode.SingleInstance
		, ScreenOrientation = ScreenOrientation.SensorLandscape
		, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout | ConfigChanges.Navigation)]
	[IntentFilter(new[] { Intent.ActionMain }, Categories = new string[] { Intent.CategoryLauncher })]
	public class MainActivity : Microsoft.Xna.Framework.AndroidGameActivity
	{
		public static bool FixingSurfaceState = false;
		private static bool IsFirstOnResume = true;
		private static Game Game = null;

		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			this.MakeFullScreen();

			View game_view;
			if (MainActivity.Game == null) {
				SnakeGame.Vibrator = (Vibrator)this.ApplicationContext.GetSystemService(Context.VibratorService);
				var g = new SnakeGame();
				game_view = (View)g.Services.GetService(typeof(View));
				g.ExitEvent += () => this.MoveTaskToBack(true);
				g.Run();

				MainActivity.Game = g;
			} else {
				game_view = (View)MainActivity.Game.Services.GetService(typeof(View));
				ViewGroup parent = (ViewGroup)game_view.Parent;
				if (parent != null) {
					parent.RemoveView(game_view);
				}
			}

#if ADS
			var layout = new FrameLayout(this);
			layout.LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
			layout.AddView(game_view);
			SetContentView(layout);
			AndroidAds.Context = this;
			AndroidAds.ViewGroup = layout;
#else
			this.SetContentView(game_view);
#endif

		}

		public override void OnWindowFocusChanged(bool has_focus) {
			if (has_focus)
				this.MakeFullScreen();
			base.OnWindowFocusChanged(has_focus);
		}

		protected override void OnResume()
		{
#if ADS
			if (AndroidAds.AdsManager != null)
				AndroidAds.AdsManager.Resume();
#endif
			base.OnResume();

			if (!MainActivity.IsFirstOnResume) {
				MainActivity.FixingSurfaceState = true;
				System.Threading.Tasks.Task.Run(() => {
					while (MainActivity.FixingSurfaceState) {
						if (MainActivity.Game != null && MainActivity.Game.GraphicsDevice != null) {
							var game_view = (MonoGameAndroidGameView)((View)Game.Services.GetService(typeof(View)));
							(game_view).SurfaceChanged(game_view.Holder, Android.Graphics.Format.Rgb565, Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
						}
						System.Threading.Thread.Sleep(10);
					}
				});
			}

			MainActivity.IsFirstOnResume = false;
		}

		protected override void OnPause()
		{
			if (Engine.Room is Rooms.RoomPlay) {
				Engine.GetFirstInstanceByType<Entities.Controls.ControlPause>()?.TryPause();
			}
#if ADS
			if (AndroidAds.AdsManager != null)
				AndroidAds.AdsManager.Pause();
#endif
			base.OnPause();
		}

		protected override void OnDestroy()
		{
			ContentHolder.Deinit();
			SaveDataHandler.ResetCache();
			MainActivity.Game = null;
			base.OnDestroy();
		}

		protected void MakeFullScreen()	{
			var ui_options =
				SystemUiFlags.HideNavigation |
				SystemUiFlags.LayoutFullscreen |
				SystemUiFlags.LayoutHideNavigation |
				SystemUiFlags.LayoutStable |
				SystemUiFlags.Fullscreen |
				SystemUiFlags.ImmersiveSticky;

			this.Window.DecorView.SystemUiVisibility = (StatusBarVisibility)ui_options;
		}
	}

	[Activity(Label = "@string/ApplicationName"
		, Icon = "@drawable/icon"
		, RoundIcon = "@drawable/iconround"
		, Theme = "@style/Theme.Leanback"
		, AlwaysRetainTaskState = true
		, LaunchMode = LaunchMode.SingleInstance
		, ScreenOrientation = ScreenOrientation.Landscape
		, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout | ConfigChanges.Navigation)]
	[IntentFilter(new[] { Intent.ActionMain }, Categories = new string[] { Intent.CategoryLeanbackLauncher })]
	public class TVActivity : MainActivity
	{
		public override bool OnKeyDown([GeneratedEnum] Keycode key_code, KeyEvent e) {
			bool handled = false;
			if (key_code == Keycode.MediaFastForward) {
				var entities = Engine.GetAllInstances<MonoEngine.Entity>();
				var iFastForwardableEntities = entities.OfType<IFastForwardable>().ToList();
				foreach (var entity in iFastForwardableEntities) entity.onFastForwardPressed();
				handled = true;
			}

			if (handled)
				return true;
			return base.OnKeyDown(key_code, e);
		}

		public override bool OnKeyUp([GeneratedEnum] Keycode key_code, KeyEvent e) {
			bool handled = false;
			if (key_code == Keycode.MediaFastForward) {
				var entities = Engine.GetAllInstances<MonoEngine.Entity>();
				var iFastForwardableEntities = entities.OfType<IFastForwardable>().ToList();
				foreach (var entity in iFastForwardableEntities) entity.onFastForwardReleased();
				handled = true;
			}

			if (handled)
				return true;
			return base.OnKeyDown(key_code, e);
		}
	}
}

