using System;
using MonoEngine;

namespace Snake.Entities.Controls
{
	public class ControlLevelConstructor : Entity
	{
		public const int ObjectSpawnDelay = 30;
		private readonly WallSpawn[] WallSpawns;
		private readonly PortalSpawn[] PortalSpawns;
		private readonly GameTimeSpan Timer = new GameTimeSpan();
		private int Index = 0;
		private bool SoundToggle = true;

		public ControlLevelConstructor(LevelData level_data) {
			this.WallSpawns = level_data.WallSpawns;
			this.PortalSpawns = level_data.PortalSpawns;
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);
			if (this.Timer.TotalMilliseconds > ControlLevelConstructor.ObjectSpawnDelay * this.Index) {
				int wall_count = WallSpawns?.Length ?? 0;
				int portal_count = PortalSpawns?.Length ?? 0;

				if (this.WallSpawns != null && this.Index < wall_count) {
					var spawn = this.WallSpawns[this.Index];
					Engine.SpawnInstance(new Wall(spawn.X, spawn.Y, spawn.Scale));

					if (this.SoundToggle) {
						SFXPlayer.Play(AvailableSounds.create_block, 0.75f);
					}
				} else if (this.PortalSpawns != null && this.Index < wall_count + portal_count) {
					var spawn = this.PortalSpawns[this.Index - wall_count];
					Engine.SpawnInstance(new Portal(spawn));
				}

				this.SoundToggle = !this.SoundToggle;
				this.Index++;
				if (this.Index > wall_count + portal_count - 1) {
					this.Destroy();
				}
			}
		}

		public override void onResume(int pause_time) {
			base.onResume(pause_time);
			this.Timer.RemoveTime(pause_time);
		}

		public static int TotalTimeToSpawnLevel(LevelData level_data) {
			int time = 0;

			if (level_data.WallSpawns != null) {
				time += ControlLevelConstructor.ObjectSpawnDelay * level_data.WallSpawns.Length;
			}

			if (level_data.PortalSpawns != null) {
				time += ControlLevelConstructor.ObjectSpawnDelay * level_data.PortalSpawns.Length;
			}

			return time;
		}
	}
}
