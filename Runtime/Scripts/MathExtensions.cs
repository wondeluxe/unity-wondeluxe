using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Math utility methods.
	/// </summary>

	public static class MathExtensions
	{
		/// <summary>
		/// Half the value of the constant π.
		/// </summary>

		public const float HalfPI = Mathf.PI / 2f;

		/// <summary>
		/// Double the value of the constant π.
		/// </summary>

		public const float TwoPI = Mathf.PI * 2f;

		/// <summary>
		/// Gets the sign of a value as an integer.
		/// </summary>
		/// <param name="value">The value to return the sign of.</param>
		/// <returns>1 when value is positive or zero, -1 when value is negative.</returns>

		public static int SignToInt(float value)
		{
			return (value < 0f) ? -1 : 1;
		}

		/// <summary>
		/// Gets the sign of a value if it is not zero.
		/// </summary>
		/// <param name="value">The value to return the sign of.</param>
		/// <returns>1 when value is positive, -1 when value is negative, 0 otherwise.</returns>

		public static float ExplicitSign(float value)
		{
			return (value < 0f) ? -1f : (value > 0f) ? 1f : 0f;
		}

		/// <summary>
		/// Gets the sign of a value as an integer if it is not zero.
		/// </summary>
		/// <param name="value">The value to return the sign of.</param>
		/// <returns>1 when value is positive, -1 when value is negative, 0 otherwise.</returns>

		public static int ExplicitSignToInt(float value)
		{
			return (value < 0f) ? -1 : (value > 0f) ? 1 : 0;
		}

		/// <summary>
		/// Gets a rotation (in degrees) clamped between -180 and 180.
		/// </summary>
		/// <param name="rotation">The rotation to clamp.</param>
		/// <returns>The clamped rotation.</returns>

		public static float ClampRotation(float rotation)
		{
			rotation %= 360f;

			return (rotation > 180f) ? (rotation - 360f) : (rotation < -180f) ? (rotation + 360f) : rotation;
		}

		/// <summary>
		/// Gets an angle (in radians) clamped between -π and π.
		/// </summary>
		/// <param name="angle">The angle to clamp.</param>
		/// <returns>The clamped angle.</returns>

		public static float ClampAngle(float angle)
		{
			angle %= TwoPI;

			return (angle > Mathf.PI) ? (angle - TwoPI) : (angle < -Mathf.PI) ? (angle + TwoPI) : angle;
		}

		/// <summary>
		/// Gets the Highest Common Factor of two values.
		/// </summary>
		/// <param name="a">The first value.</param>
		/// <param name="b">The second value.</param>
		/// <returns>The Highest Common Factor of <c>a</c> and <c>b</c>.</returns>

		public static int HCF(int a, int b)
		{
			// Division Method to determine HCF.

			int n = (a > b) ? a : b;
			int d = (a < b) ? a : b;
			int r = n % d;

			while (r > 0)
			{
				n = d;
				d = r;
				r = n % d;
			}

			return d;
		}

		/// <summary>
		/// Gets the Highest Common Factor of two values.
		/// </summary>
		/// <param name="a">The first value.</param>
		/// <param name="b">The second value.</param>
		/// <returns>The Highest Common Factor of <c>a</c> and <c>b</c>.</returns>

		public static float HCF(float a, float b)
		{
			// Division Method to determine HCF.

			float n = (a > b) ? a : b;
			float d = (a < b) ? a : b;
			float r = n % d;

			while (r > 0)
			{
				n = d;
				d = r;
				r = n % d;
			}

			return d;
		}

		/// <summary>
		/// Finds the nearest incremental value within a range.
		/// </summary>
		/// <param name="value">The value to find the nearest incremental value to.</param>
		/// <param name="min">The minimum value of the range.</param>
		/// <param name="max">The maximum value of the range.</param>
		/// <param name="increments">The number of increments to get to <c>max</c> from <c>min</c>.</param>
		/// <returns>The nearest incremental value to <c>value</c>.</returns>

		public static float Snap(float value, float min, float max, int increments)
		{
			if (increments <= 0)
			{
				increments = 1;
			}

			value = Mathf.Clamp(value, min, max);

			float step = (max - min) / increments;

			return min + Mathf.RoundToInt((value - min) / step) * step;
		}

		/// <summary>
		/// Finds the nearest incremental value within a range.
		/// </summary>
		/// <param name="value">The value to find the nearest incremental value to.</param>
		/// <param name="min">The minimum value of the range.</param>
		/// <param name="max">The maximum value of the range.</param>
		/// <param name="increments">The number of increments to get to <c>max</c> from <c>min</c>.</param>
		/// <returns>The nearest incremental value to <c>value</c>.</returns>

		public static int Snap(int value, int min, int max, int increments)
		{
			if (increments <= 0)
			{
				increments = 1;
			}

			value = Mathf.Clamp(value, min, max);

			float step = (float)(max - min) / increments;

			return min + (int)(Mathf.RoundToInt((value - min) / step) * step);
		}
	}
}