using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MonoEngine;
using System.Linq;

namespace Snake
{
    [Activity(Label = "@string/ApplicationName"
        , Icon = "@drawable/icon"
        , RoundIcon = "@drawable/iconround"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout | ConfigChanges.Navigation)]
    [IntentFilter(new[] { Android.Content.Intent.ActionMain }, Categories = new string[] { Android.Content.Intent.CategoryLauncher })]
    public class MainActivity : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            this.MakeFullScreen();

            SnakeGame.Vibrator = (Vibrator)this.ApplicationContext.GetSystemService(Android.Content.Context.VibratorService);
            var g = new SnakeGame();
            var game_view = (View)g.Services.GetService(typeof(View));
            g.ExitEvent += () => MoveTaskToBack(true);

            this.SetContentView(game_view);

            g.Run();
        }

        public override void OnWindowFocusChanged(bool hasFocus)
        {
            if (hasFocus)
                this.MakeFullScreen();
            base.OnWindowFocusChanged(hasFocus);
        }

        protected void MakeFullScreen()
        {
            var ui_options =
                SystemUiFlags.HideNavigation |
                SystemUiFlags.LayoutFullscreen |
                SystemUiFlags.LayoutHideNavigation |
                SystemUiFlags.LayoutStable |
                SystemUiFlags.Fullscreen |
                SystemUiFlags.ImmersiveSticky;

            Window.DecorView.SystemUiVisibility = (StatusBarVisibility)ui_options;
        }
    }

    [Activity(Label = "@string/ApplicationName"
        , Icon = "@drawable/icon"
        , RoundIcon = "@drawable/iconround"
        , Theme = "@style/Theme.Leanback"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.Landscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout | ConfigChanges.Navigation)]
    [IntentFilter(new[] { Android.Content.Intent.ActionMain }, Categories = new string[] { Android.Content.Intent.CategoryLeanbackLauncher })]
    public class TVActivity : MainActivity
    {
        public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            bool handled = false;
            if (keyCode == Keycode.MediaFastForward)
            {
                var entities = Engine.GetAllInstances<Entity>();
                var iFastForwardableEntities = entities.OfType<IFastForwardable>().ToList();
                foreach (IFastForwardable entity in iFastForwardableEntities)
                {
                    entity.onFastForwardPressed();
                }
                handled = true;
            }

            if (handled)
                return true;
            else
                return base.OnKeyDown(keyCode, e);
        }

        public override bool OnKeyUp([GeneratedEnum] Keycode keyCode, KeyEvent e)
        {
            bool handled = false;
            if (keyCode == Keycode.MediaFastForward)
            {
                var entities = Engine.GetAllInstances<Entity>();
                var iFastForwardableEntities = entities.OfType<IFastForwardable>().ToList();
                foreach (IFastForwardable entity in iFastForwardableEntities)
                {
                    entity.onFastForwardReleased();
                }
                handled = true;
            }

            if (handled)
                return true;
            else
                return base.OnKeyDown(keyCode, e);
        }
    }
}

