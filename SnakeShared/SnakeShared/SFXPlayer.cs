using MonoEngine;

namespace Snake
{
	public static class SFXPlayer
	{
		public static void Play(AvailableSounds sound, float volume = 1.0f, float pitch = 0f, float pan = 0f)
		{
			Utilities.Try(() => ContentHolder.Get(sound).Play(volume, pitch, pan));
		}
	}
}
