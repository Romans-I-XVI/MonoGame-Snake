#if XBOX_LIVE
using Microsoft.Xbox.Services.Statistics.Manager;
using Microsoft.Xbox.Services.System;
using MonoEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Snake
{
    public static class XboxLiveStatsManager
    {
        private static StatisticManager StatsManager = StatisticManager.SingletonInstance;
        private static GameTimeSpan _statFlushTimer = new GameTimeSpan();
        private static bool _hasFlushedStatsBefore = false;

        public static void OnSignOutCompleted(object sender, SignOutCompletedEventArgs e)
        {
            try
            {
                Debug.WriteLine("Flushing stats to service before sign out.");
                StatsManager.RequestFlushToService(e.User);
                StatsManager.DoWork();
                StatsManager.RemoveLocalUser(e.User);
                StatsManager.DoWork();
            }
            catch
            {
            }
        }

        public static void OnSignInCompleted(object sender, SignInCompletedEventArgs e)
        {
            try
            {
                StatsManager.AddLocalUser(e.User);
                StatsManager.DoWork();
            }
            catch
            {
            }
        }

        public static void OnExitGame()
        {
            if (!XboxLiveObject.IsReady)
                return;

            try
            {
                Debug.WriteLine("Flushing stats to service before exiting game.");
                StatsManager.RequestFlushToService(XboxLiveObject.CurrentUser);
                StatsManager.DoWork();
                StatsManager.RemoveLocalUser(XboxLiveObject.CurrentUser);
                StatsManager.DoWork();
            }
            catch
            {
            }
        }

        public static void SetStatInteger(XboxLiveStats stat, int value)
        {
            if (!XboxLiveObject.IsReady)
                return;

            StatsManager.SetStatisticIntegerData(XboxLiveObject.CurrentUser, stat.ToString(), value);
            WriteDebugOutput(stat, value.ToString());
            StatsManager.DoWork();
        }

        public static void SetStatNumber(XboxLiveStats stat, double value)
        {
            if (!XboxLiveObject.IsReady)
                return;

            StatsManager.SetStatisticNumberData(XboxLiveObject.CurrentUser, stat.ToString(), value);
            WriteDebugOutput(stat, value.ToString());
            StatsManager.DoWork();
        }

        public static void SetStatString(XboxLiveStats stat, string value)
        {
            if (!XboxLiveObject.IsReady)
                return;

            StatsManager.SetStatisticStringData(XboxLiveObject.CurrentUser, stat.ToString(), value);
            WriteDebugOutput(stat, value);
            StatsManager.DoWork();
        }

        public static void CheckAndFlush(bool forceFlush = false)
        {
            if (!XboxLiveObject.IsReady)
                return;

            if (forceFlush || _statFlushTimer.TotalMilliseconds >= 300000 || !_hasFlushedStatsBefore)
            {
                _hasFlushedStatsBefore = true;
                StatsManager.RequestFlushToService(XboxLiveObject.CurrentUser);
                _statFlushTimer.Mark();
                Debug.WriteLine("Flushed Stats To Service");
                StatsManager.DoWork();
            }
        }

        private static void WriteDebugOutput(XboxLiveStats stat, string value)
        {
            Debug.WriteLine("XboxLiveStatManager :: SetStat - " + stat.ToString() + " = " + value);
        }

        public static void PrintStats()
        {
            if (!XboxLiveObject.IsReady)
                return;

            var stats = StatsManager.GetStatisticNames(XboxLiveObject.CurrentUser);
            Debug.WriteLine("######### Printing Stats #######");
            foreach (var stat in stats)
            {
                Debug.WriteLine("~~~~~~~" + stat + "~~~~~~~");
                var ret = StatsManager.GetStatistic(XboxLiveObject.CurrentUser, stat);
                StatsManager.DoWork();
                Debug.WriteLine(ret.AsInteger);

                var query = new Microsoft.Xbox.Services.Leaderboard.LeaderboardQuery()
                {
                    MaxItems = 10,
                };
                StatsManager.GetLeaderboard(XboxLiveObject.CurrentUser, stat, query);
                var statEvents = StatsManager.DoWork();
                while (statEvents.Count == 0)
                {
                    statEvents = StatsManager.DoWork();
                }
                foreach (StatisticEvent statEvent in statEvents)
                {
                    if (statEvent.EventType == StatisticEventType.GetLeaderboardComplete
                        && statEvent.ErrorCode == 0)
                    {
                        try
                        {
                            LeaderboardResultEventArgs leaderArgs = (LeaderboardResultEventArgs)statEvent.EventArgs;
                            Microsoft.Xbox.Services.Leaderboard.LeaderboardResult leaderboardResult = leaderArgs.Result;
                            foreach (Microsoft.Xbox.Services.Leaderboard.LeaderboardRow leaderRow in leaderboardResult.Rows)
                            {
                                Debug.WriteLine(string.Format("Rank: {0} | Gamertag: {1} | Score: {2}", leaderRow.Rank, leaderRow.Gamertag, leaderRow.Values[0]));
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            Debug.WriteLine("######### Printing Stats #######");
        }
    }
}
#endif
