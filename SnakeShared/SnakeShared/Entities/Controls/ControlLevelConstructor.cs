using System;
using MonoEngine;

namespace Snake.Entities.Controls
{
	public class ControlLevelConstructor : Entity
	{
		public const int ObjectSpawnDelay = 30;
		private readonly WallSpawn[] WallSpawns;
		private readonly GameTimeSpan Timer = new GameTimeSpan();
		private int Index = 0;
		private bool SoundToggle = true;

		public ControlLevelConstructor(LevelData level_data) {
			this.WallSpawns = level_data.WallSpawns;
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);
			if (this.Timer.TotalMilliseconds > ControlLevelConstructor.ObjectSpawnDelay * this.Index) {
				if (this.Index < this.WallSpawns.Length) {
					var spawn = this.WallSpawns[this.Index];
					Engine.SpawnInstance(new Wall(spawn.X, spawn.Y, spawn.Scale));

					if (this.SoundToggle) {
						SFXPlayer.Play(AvailableSounds.create_block, 0.75f);
					}
				}


				this.SoundToggle = !this.SoundToggle;
				this.Index++;
				if (this.Index > this.WallSpawns.Length - 1) {
					this.Destroy();
				}
			}
		}

		public static int TotalTimeToSpawnLevel(LevelData level_data) {
			int time = 0;
			if (level_data.WallSpawns != null) {
				time += ControlLevelConstructor.ObjectSpawnDelay * level_data.WallSpawns.Length;
			}

			return time;
		}
	}
}
