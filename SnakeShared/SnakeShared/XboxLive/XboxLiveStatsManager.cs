#if XBOX_LIVE
using Microsoft.Xbox.Services.Statistics.Manager;
using Microsoft.Xbox.Services.System;
using MonoEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xbox.Services.Leaderboard;

namespace Snake
{
	public static class XboxLiveStatsManager
	{
		private const int StatFlushDelay = 300000;
		private static readonly StatisticManager StatsManager = StatisticManager.SingletonInstance;
		private static readonly GameTimeSpan StatFlushTimer = new GameTimeSpan();
		private static bool HasFlushedStatsBefore = false;

		public static void OnSignOutCompleted(object sender, SignOutCompletedEventArgs e) {
			try {
				Debug.WriteLine("Flushing stats to service before sign out.");
				XboxLiveStatsManager.StatsManager.RequestFlushToService(e.User);
				XboxLiveStatsManager.StatsManager.DoWork();
				XboxLiveStatsManager.StatsManager.RemoveLocalUser(e.User);
				XboxLiveStatsManager.StatsManager.DoWork();
			} catch {}
		}

		public static void OnSignInCompleted(object sender, SignInCompletedEventArgs e) {
			try {
				XboxLiveStatsManager.StatsManager.AddLocalUser(e.User);
				XboxLiveStatsManager.StatsManager.DoWork();
			} catch {}
		}

		public static void OnExitGame() {
			if (!XboxLiveObject.IsReady)
				return;

			try {
				Debug.WriteLine("Flushing stats to service before exiting game.");
				XboxLiveStatsManager.StatsManager.RequestFlushToService(XboxLiveObject.CurrentUser);
				XboxLiveStatsManager.StatsManager.DoWork();
				XboxLiveStatsManager.StatsManager.RemoveLocalUser(XboxLiveObject.CurrentUser);
				XboxLiveStatsManager.StatsManager.DoWork();
			} catch {}
		}

		public static void SetStatInteger(XboxLiveStats stat, int value) {
			if (!XboxLiveObject.IsReady)
				return;

			XboxLiveStatsManager.StatsManager.SetStatisticIntegerData(XboxLiveObject.CurrentUser, stat.ToString(), value);
			XboxLiveStatsManager.WriteDebugOutput(stat, value.ToString());
			XboxLiveStatsManager.StatsManager.DoWork();
		}

		public static void SetStatNumber(XboxLiveStats stat, double value) {
			if (!XboxLiveObject.IsReady)
				return;

			XboxLiveStatsManager.StatsManager.SetStatisticNumberData(XboxLiveObject.CurrentUser, stat.ToString(), value);
			XboxLiveStatsManager.WriteDebugOutput(stat, value.ToString());
			XboxLiveStatsManager.StatsManager.DoWork();
		}

		public static void SetStatString(XboxLiveStats stat, string value) {
			if (!XboxLiveObject.IsReady)
				return;

			XboxLiveStatsManager.StatsManager.SetStatisticStringData(XboxLiveObject.CurrentUser, stat.ToString(), value);
			XboxLiveStatsManager.WriteDebugOutput(stat, value);
			XboxLiveStatsManager.StatsManager.DoWork();
		}

		public static void CheckAndFlush(bool force_flush = false) {
			if (!XboxLiveObject.IsReady)
				return;

			if (force_flush || XboxLiveStatsManager.StatFlushTimer.TotalMilliseconds >= XboxLiveStatsManager.StatFlushDelay || !XboxLiveStatsManager.HasFlushedStatsBefore) {
				XboxLiveStatsManager.HasFlushedStatsBefore = true;
				XboxLiveStatsManager.StatsManager.RequestFlushToService(XboxLiveObject.CurrentUser);
				XboxLiveStatsManager.StatFlushTimer.Mark();
				Debug.WriteLine("Flushed Stats To Service");
				XboxLiveStatsManager.StatsManager.DoWork();
			}
		}

		private static void WriteDebugOutput(XboxLiveStats stat, string value) {
			Debug.WriteLine("XboxLiveStatManager :: SetStat - " + stat + " = " + value);
		}

		public static void PrintStats() {
			if (!XboxLiveObject.IsReady)
				return;

			var stats = XboxLiveStatsManager.StatsManager.GetStatisticNames(XboxLiveObject.CurrentUser);
			Debug.WriteLine("######### Printing Stats #######");
			foreach (string stat in stats) {
				Debug.WriteLine("~~~~~~~" + stat + "~~~~~~~");
				var ret = XboxLiveStatsManager.StatsManager.GetStatistic(XboxLiveObject.CurrentUser, stat);
				XboxLiveStatsManager.StatsManager.DoWork();
				Debug.WriteLine(ret.AsInteger);

				var query = new LeaderboardQuery {
					MaxItems = 10
				};
				XboxLiveStatsManager.StatsManager.GetLeaderboard(XboxLiveObject.CurrentUser, stat, query);
				var stat_events = XboxLiveStatsManager.StatsManager.DoWork();
				while (stat_events.Count == 0) stat_events = XboxLiveStatsManager.StatsManager.DoWork();
				foreach (var stat_event in stat_events)
					if (stat_event.EventType == StatisticEventType.GetLeaderboardComplete
						&& stat_event.ErrorCode == 0)
						try {
							var leader_args = (LeaderboardResultEventArgs)stat_event.EventArgs;
							var leaderboard_result = leader_args.Result;
							foreach (var leader_row in leaderboard_result.Rows) Debug.WriteLine("Rank: {0} | Gamertag: {1} | Score: {2}", leader_row.Rank, leader_row.Gamertag, leader_row.Values[0]);
						} catch {}
			}

			Debug.WriteLine("######### Printing Stats #######");
		}
	}
}
#endif
