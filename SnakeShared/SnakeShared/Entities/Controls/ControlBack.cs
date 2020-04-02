using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using MonoEngine;
using Snake.Rooms;

namespace Snake.Entities.Controls
{
	public class ControlBack : Entity
	{
		private readonly Dictionary<Type, Action> RoomBackActions = new Dictionary<Type, Action> {
			[typeof(RoomMain)] = () => ((SnakeGame)Engine.Game).ExitGame = true,
		};

		public ControlBack() {
			this.IsPersistent = true;
			this.IsPauseable = false;
		}

		public override void onKeyDown(KeyboardEventArgs e) {
			base.onKeyDown(e);
			if (e.Key == Keys.Escape || e.Key == Keys.Back)
				this.OnBack();
		}

		public override void onButtonDown(GamePadEventArgs e) {
			base.onButtonDown(e);

			if (e.Button == Buttons.B || e.Button == Buttons.Back)
				this.OnBack();
		}

		public void OnBack() {
			Action action = null;
			this.RoomBackActions.TryGetValue(Engine.Room.GetType(), out action);
			action?.Invoke();
		}
	}
}
