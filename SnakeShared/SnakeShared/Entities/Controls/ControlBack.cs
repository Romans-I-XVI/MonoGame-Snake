using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using MonoEngine;
using Snake.Enums;
using Snake.Rooms;

namespace Snake.Entities.Controls
{
	public class ControlBack : Entity
	{
		private readonly VirtualButton BackButton = new VirtualButton();

		private readonly Dictionary<Type, Action> RoomBackActions = new Dictionary<Type, Action> {
			[typeof(RoomMain)] = () => ((SnakeGame)Engine.Game).ExitGame = true,
			[typeof(RoomPlay)] = () => {
				if (Settings.CurrentGameRoom == GameRooms.Classic || Settings.CurrentGameRoom == GameRooms.Open)
					Engine.ChangeRoom<RoomMain>();
				else
					Engine.ChangeRoom<RoomLevels>();
			},
			[typeof(RoomLevels)] = () => {
				Settings.CurrentGameRoom = GameRooms.Level1;
				Engine.ChangeRoom<RoomMain>();
			}
		};

		public ControlBack() {
			this.IsPersistent = true;
			this.IsPauseable = false;
			this.BackButton.AddKey(Keys.Escape);
			this.BackButton.AddKey(Keys.Back);
			this.BackButton.AddButton(Buttons.B);
			this.BackButton.AddButton(Buttons.Back);
		}

		public override void onUpdate(float dt) {
			base.onUpdate(dt);
			if (this.BackButton.IsPressed()) {
				Action action = null;
				this.RoomBackActions.TryGetValue(Engine.Room.GetType(), out action);
				action?.Invoke();
			}
		}
	}
}
