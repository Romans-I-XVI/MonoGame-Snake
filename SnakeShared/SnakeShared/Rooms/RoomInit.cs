using System;
using MonoEngine;
using System.Collections.Generic;
using Snake.Entities;
using Snake.Enums;

namespace Snake.Rooms
{
    class RoomInit : Room
    {
        private const int MinimumSplashDuration = 2500;

        public override void onSwitchTo(Room previous_room, Dictionary<string, object> args) {
            Engine.SpawnInstance<LoadingSplash>();

            // Load all save data in to cache
            var timer = new GameTimeSpan();
            Action initialize_data = () => {
                foreach (GameRooms game_room in Enum.GetValues(typeof(GameRooms))) {
                    foreach (GameplaySpeeds gameplay_speed in Enum.GetValues(typeof(GameplaySpeeds))) {
                        SaveDataHandler.LoadData(Settings.GetSaveFilePath(game_room, gameplay_speed));
                    }
                }

                int remaining_time_to_wait = RoomInit.MinimumSplashDuration - (int)timer.TotalMilliseconds;
                if (remaining_time_to_wait < 0)
                    remaining_time_to_wait = 0;

                Engine.SpawnInstance(new TimedExecution(remaining_time_to_wait, () => Engine.ChangeRoom<RoomMain>()));
            };

            Engine.SpawnInstance(new TimedExecution(100, initialize_data));
        }

        public override void onSwitchAway(Room next_room) {
        }
    }
}
