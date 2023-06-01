using System;

namespace Wondeluxe
{
	/// <summary>
	/// Structure representing a screen resolution.
	/// </summary>

	[Serializable]
	public struct ScreenResolution
	{
		/// <summary>
		/// Screen width.
		/// </summary>

		public float Width;

		/// <summary>
		/// Screen height.
		/// </summary>

		public float Height;

		/// <summary>
		/// Aspect ratio of the screen's width divided by its height.
		/// </summary>

		public float AspectRatio
		{
			get => Width / Height;
		}

		/// <summary>
		/// Initializes a new ScreenResolution.
		/// </summary>
		/// <param name="width">The screen width.</param>
		/// <param name="height">The screen height.</param>

		public ScreenResolution(float width, float height)
		{
			Width = width;
			Height = height;
		}

		/// <summary>
		/// Returns a string representation of the ScreenResolution.
		/// </summary>
		/// <returns>A string representation of the ScreenResolution.</returns>

		public override string ToString()
		{
			return $"({Width} x {Height})";
		}
	}
}