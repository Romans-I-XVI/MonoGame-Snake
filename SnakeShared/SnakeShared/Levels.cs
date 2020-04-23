using System;
using System.Collections.Generic;
using System.IO;
using MonoEngine;
using Newtonsoft.Json;

namespace Snake
{
	public static class Levels
	{
		private static bool Initialized = false;
		private static string[] LevelJSON;

		public static void Init() {
			if (Levels.Initialized)
				return;

			const int level_count = 12;
			Levels.LevelJSON = new string[level_count];
			var assembly = System.Reflection.Assembly.GetExecutingAssembly();

			for (int i = 0; i < level_count; i++) {
				int level_int = i + 1;
				string resource_name = "Snake." + Engine.Game.Content.RootDirectory +  ".json.level_" + level_int + ".json";
				using (Stream path = assembly.GetManifestResourceStream(resource_name)) {
					if (path == null)
						throw new NullReferenceException();

					using (var reader = new StreamReader(path)) {
						Levels.LevelJSON[i] = reader.ReadToEnd();
					}
				}
			}

			Levels.Initialized = true;
		}

		public static LevelData Load(int level_index) {
			if (!Levels.Initialized)
				Levels.Init();

			return JsonConvert.DeserializeObject<LevelData>(Levels.LevelJSON[level_index]);
		}
	}
}
