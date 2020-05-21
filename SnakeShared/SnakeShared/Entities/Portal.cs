using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoEngine;

namespace Snake.Entities
{
	public class Portal : Entity
	{
		public const int Size = 30;
		public readonly ColliderRectangle MainCollider;
		public readonly AnimatedSprite MainSprite;
		private readonly ReadOnlyDictionary<Directions, int> DestinationIDs;
		private readonly ReadOnlyDictionary<Directions, bool> ReverseDirection;

		public Portal(int x, int y, int id, int goto_up, int goto_down, int goto_left, int goto_right, ReadOnlyDictionary<Directions, bool> reverse_direction = null) {
			this.Depth = 100;
			this.Position = new Vector2(x, y);
			this.ID = id;
			this.DestinationIDs = new ReadOnlyDictionary<Directions, int>(new Dictionary<Directions, int> {
				[Directions.Up] = goto_up,
				[Directions.Down] = goto_down,
				[Directions.Left] = goto_left,
				[Directions.Right] = goto_right
			});
			this.ReverseDirection = reverse_direction;

			var regions = new Region[] {
				new Region(ContentHolder.Get(AvailableTextures.portal_0), 0, 0, Portal.Size, Portal.Size, 0, 0),
				new Region(ContentHolder.Get(AvailableTextures.portal_1), 0, 0, Portal.Size, Portal.Size, 0, 0),
			};
			this.MainSprite = new AnimatedSprite(regions);
			this.AddSprite("main", this.MainSprite);
			this.MainCollider = this.AddColliderRectangle("main", 0, 0, Portal.Size, Portal.Size);
		}

		public Portal(PortalSpawn spawn) : this(spawn.X, spawn.Y, spawn.ID, spawn.GotoUp, spawn.GotoDown, spawn.GotoLeft, spawn.GotoRight, spawn.ReverseDirection) {}

		public Portal GetDestination(Directions direction) => Engine.GetAllInstances<Portal>().First(portal => portal.ID == this.DestinationIDs[direction]);

		public Directions GetDirection(Directions direction) {
			bool need_to_reverse = (this.ReverseDirection != null && this.ReverseDirection[direction]);

			if (need_to_reverse) {
				switch (direction) {
					case Directions.Up:
						return Directions.Down;
					case Directions.Down:
						return Directions.Up;
					case Directions.Left:
						return Directions.Right;
					case Directions.Right:
						return Directions.Left;
				}
			}

			return direction;
		}
	}
}
