using System;
using MonoEngine;

namespace Snake.Entities
{
	public class TimedExecution : Entity
	{
		private readonly Action Exec;
		private readonly GameTimeSpan Timer = new GameTimeSpan();
		private readonly int TimeToWait;
		private bool Executed;

		public TimedExecution(int ms_to_wait, Action exec, bool persistent = false, bool pauseable = true) {
			this.IsPersistent = persistent;
			this.IsPauseable = pauseable;
			this.TimeToWait = ms_to_wait;
			this.Exec = exec;
		}

		public override void onUpdate(float dt) {
			if (this.Timer.TotalMilliseconds >= this.TimeToWait && !this.Executed) {
				this.Exec();
				this.Executed = true;
				this.IsExpired = true;
			}

			base.onUpdate(dt);
		}

		public override void onResume(int pause_time) {
			base.onResume(pause_time);
			this.Timer.RemoveTime(pause_time);
		}
	}
}
