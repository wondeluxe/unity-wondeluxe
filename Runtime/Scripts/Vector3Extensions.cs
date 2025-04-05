using System.Text.RegularExpressions;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for Vector3.
	/// </summary>

	public static class Vector3Extensions
	{
		/// <summary>
		/// Convert a string representation of a Vector3 to a Vector3.
		/// </summary>
		/// <param name="value">A string representation of a Vector3.</param>
		/// <returns>The Vector3 represented by <c>value</c>.</returns>

		public static Vector3 Parse(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return default;
			}

			Regex regex = new Regex(@"[-]?\d+([\.,](?=\d)\d+)?(e?[+-]\d+)?");
			MatchCollection matches = regex.Matches(value);

			float x = float.Parse(matches[0].Value);
			float y = float.Parse(matches[1].Value);
			float z = float.Parse(matches[2].Value);

			return new Vector3(x, y, z);
		}

		/// <summary>
		/// Clamps euler angles between -180 and 180 degrees.
		/// </summary>
		/// <param name="angles">The euler angles to clamp.</param>
		/// <returns>The clamped euler angles.</returns>

		public static Vector3 ClampAngles(Vector3 angles)
		{
			float x = angles.x % 360f;
			float y = angles.y % 360f;
			float z = angles.z % 360f;

			x = (x < -180f) ? (x + 360f) : (x > 180f) ? (x - 360f) : x;
			y = (y < -180f) ? (y + 360f) : (y > 180f) ? (y - 360f) : y;
			z = (z < -180f) ? (z + 360f) : (z > 180f) ? (z - 360f) : z;

			return new Vector3(x, y, z);
		}

		/// <summary>
		/// Calculates the required velocity to reach a destination.
		/// </summary>
		/// <param name="from">The current position.</param>
		/// <param name="to">The destination position.</param>
		/// <param name="velocity">The current velocity.</param>
		/// <param name="power">The maximum acceleration/deceleration.</param>
		/// <param name="maxSpeed">The maximum speed.</param>
		/// <param name="deltaTime">The delta time to step with.</param>
		/// <returns>The velocity after a step by the given delta time.</returns>

		public static Vector3 VelocityTo(Vector3 from, Vector3 to, Vector3 velocity, float power, float maxSpeed, float deltaTime)
		{
			Debug.Assert(power > 0f, "Parameter power must be a positive value.");

			Vector3 separation = to - from;
			float distance = separation.magnitude;
			float speed = velocity.magnitude;
			float deltaPower = power * deltaTime;

			if (distance <= deltaPower && speed <= deltaPower)
			{
				// Snap to target.

				return separation / deltaTime;
			}

			// Accelerate or decelerate to the required speed.
			// Target speed is calculated as speed needed to stop from the current distance using the kinematic equation: vf² = vi² + 2 • a • d
			// Distance used should be the lower of the distance needed to stop while moving at max speed and current distance from target.

			Vector3 nextPosition = from + velocity * deltaTime;
			Vector3 nextSeparation = to - nextPosition;
			float nextDistance = nextSeparation.magnitude;

			float maxStoppingDistance = (maxSpeed * maxSpeed) / (2f * power);

			float targetSpeed = Mathf.Sqrt(2f * power * Mathf.Min(nextDistance, maxStoppingDistance));

			Vector3 targetVelocity = separation.normalized * targetSpeed;

			return velocity + Vector3.ClampMagnitude(targetVelocity - velocity, deltaPower);
		}
	}
}