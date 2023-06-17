using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for the AnimationCurve class.
	/// </summary>

	public static class AnimationCurveExtensions
	{
		/// <summary>
		/// Create an ease in curve based on the quadratic equation (t²).
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease in curve.</returns>

		public static AnimationCurve QuadraticEaseIn(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.55f, 0.085f,
				0.68f, 0.53f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease out curve based on the quadratic equation (t²).
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease out curve.</returns>

		public static AnimationCurve QuadraticEaseOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.25f, 0.46f,
				0.45f, 0.94f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease in & out curve based on the quadratic equation (t²).
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease in & out curve.</returns>

		public static AnimationCurve QuadraticEaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.455f, 0.03f,
				0.515f, 0.955f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease in curve based on the cubic equation (t³).
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease in curve.</returns>

		public static AnimationCurve CubicEaseIn(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.55f, 0.055f,
				0.675f, 0.19f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease out curve based on the cubic equation (t³).
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease out curve.</returns>

		public static AnimationCurve CubicEaseOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.215f, 0.61f,
				0.355f, 1f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease in & out curve based on the cubic equation (t³).
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease in & out curve.</returns>

		public static AnimationCurve CubicEaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.645f, 0.045f,
				0.355f, 1f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease in curve based on the quartic equation (t⁴).
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease in curve.</returns>

		public static AnimationCurve QuarticEaseIn(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.895f, 0.03f,
				0.685f, 0.22f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease out curve based on the quartic equation (t⁴).
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease out curve.</returns>

		public static AnimationCurve QuarticEaseOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.165f, 0.84f,
				0.44f, 1f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease in & out curve based on the quartic equation (t⁴).
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease in & out curve.</returns>

		public static AnimationCurve QuarticEaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.77f, 0f,
				0.175f, 1f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease in curve based on the quartic equation (t⁵).
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease in curve.</returns>

		public static AnimationCurve QuinticEaseIn(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.755f, 0.05f,
				0.855f, 0.06f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease out curve based on the quartic equation (t⁵).
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease out curve.</returns>

		public static AnimationCurve QuinticEaseOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.23f, 1f,
				0.32f, 1f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease in & out curve based on the quartic equation (t⁵).
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease in & out curve.</returns>

		public static AnimationCurve QuinticEaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.86f, 0f,
				0.07f, 1f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease in curve based on an approximated exponential equation.
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease in curve.</returns>

		public static AnimationCurve ExponentialEaseIn(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				1f, 0f,
				1f, 0f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease out curve based on an approximated exponential equation.
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease out curve.</returns>

		public static AnimationCurve ExponentialEaseOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0f, 1f,
				0f, 1f,
				1f, 1f });
		}

		/// <summary>
		/// Create an ease in & out curve based on an approximated exponential equation.
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease in & out curve.</returns>

		public static AnimationCurve ExponentialEaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.4942f, 0f,
				0.5f, 0f,
				0.5f, 0.5f,
				0.5001f, 1f,
				0.4959f, 1f,
				1f, 1f });
		}

		/// <summary>
		/// Create an ease in curve based on sine wave.
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease in curve.</returns>

		public static AnimationCurve SineEaseIn(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.47f, 0f,
				0.745f, 0.715f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease out curve based on sine wave.
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease out curve.</returns>

		public static AnimationCurve SineEaseOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.39f, 0.575f,
				0.565f, 1f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease in & out curve based on sine wave.
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease in & out curve.</returns>

		public static AnimationCurve SineEaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.45f, 0.05f,
				0.55f, 0.95f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease in curve based on the arc of a circle.
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease in curve.</returns>

		public static AnimationCurve CircularEaseIn(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.6f, 0.04f,
				0.98f, 0.335f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease out curve based on the arc of a circle.
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease out curve.</returns>

		public static AnimationCurve CircularEaseOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.075f, 0.82f,
				0.165f, 1f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an ease in & out curve based on the arc of a circle.
		/// </summary>
		/// <param name="timeStart">The start time for the ease curve.</param>
		/// <param name="valueStart">The start value for the ease curve.</param>
		/// <param name="timeEnd">The end time for the ease curve.</param>
		/// <param name="valueEnd">The end value for the ease curve.</param>
		/// <returns>An ease in & out curve.</returns>

		public static AnimationCurve CircularEaseInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.785f, 0.135f,
				0.15f, 0.86f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create a curve with anticipation at the start.
		/// </summary>
		/// <param name="timeStart">The start time for the curve.</param>
		/// <param name="valueStart">The start value for the curve.</param>
		/// <param name="timeEnd">The end time for the curve.</param>
		/// <param name="valueEnd">The end value for the curve.</param>
		/// <returns>A curve with anticipation at the start.</returns>

		public static AnimationCurve BackIn(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.6f, -0.28f,
				0.735f, 0.045f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create a curve with overshoot at the end.
		/// </summary>
		/// <param name="timeStart">The start time for the curve.</param>
		/// <param name="valueStart">The start value for the curve.</param>
		/// <param name="timeEnd">The end time for the curve.</param>
		/// <param name="valueEnd">The end value for the curve.</param>
		/// <returns>A curve with overshoot at the end.</returns>

		public static AnimationCurve BackOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.175f, 0.885f,
				0.32f, 1.275f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create a curve with anticipation at the start and overshoot at the end.
		/// </summary>
		/// <param name="timeStart">The start time for the curve.</param>
		/// <param name="valueStart">The start value for the curve.</param>
		/// <param name="timeEnd">The end time for the curve.</param>
		/// <param name="valueEnd">The end value for the curve.</param>
		/// <returns>A curve with anticipation at the start and overshoot at the end.</returns>

		public static AnimationCurve BackInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.68f, -0.55f,
				0.265f, 1.55f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create a curve to simulate a reverse bounce effect.
		/// </summary>
		/// <param name="timeStart">The start time for the curve.</param>
		/// <param name="valueStart">The start value for the curve.</param>
		/// <param name="timeEnd">The end time for the curve.</param>
		/// <param name="valueEnd">The end value for the curve.</param>
		/// <returns>A bounce curve.</returns>

		public static AnimationCurve BounceIn(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.05f, 0.035f,
				0.05f, 0.035f,
				0.090909f, 0f,
				0.2f, 0.14f,
				0.2f, 0.11f,
				0.27272728f, 0f,
				0.5f, 0.7917f,
				0.5f, 0.375f,
				0.6363636364f, 0f,
				0.8f, 0.8712f,
				1f, 1f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create a curve to simulate a bounce effect.
		/// </summary>
		/// <param name="timeStart">The start time for the curve.</param>
		/// <param name="valueStart">The start value for the curve.</param>
		/// <param name="timeEnd">The end time for the curve.</param>
		/// <param name="valueEnd">The end value for the curve.</param>
		/// <returns>A bounce curve.</returns>

		public static AnimationCurve BounceOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0f, 0f,
				0.2f, 0.1288f,
				0.3636363636f, 1f,
				0.5f, 0.625f,
				0.5f, 0.2083f,
				0.72727272f, 1f,
				0.8f, 0.89f,
				0.8f, 0.86f,
				0.90909f, 1f,
				0.95f, 0.965f,
				0.95f, 0.965f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create a curve to simulate a reverse bounce effect at the start and bounce effect at the end.
		/// </summary>
		/// <param name="timeStart">The start time for the curve.</param>
		/// <param name="valueStart">The start value for the curve.</param>
		/// <param name="timeEnd">The end time for the curve.</param>
		/// <param name="valueEnd">The end value for the curve.</param>
		/// <returns>A bounce curve.</returns>

		public static AnimationCurve BounceInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.025f, 0.0175f,
				0.025f, 0.0175f,
				0.0454545f, 0f,
				0.1f, 0.07f,
				0.1f, 0.055f,
				0.1363636f, 0f,
				0.25f, 0.39585f,
				0.25f, 0.1845f,
				0.3181818f, 0f,
				0.4f, 0.4356f,
				0.5f, 0.5f,
				0.5f, 0.5f,
				0.5f, 0.5f,
				0.6f, 0.5644f,
				0.6818181818f, 1f,
				0.75f, 0.8125f,
				0.75f, 0.60415f,
				0.86363636f, 1f,
				0.9f, 0.945f,
				0.9f, 0.93f,
				0.954545f, 1f,
				0.975f, 0.9825f,
				0.975f, 0.9825f,
				1f, 1f });
		}

		/// <summary>
		/// Create a curve to simulate a reverse elastic/wobble effect.
		/// </summary>
		/// <param name="timeStart">The start time for the curve.</param>
		/// <param name="valueStart">The start value for the curve.</param>
		/// <param name="timeEnd">The end time for the curve.</param>
		/// <param name="valueEnd">The end value for the curve.</param>
		/// <returns>An elastic curve.</returns>

		public static AnimationCurve ElasticIn(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.63f, 0f,
				0f, 0f,
				0.63f, 0f,
				0.865f, 0.25f,
				0.865f, 0.25f,
				0.925f, 0f,
				0.865f, -0.5f,
				1f, 0f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create a curve to simulate an elastic/wobble effect.
		/// </summary>
		/// <param name="timeStart">The start time for the curve.</param>
		/// <param name="valueStart">The start value for the curve.</param>
		/// <param name="timeEnd">The end time for the curve.</param>
		/// <param name="valueEnd">The end value for the curve.</param>
		/// <returns>An elastic curve.</returns>

		public static AnimationCurve ElasticOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0f, 1f,
				0.145f, 1.6f,
				0.225f, 1f,
				0.26f, 0.8f,
				0.26f, 0.8f,
				0.38f, 1f,
				1f, 1f,
				0.38f, 1f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create a curve to simulate a reverse elastic/wobble effect at the start and elastic/wobble effect at the end.
		/// </summary>
		/// <param name="timeStart">The start time for the curve.</param>
		/// <param name="valueStart">The start value for the curve.</param>
		/// <param name="timeEnd">The end time for the curve.</param>
		/// <param name="valueEnd">The end value for the curve.</param>
		/// <returns>An elastic curve.</returns>

		public static AnimationCurve ElasticInOut(float timeStart, float valueStart, float timeEnd, float valueEnd)
		{
			return FromPoints(timeStart, valueStart, timeEnd, valueEnd, new float[] {
				0f, 0f,
				0.33f, 0f,
				0f, 0f,
				0.33f, 0f,
				0.4f, -0.25f,
				0.6f, 1.25f,
				0.67f, 1f,
				1f, 1f,
				0.67f, 1f,
				1f, 1f
			});
		}

		/// <summary>
		/// Create an AnimationCurve from a set of points representing a Bézier curve.
		/// </summary>
		/// <remarks>
		/// The supplied points are given in an array of floats of length <c>2 + 6n</c>, where each element alternates between an x value (which represents time) and a y value (which represents a value).
		/// The points should be normalized between 0,0 and 1,1.
		/// </remarks>
		/// <param name="timeStart">The start time for the AnimationCurve.</param>
		/// <param name="valueStart">The start value for the AnimationCurve.</param>
		/// <param name="timeEnd">The end time for the AnimationCurve.</param>
		/// <param name="valueEnd">The end value for the AnimationCurve.</param>
		/// <param name="points">The points representing a Bézier curve.</param>
		/// <returns>An AnimationCurve matching <paramref name="points"/>.</returns>

		public static AnimationCurve FromPoints(float timeStart, float valueStart, float timeEnd, float valueEnd, float[] points)
		{
			float timeChange = timeEnd - timeStart;
			float valueChange = valueEnd - valueStart;

			int count = 1 + (points.Length - 2) / 6;

			Keyframe[] keys = new Keyframe[count];

			int k = 0;

			keys[k].time = timeStart;
			keys[k].value = valueStart;
			keys[k].inTangent = float.PositiveInfinity;
			keys[k].inWeight = 0;
			keys[k].weightedMode = WeightedMode.Both;

			for (k = 1; k < count; k++)
			{
				int n = 1 + k * 6;

				// For each iteration:
				// points[n - 7] = previous anchor point x (time)
				// points[n - 6] = previous anchor point y (value)
				// points[n - 5] = previous out control point x (time)
				// points[n - 4] = previous out control point y (value)
				// points[n - 3] = current in control point x (time)
				// points[n - 2] = current in control point y (value)
				// points[n - 1] = current anchor point x (time)
				// points[  n  ] = current anchor point y (value)

				float prevTime = timeStart + points[n - 7] * timeChange;
				float time = timeStart + points[n - 1] * timeChange;

				float deltaTime = time - prevTime;

				float prevValue = valueStart + points[n - 6] * valueChange;
				float value = valueStart + points[n] * valueChange;

				float prevOutControlTime = (timeStart + points[n - 5] * timeChange) - prevTime;
				float prevOutControlValue = (valueStart + points[n - 4] * valueChange) - prevValue;

				float inControlTime = time - (timeStart + points[n - 3] * timeChange);
				float inControlValue = value - (valueStart + points[n - 2] * valueChange);

				float prevOutTangent = (prevOutControlTime != 0f) ? (prevOutControlValue / prevOutControlTime) : 0f;
				float prevOutWeight = prevOutControlTime / deltaTime;

				float inTangent = (inControlTime != 0f) ? (inControlValue / inControlTime) : 0f;
				float inWeight = inControlTime / deltaTime;

				int a = k - 1;

				keys[a].outTangent = prevOutTangent;
				keys[a].outWeight = prevOutWeight;

				keys[k].time = time;
				keys[k].value = value;
				keys[k].inTangent = inTangent;
				keys[k].inWeight = inWeight;
				keys[k].weightedMode = WeightedMode.Both;
			}

			k = count - 1;

			keys[k].outTangent = float.PositiveInfinity;
			keys[k].outWeight = 0;

			return new AnimationCurve(keys);
		}
	}
}