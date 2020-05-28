#if XBOX_LIVE
using System;
using MonoEngine;
using System.Diagnostics;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Microsoft.Xbox.Services.System;
using Microsoft.Xbox.Services;
using Microsoft.Xbox.Services.Statistics.Manager;
using Microsoft.Xna.Framework;
using Snake.Entities;
using Snake.Rooms;

namespace Snake
{
	public static class XboxLiveObject
	{
		public static bool IsReady => (XboxLiveObject.CurrentUser != null && XboxLiveObject.CurrentContext != null && XboxLiveObject.CurrentUser.IsSignedIn);
		public static XboxLiveUser CurrentUser { get; private set; }
		public static XboxLiveContext CurrentContext { get; private set; }
		public delegate void SignInCompletedDelegate(object sender, SignInCompletedEventArgs e);
		public static event SignInCompletedDelegate SignInCompleted;

		public delegate void SignOutCompletedDelegate(object sender, SignOutCompletedEventArgs e);
		public static event SignOutCompletedDelegate SignOutCompleted;

		public static bool CurrentlyAttemptingSignIn { get; private set; } = false;
		public static bool SubscribedToEvents { get; private set; } = false;

		private static void OnSignOutCompleted(object sender, SignOutCompletedEventArgs e) {
			XboxLiveObject.CurrentUser = null;
			XboxLiveObject.CurrentContext = null;
			SaveDataHandler.ResetCache();
			StatTracker.Reset();
			if (!(Engine.Room is RoomMain) && !(Engine.Room is RoomInit))
				Engine.ChangeRoom<RoomMain>();
			XboxLiveObject.SignOutCompleted?.Invoke(sender, e);
		}

		private static void OnSignInCompleted(object sender, SignInCompletedEventArgs e) {
			SaveDataHandler.ResetCache();
			StatTracker.LoadAsync();
			if (!(Engine.Room is RoomMain) && !(Engine.Room is RoomInit))
				Engine.ChangeRoom<RoomMain>();
		}

		public static async void SignIn(bool attempt_silent = true) {
			if (!XboxLiveObject.SubscribedToEvents) {
				XboxLiveUser.SignOutCompleted += XboxLiveObject.OnSignOutCompleted;
				XboxLiveObject.SignInCompleted += XboxLiveObject.OnSignInCompleted;
				XboxLiveObject.SubscribedToEvents = true;
			}

			if (XboxLiveObject.CurrentlyAttemptingSignIn)
				return;

			XboxLiveObject.CurrentlyAttemptingSignIn = true;
			var users = await User.FindAllAsync();
			try {
				XboxLiveObject.CurrentUser = new XboxLiveUser(users[0]);

				if (!XboxLiveObject.CurrentUser.IsSignedIn) {
					CoreDispatcher core_dispatcher = null;
					await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { core_dispatcher = Window.Current.CoreWindow.Dispatcher; });
					if (attempt_silent)
						try {
							await XboxLiveObject.CurrentUser.SignInSilentlyAsync(core_dispatcher);
						} catch {
							Debug.WriteLine("SignInSilentlyAsync Threw Exception");
						}

					if (!XboxLiveObject.CurrentUser.IsSignedIn) {
						Debug.WriteLine("Silent Sign-In failed, requesting sign in");
						try {
							await XboxLiveObject.CurrentUser.SignInAsync(core_dispatcher);
						} catch {
							Debug.WriteLine("SingInAsync Threw Exception");
						}
					}
				}

				if (XboxLiveObject.CurrentUser.IsSignedIn) {
					try {
						XboxLiveObject.CurrentContext = new XboxLiveContext(XboxLiveObject.CurrentUser);
					} catch {}

					if (XboxLiveObject.IsReady)
						XboxLiveObject.SignInCompleted?.Invoke(typeof(XboxLiveObject), new SignInCompletedEventArgs(XboxLiveObject.CurrentUser, XboxLiveObject.CurrentContext));
				}

				XboxLiveObject.WriteInfo();
			} catch {
				Debug.WriteLine("XboxLiveObject.SignIn() :: Unable to sign in for unknown reasons");
			}

			XboxLiveObject.CurrentlyAttemptingSignIn = false;
		}

		public static void WriteInfo()
		{
			Debug.WriteLine("############ Xbox Live Info ############");
			Debug.WriteLine(XboxLiveObject.CurrentUser.XboxUserId);
			Debug.WriteLine(XboxLiveObject.CurrentUser.WebAccountId);
			Debug.WriteLine("############ Xbox Live Info ############");
		}
	}

	public class SignInCompletedEventArgs
	{
		public readonly XboxLiveUser User;
		public readonly XboxLiveContext Context;

		public SignInCompletedEventArgs(XboxLiveUser user, XboxLiveContext context) {
			this.User = user;
			this.Context = context;
		}
	}
}
#endif