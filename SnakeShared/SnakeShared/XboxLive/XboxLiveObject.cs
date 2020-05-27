#if XBOX_LIVE
using System;
using MonoEngine;
using System.Diagnostics;
using Microsoft.Xbox.Services.System;
using Microsoft.Xbox.Services;
using Microsoft.Xbox.Services.Statistics.Manager;
using Microsoft.Xna.Framework;
using Snake.Rooms;

namespace Snake
{
    public static class XboxLiveObject
    {
        public static bool IsReady => (CurrentUser != null && CurrentContext != null && CurrentUser.IsSignedIn);
        public static XboxLiveUser CurrentUser { get; private set; }
        public static XboxLiveContext CurrentContext { get; private set; }
        public delegate void SignInCompletedDelegate(object sender, SignInCompletedEventArgs e);
        public static event SignInCompletedDelegate SignInCompleted;

        public delegate void SignOutCompletedDelegate(object sender, SignOutCompletedEventArgs e);
        public static event SignOutCompletedDelegate SignOutCompleted;

        private static bool _currently_attempting_sign_in = false;
        private static bool _subscribed_to_events = false;

        private static void OnSignOutCompleted(object sender, SignOutCompletedEventArgs e)
        {
            CurrentUser = null;
            CurrentContext = null;
            SaveDataHandler.ResetCache();
            // StatTracker.Reset();
            if (!(Engine.Room is RoomMain))
                Engine.ChangeRoom<RoomMain>();
            SignOutCompleted?.Invoke(sender, e);
        }

        private static void OnSignInCompleted(object sender, SignInCompletedEventArgs e)
        {
            SaveDataHandler.ResetCache();
            // StatTracker.LoadAsync();
            if (!(Engine.Room is RoomMain))
                Engine.ChangeRoom<RoomMain>();
        }

        public static async void SignIn(bool attempt_silent = true)
        {
            if (!_subscribed_to_events)
            {
                XboxLiveUser.SignOutCompleted += OnSignOutCompleted;
                XboxLiveObject.SignInCompleted += OnSignInCompleted;
                _subscribed_to_events = true;
            }

            if (_currently_attempting_sign_in)
                return;

            _currently_attempting_sign_in = true;
            var users = await Windows.System.User.FindAllAsync();
            try
            {
                CurrentUser = new XboxLiveUser(users[0]);

                if (!CurrentUser.IsSignedIn)
                {
                    Windows.UI.Core.CoreDispatcher coreDispatcher = null;
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        coreDispatcher = Windows.UI.Xaml.Window.Current.CoreWindow.Dispatcher;
                    });
                    if (attempt_silent)
                    {
                        try
                        {
                            await CurrentUser.SignInSilentlyAsync(coreDispatcher);
                        }
                        catch
                        {
                            Debug.WriteLine("SignInSilentlyAsync Threw Exception");
                        }
                    }
                    if (!CurrentUser.IsSignedIn)
                    {
                        Debug.WriteLine("Silent Sign-In failed, requesting sign in");
                        try
                        {
                            await CurrentUser.SignInAsync(coreDispatcher);
                        }
                        catch
                        {
                            Debug.WriteLine("SingInAsync Threw Exception");
                        }
                    }
                }
                if (CurrentUser.IsSignedIn)
                {
                    try
                    {
                        CurrentContext = new XboxLiveContext(CurrentUser);
                    }
                    catch
                    {
                    }

                    if (IsReady)
                    {
                        SignInCompleted?.Invoke(typeof(XboxLiveObject), new SignInCompletedEventArgs(CurrentUser, CurrentContext));
                    }
                }
                WriteInfo();
            }
            catch
            {
                Debug.WriteLine("XboxLiveObject.SignIn() :: Unable to sign in for unkown reasons");
            }
            _currently_attempting_sign_in = false;
        }

        public static void WriteInfo()
        {
            Debug.WriteLine("############ Xbox Live Info ############");
            Debug.WriteLine(CurrentUser.XboxUserId);
            Debug.WriteLine(CurrentUser.WebAccountId);
            Debug.WriteLine("############ Xbox Live Info ############");
        }
    }

    public class SignInCompletedEventArgs
    {
        public readonly XboxLiveUser User;
        public readonly XboxLiveContext Context;
        public SignInCompletedEventArgs(XboxLiveUser user, XboxLiveContext context)
        {
            User = user;
            Context = context;
        }
    }
}
#endif