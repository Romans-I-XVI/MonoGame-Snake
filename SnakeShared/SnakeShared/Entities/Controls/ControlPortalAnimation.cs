using MonoEngine;

namespace Snake.Entities.Controls
{
	public class ControlPortalAnimation : Entity
	{
		public const int AnimationSpeed = 125;
		public const int ImageCount = 2;
		private int CurrentIndex = 0;
		private readonly GameTimeSpan Timer = new GameTimeSpan();

		public override void onUpdate(float dt) {
			base.onUpdate(dt);

			if (this.Timer.TotalMilliseconds >= ControlPortalAnimation.AnimationSpeed) {
				if (this.CurrentIndex < ControlPortalAnimation.ImageCount - 1)
					this.CurrentIndex++;
				else
					this.CurrentIndex = 0;
				this.Timer.RemoveTime(ControlPortalAnimation.AnimationSpeed);
			}

			var portals = Engine.GetAllInstances<Portal>();
			foreach (var portal in portals) {
				portal.MainSprite.Index = this.CurrentIndex;
			}
		}

		public override void onResume(int pause_time) {
			base.onResume(pause_time);
			this.Timer.RemoveTime(pause_time);
		}
	}
}
