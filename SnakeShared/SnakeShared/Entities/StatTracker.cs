using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MonoEngine;
using Snake.Enums;
using Snake.GameEvents;

namespace Snake.Entities
{
    public class StatTracker : Entity
    {
        public static int FoodEaten { get; private set; } = 0;

        public StatTracker() {
            this.IsPersistent = true;
            this.IsPauseable = false;
            this.Depth = -int.MaxValue;
        }

        public override void onGameEvent(GameEvent game_event) {
            base.onGameEvent(game_event);

            if (game_event is FoodEatenEvent) {
                StatTracker.FoodEaten++;
            } else if (game_event is SnakeDiedEvent) {
                StatTracker.Save();
                StatTracker.PostFoodStatOnly();
            }
        }

        public static void Load() {
            StatTracker.Reset();

            string data_string = SaveDataHandler.LoadData(Settings.StatTrackerSavePath);
            if (data_string != null)
                StatTracker.FoodEaten = int.Parse(data_string);
        }

        public static async void LoadAsync() {
            Action load = StatTracker.Load;
            await Task.Run(load);
        }

        public static void Save() {
            string data_string = StatTracker.FoodEaten.ToString();
            SaveDataHandler.SaveData(data_string, Settings.StatTrackerSavePath);
        }

        public static void Reset() {
            StatTracker.FoodEaten = 0;
        }

        public static void PostStats(bool force_post = false)
        {
#if XBOX_LIVE
            if (StatTracker.FoodEaten > 0)
            {
                XboxLiveStatsManager.SetStatInteger(XboxLiveStats.FoodEaten, StatTracker.FoodEaten);
            }

            var game_rooms = Enum.GetValues(typeof(GameRooms));
            var game_speeds = Enum.GetValues(typeof(GameplaySpeeds));
            foreach (GameRooms game_room in game_rooms) {
                foreach (GameplaySpeeds gameplay_speed in game_speeds) {
                    const string prefix = "LongestSnake";
                    string stat_string = prefix + game_room + gameplay_speed;

                    bool success = Enum.TryParse(typeof(XboxLiveStats), stat_string, out var stat);
                    if (success && stat is XboxLiveStats) {
                        string data = SaveDataHandler.LoadData(Settings.GetSaveFilePath(game_room, gameplay_speed));
                        int.TryParse(data, out int score);
                        if (score > 0) {
                            XboxLiveStatsManager.SetStatInteger((XboxLiveStats)stat, score);
                        }
                    }
                }
            }

            XboxLiveStatsManager.CheckAndFlush(force_post);
#else
#endif
        }

        private static void PostFoodStatOnly(bool force_post = false) {
#if XBOX_LIVE
            if (StatTracker.FoodEaten > 0) {
                XboxLiveStatsManager.SetStatInteger(XboxLiveStats.FoodEaten, StatTracker.FoodEaten);
                XboxLiveStatsManager.CheckAndFlush();
            }
#else
#endif
        }
    }
}

