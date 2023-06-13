using System.Text.RegularExpressions;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for Vector2.
	/// </summary>

	public static class Vector2Extensions
	{
		#region Instance methods

		/// <summary>
		/// Check if both the x and y values of the vector are equal to zero.
		/// </summary>
		/// <param name="vector">The vector to check.</param>
		/// <returns><c>true</c> if both the x and y values are zero, otherwise <c>false</c>.</returns>

		public static bool IsZero(this Vector2 vector)
		{
			return (Mathf.Abs(vector.x) <= float.Epsilon && Mathf.Abs(vector.y) <= float.Epsilon);
		}

		/// <summary>
		/// Check if either the x or y value of the vector is at least a given size.
		/// </summary>
		/// <param name="vector">The vector to check.</param>
		/// <param name="minSize">The minimum size to check for.</param>
		/// <returns><c>true</c> if either the x or y value is >= <c>minSize</c>, otherwise <c>false</c>.</returns>

		public static bool HasDimension(this Vector2 vector, float minSize)
		{
			return (Mathf.Abs(vector.x) >= minSize || Mathf.Abs(vector.y) >= minSize);
		}

		#endregion

		#region Static methods

		/// <summary>
		/// Get the angle in radians between two vectors.
		/// </summary>
		/// <param name="from">The first vector.</param>
		/// <param name="to">The second vector.</param>
		/// <returns>The angle between <c>to</c> and <c>from</c> in radians.</returns>

		public static float Radians(Vector2 from, Vector2 to)
		{
			float denominator = Mathf.Sqrt(from.sqrMagnitude * to.sqrMagnitude);

			if (denominator < Vector2.kEpsilonNormalSqrt)
				return 0f;

			float dot = Mathf.Clamp(Vector2.Dot(from, to) / denominator, -1f, 1f);

			return Mathf.Acos(dot);
		}

		/// <summary>
		/// Rotate a vector by the specified angle (in radians).
		/// </summary>
		/// <param name="vector">The vector to rotate.</param>
		/// <param name="angle">The angle in radians to rotate the vector by.</param>
		/// <returns>The rotated vector.</returns>

		public static Vector2 Rotate(Vector2 vector, float angle)
		{
			return Rotate(vector.x, vector.y, angle);
		}

		/// <summary>
		/// Rotate a vector by the specified angle (in radians).
		/// </summary>
		/// <param name="x">The x component of the vector to rotate.</param>
		/// <param name="y">The y component of the vector to rotate.</param>
		/// <param name="angle">The angle in radians to rotate the vector by.</param>
		/// <returns>The rotated vector.</returns>

		public static Vector2 Rotate(float x, float y, float angle)
		{
			float cos = Mathf.Cos(angle);
			float sin = Mathf.Sin(angle);

			return new Vector2(cos * x - sin * y, sin * x + cos * y);
		}

		/// <summary>
		/// Rotate a vector by the specified angle (in radians) around a given origin/pivot point.
		/// </summary>
		/// <param name="vector">The vector to rotate.</param>
		/// <param name="angle">The angle in radians to rotate the vector by.</param>
		/// <param name="origin">The origin to rotate around.</param>
		/// <returns>The rotated vector.</returns>

		public static Vector2 Rotate(Vector2 vector, float angle, Vector2 origin)
		{
			float dx = vector.x - origin.x;
			float dy = vector.y - origin.y;

			float cos = Mathf.Cos(angle);
			float sin = Mathf.Sin(angle);

			float x = origin.x + (cos * dx - sin * dy);
			float y = origin.y + (sin * dx + cos * dy);

			return new Vector2(x, y);
		}

		/// <summary>
		/// Rotate a vector 90° in the specified direction.
		/// </summary>
		/// <param name="vector">The vector to rotate.</param>
		/// <param name="direction">The direction to rotate in. A positive value will rotate the vector clockwise, a negative value will rotate the vector anti-clockwise. A value of 0 will not rotate the vector.</param>
		/// <returns>The rotated vector.</returns>

		public static Vector2 Rotate90(Vector2 vector, float direction)
		{
			if (direction > 0f)
			{
				return new Vector2(vector.y, -vector.x);
			}

			if (direction < 0f)
			{
				return new Vector2(-vector.y, vector.x);
			}

			return vector;
		}

		/// <summary>
		/// Create a copy of a vector with a specified magnitude.
		/// </summary>
		/// <param name="vector">The vector to copy.</param>
		/// <param name="magnitude">The magnitude.</param>
		/// <returns>A new Vector2 with a magnitude of <c>magnitude</c>.</returns>

		public static Vector2 SetMagnitude(Vector2 vector, float magnitude)
		{
			return vector.normalized * magnitude;
		}

		/// <summary>
		/// Create a copy of a vector with the x and y components set to their respective signs.
		/// -1 for negative values and 1 for all others (including 0).
		/// </summary>
		/// <param name="vector">The vector to copy.</param>
		/// <returns>A new vector with the x and y components set to their respective signs.</returns>

		public static Vector2 Sign(Vector2 vector)
		{
			return new Vector2(Mathf.Sign(vector.x), Mathf.Sign(vector.y));
		}

		/// <summary>
		/// Create a copy of a vector with the x and y components set to their respective signs.
		/// -1 for negative values and 1 for all others (including 0).
		/// </summary>
		/// <param name="vector">The vector to copy.</param>
		/// <returns>A new vector with the x and y components set to their respective signs.</returns>

		public static Vector2Int SignToInt(Vector2 vector)
		{
			return new Vector2Int(MathExtensions.SignToInt(vector.x), MathExtensions.SignToInt(vector.y));
		}

		/// <summary>
		/// Create a copy of a vector with the x and y components set to their respective signs.
		/// Component values of 0 are returned unchanged, while negative values are set to -1 and positive values are set to 1.
		/// </summary>
		/// <param name="vector">The vector to copy.</param>
		/// <returns>A new vector with the x and y components set to their respective signs.</returns>

		public static Vector2 ExplicitSign(Vector2 vector)
		{
			return new Vector2(MathExtensions.ExplicitSign(vector.x), MathExtensions.ExplicitSign(vector.y));
		}

		/// <summary>
		/// Create a copy of a vector with the x and y components set to their respective signs.
		/// Component values of 0 are returned unchanged, while negative values are set to -1 and positive values are set to 1.
		/// </summary>
		/// <param name="vector">The vector to copy.</param>
		/// <returns>A new vector with the x and y components set to their respective signs.</returns>

		public static Vector2Int ExplicitSignToInt(Vector2 vector)
		{
			return new Vector2Int(MathExtensions.ExplicitSignToInt(vector.x), MathExtensions.ExplicitSignToInt(vector.y));
		}

		/// <summary>
		/// Convert a string representation of a Vector2 to a Vector2.
		/// </summary>
		/// <param name="value">A string representation of a Vector2.</param>
		/// <returns>The Vector2 represented by <c>value</c>.</returns>

		public static Vector2 Parse(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return default;
			}

			Regex regex = new Regex(@"[-]?\d+([\.,](?=\d)\d+)?(e?[+-]\d+)?");
			MatchCollection matches = regex.Matches(value);

			float x = float.Parse(matches[0].Value);
			float y = float.Parse(matches[1].Value);

			return new Vector2(x, y);
		}

		#endregion
	}
}