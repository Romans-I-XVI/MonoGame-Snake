using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoEngine;
using Snake.Enums;
using Snake.GameEvents;

namespace Snake.Entities
{
	public class Scoreboard : Entity
	{
		private const int Width = 380;
		private const int Height = 80;
		private const float SnakePartSize = 5;
		private int CurrentScore = 0;
		private int HighScore = 0;
		private bool IsNewHighscore = false;
		private readonly Region BackgroundRegion;
		private readonly Region SnakeRegion;

		public Scoreboard() {
			this.Depth = -100;
			this.Position = new Vector2(Engine.Game.CanvasWidth / 2 - Scoreboard.Width / 2, 30 + 30 + 20);

			this.BackgroundRegion = new Region(ContentHolder.Get(Settings.CurrentBackground), (int)this.Position.X, (int)this.Position.Y, Scoreboard.Width, Scoreboard.Height, 0, 0);
			this.SnakeRegion = new Region(ContentHolder.Get(Settings.CurrentSnake));

			string data = SaveDataHandler.LoadData(Settings.CurrentSaveFilePath);
			bool success = (data != null && int.TryParse(data, out this.HighScore));
			if (!success) {
				this.SaveHighscore();
			}
		}

		public override void onDraw(SpriteBatch sprite_batch) {
			base.onDraw(sprite_batch);

			var scale = new Vector2(Scoreboard.SnakePartSize / this.SnakeRegion.GetWidth());

			// Draw the borders
			sprite_batch.Draw(this.BackgroundRegion, this.Position + new Vector2(0, 0), Color.White);
			for (int x = 0; x < 76; x++) {
				sprite_batch.Draw(this.SnakeRegion, this.Position + new Vector2(x * Scoreboard.SnakePartSize, 0), Color.White, 0, scale);
				sprite_batch.Draw(this.SnakeRegion, this.Position + new Vector2(x * Scoreboard.SnakePartSize, Scoreboard.Height - Scoreboard.SnakePartSize), Color.White, 0, scale);
			}

			for (int y = 0; y < 16; y++) {
				sprite_batch.Draw(this.SnakeRegion, this.Position + new Vector2(0, y * Scoreboard.SnakePartSize), Color.White, 0, scale);
				sprite_batch.Draw(this.SnakeRegion, this.Position + new Vector2(Scoreboard.Width / 2 - Scoreboard.SnakePartSize / 2, y * Scoreboard.SnakePartSize), Color.White, 0, scale);
				sprite_batch.Draw(this.SnakeRegion, this.Position + new Vector2(Scoreboard.Width - Scoreboard.SnakePartSize, y * Scoreboard.SnakePartSize), Color.White, 0, scale);
			}

			// Draw the words "Current" and "Best"
			for (int i = 0; i < Scoreboard.LetterLocations.Length; i++) {
				sprite_batch.Draw(this.SnakeRegion, this.Position + Scoreboard.LetterLocations[i], Color.White, 0, scale);
			}

			// Draw the scores
			var scores = new Dictionary<Sides, int> {
				[Sides.Left] = this.CurrentScore,
				[Sides.Right] = this.HighScore,
			};

			foreach (var kv in scores) {
				var side = kv.Key;
				int score = kv.Value;

				string score_text = score.ToString();
				int[] score_numbers = new int[score_text.Length];
				for (int i = 0; i < score_text.Length; i++) {
					char c = score_text[i];
					score_numbers[i] = int.Parse(c.ToString());
				}

				var center_x = this.Position + new Vector2(Scoreboard.Width / 2f, 0);
				var offsets = this.GetNumberDrawOffsets(side, score_numbers.Length);
				if (offsets == null)
					continue;

				for (int i = 0; i < score_numbers.Length; i++) {
					int number = score_numbers[i];
					var offset = offsets[i];
					foreach (var part_offset in LogoDrawData.ScoreNumberLocations[number]) {
						sprite_batch.Draw(this.SnakeRegion, center_x + offset + part_offset, Color.White, 0, scale);
					}
				}
			}
		}

		public override void onGameEvent(GameEvent game_event) {
			base.onGameEvent(game_event);

			if (game_event is SnakePartDestroyedEvent) {
				this.CurrentScore++;
				if (this.CurrentScore > this.HighScore) {
					this.HighScore = this.CurrentScore;
					this.IsNewHighscore = true;
				}
			} else if (game_event is SnakeDestructionDoneEvent) {
				if (this.IsNewHighscore) {
					this.SaveHighscore();
					StatTracker.PostStats(true);
				}
			}
		}

		private void SaveHighscore() {
			SaveDataHandler.SaveData(this.HighScore.ToString(), Settings.CurrentSaveFilePath);
		}

		private Vector2[] GetNumberDrawOffsets(Sides side, int digits) {
			var base_offset = new Vector2(94 * (int)side, 45);

			switch (digits) {
				case 1:
					return new[] {
						base_offset + new Vector2(-10, 0)
					};
				case 2:
					return new[] {
						base_offset + new Vector2(-21, 0),
						base_offset + new Vector2(2, 0)
					};
				case 3:
					return new[] {
						base_offset + new Vector2(-10 - 23, 0),
						base_offset + new Vector2(-10, 0),
						base_offset + new Vector2(-10 + 23, 0)
					};
				case 4:
					return new[] {
						base_offset + new Vector2(-21 - 23, 0),
						base_offset + new Vector2(-21, 0),
						base_offset + new Vector2(-21 + 23, 0),
						base_offset + new Vector2(-21 + 23 * 2, 0)
					};
				case 5:
					return new[] {
						base_offset + new Vector2(-10 - 23 * 2, 0),
						base_offset + new Vector2(-10 - 23, 0),
						base_offset + new Vector2(-10, 0),
						base_offset + new Vector2(-10 + 23, 0),
						base_offset + new Vector2(-10 + 23 * 2, 0)
					};
				default:
					return null;
			}
		}

		private static readonly Vector2[] LetterLocations = {
			// Current
			new Vector2(166, 30),
			new Vector2(166, 25),
			new Vector2(166, 20),
			new Vector2(173, 15),
			new Vector2(168, 15),
			new Vector2(163, 15),
			new Vector2(158, 15),
			new Vector2(150, 15),
			new Vector2(150, 30),
			new Vector2(150, 20),
			new Vector2(150, 25),
			new Vector2(145, 25),
			new Vector2(140, 20),
			new Vector2(135, 15),
			new Vector2(135, 20),
			new Vector2(135, 25),
			new Vector2(135, 30),
			new Vector2(121, 22),
			new Vector2(116, 22),
			new Vector2(126, 30),
			new Vector2(121, 30),
			new Vector2(116, 30),
			new Vector2(111, 30),
			new Vector2(111, 25),
			new Vector2(111, 20),
			new Vector2(126, 15),
			new Vector2(121, 15),
			new Vector2(116, 15),
			new Vector2(111, 15),
			new Vector2(102, 30),
			new Vector2(102, 25),
			new Vector2(102, 18),
			new Vector2(97, 23),
			new Vector2(92, 23),
			new Vector2(97, 15),
			new Vector2(92, 15),
			new Vector2(87, 30),
			new Vector2(87, 25),
			new Vector2(87, 20),
			new Vector2(87, 15),
			new Vector2(78, 30),
			new Vector2(78, 25),
			new Vector2(78, 18),
			new Vector2(73, 23),
			new Vector2(68, 23),
			new Vector2(73, 15),
			new Vector2(68, 15),
			new Vector2(63, 30),
			new Vector2(63, 25),
			new Vector2(63, 20),
			new Vector2(63, 15),
			new Vector2(55, 15),
			new Vector2(55, 20),
			new Vector2(55, 25),
			new Vector2(52, 30),
			new Vector2(47, 30),
			new Vector2(42, 30),
			new Vector2(39, 25),
			new Vector2(39, 20),
			new Vector2(39, 15),
			new Vector2(16, 20),
			new Vector2(16, 25),
			new Vector2(21, 30),
			new Vector2(26, 30),
			new Vector2(31, 27),
			new Vector2(31, 18),
			new Vector2(26, 15),
			new Vector2(21, 15),
			// Best
			new Vector2(319, 30),
			new Vector2(319, 25),
			new Vector2(319, 20),
			new Vector2(326, 15),
			new Vector2(321, 15),
			new Vector2(316, 15),
			new Vector2(311, 15),
			new Vector2(297, 22),
			new Vector2(292, 22),
			new Vector2(287, 19),
			new Vector2(292, 15),
			new Vector2(297, 15),
			new Vector2(302, 15),
			new Vector2(302, 26),
			new Vector2(297, 30),
			new Vector2(292, 30),
			new Vector2(287, 30),
			new Vector2(273, 22),
			new Vector2(268, 22),
			new Vector2(278, 30),
			new Vector2(273, 30),
			new Vector2(268, 30),
			new Vector2(263, 30),
			new Vector2(263, 25),
			new Vector2(263, 20),
			new Vector2(278, 15),
			new Vector2(273, 15),
			new Vector2(268, 15),
			new Vector2(263, 15),
			new Vector2(254, 26),
			new Vector2(254, 18),
			new Vector2(249, 22),
			new Vector2(244, 22),
			new Vector2(249, 30),
			new Vector2(244, 30),
			new Vector2(249, 15),
			new Vector2(244, 15),
			new Vector2(239, 30),
			new Vector2(239, 25),
			new Vector2(239, 20),
			new Vector2(239, 15)
		};
	}
}
