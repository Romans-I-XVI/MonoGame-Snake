using System.Collections.Generic;

namespace Snake
{
	public enum AvailableTextures {}

	public enum AvailableFonts {}

	public enum AvailableMusic {}

	public enum AvailableSounds {}

	public static class CustomContentLocations
	{
		public static readonly Dictionary<AvailableTextures, string> TextureLocations = new Dictionary<AvailableTextures, string>();
	}
}
