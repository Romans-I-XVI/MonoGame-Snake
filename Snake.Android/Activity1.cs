using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MonoEngine;
using System.Linq;
using Android.Content;

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
		protected override void OnCreate(Bundle bundle) {
			base.OnCreate(bundle);

			this.MakeFullScreen();

			SnakeGame.Vibrator = (Vibrator)this.ApplicationContext.GetSystemService(Context.VibratorService);
			var g = new SnakeGame();
			var game_view = (View)g.Services.GetService(typeof(View));
			g.ExitEvent += () => this.MoveTaskToBack(true);

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

			g.Run();
		}

		public override void OnWindowFocusChanged(bool has_focus) {
			if (has_focus)
				this.MakeFullScreen();
			base.OnWindowFocusChanged(has_focus);
		}

#if ADS
		protected override void OnResume()
		{
			if (AndroidAds.AdsManager != null)
				AndroidAds.AdsManager.Resume();
			base.OnResume();
		}

		protected override void OnPause()
		{
			if (AndroidAds.AdsManager != null)
				AndroidAds.AdsManager.Pause();
			base.OnPause();
		}
#endif

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

