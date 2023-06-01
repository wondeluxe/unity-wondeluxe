using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for SpriteRenderer.
	/// </summary>

	public static class SpriteRendererExtensions
	{
		/// <summary>
		/// Sets the alpha channel of the sprite's rendering color, RGB values remain unchanged.
		/// </summary>
		/// <param name="spriteRenderer">The SpriteRenderer whose alpha to set.</param>
		/// <param name="alpha">The alpha value.</param>

		public static void SetAlpha(this SpriteRenderer spriteRenderer, float alpha)
		{
			Color color = spriteRenderer.color;
			spriteRenderer.color = new Color(color.r, color.g, color.b, alpha);
		}
	}
}