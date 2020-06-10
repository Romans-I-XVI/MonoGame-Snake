using System;
using System.Collections.Generic;

namespace Snake
{
	public enum AvailableTextures
	{
		splash,
#if ADS
		splash_ad_buffer,
#endif
		portal_0,
		portal_1,
		theme_0_background,
		theme_0_food,
		theme_0_snake,
		theme_0_wall,
		theme_1_background,
		theme_1_food,
		theme_1_snake,
		theme_1_wall,
		theme_2_background,
		theme_2_food,
		theme_2_snake,
		theme_2_wall,
		theme_3_background,
		theme_3_food,
		theme_3_snake,
		theme_3_wall,
		theme_4_background,
		theme_4_food,
		theme_4_snake,
		theme_4_wall,
		theme_5_background,
		theme_5_food,
		theme_5_snake,
		theme_5_wall,
		theme_6_background,
		theme_6_food,
		theme_6_snake,
		theme_6_wall,
		theme_7_background,
		theme_7_food,
		theme_7_snake,
		theme_7_wall,
		theme_8_background,
		theme_8_food,
		theme_8_snake,
		theme_8_wall,
	}

	public enum AvailableFonts
	{
		retro_computer
	}

	public enum AvailableMusic
	{
		background_music
	}

	public enum AvailableSounds
	{
		create_block,
		death,
		death_hit,
		destroy_part,
		eat,
		navsingle
	}

	public static class CustomContentLocations
	{
		private static Dictionary<AvailableTextures, string> _textureLocations = null;

		public static Dictionary<AvailableTextures, string> TextureLocations {
			get {
				if (CustomContentLocations._textureLocations == null) {
					CustomContentLocations._textureLocations = new Dictionary<AvailableTextures, string>();
					string[] theme_items = {
						"background",
						"food",
						"snake",
						"wall"
					};

					for (int i = 0; i <= 8; i++) {
						foreach (string item in theme_items) {
							var available_texture = Enum.Parse<AvailableTextures>("theme_" + i + "_" + item);
							string path = "textures/themes/theme_" + i + "/" + item;

							CustomContentLocations._textureLocations[available_texture] = path;
						}
					}
				}

				return CustomContentLocations._textureLocations;
			}
		}
	}
}
