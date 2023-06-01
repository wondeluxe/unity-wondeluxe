using System;

namespace Wondeluxe
{
	/// <summary>
	/// Represents a numerical range.
	/// </summary>

	[Serializable]
	public struct Range
	{
		/// <summary>
		/// The minimum value of the range.
		/// </summary>

		public float Min;

		/// <summary>
		/// The maximum value of the range.
		/// </summary>

		public float Max;

		/// <summary>
		/// Instantiates a new Range with minimum and maximum values.
		/// </summary>
		/// <param name="min">The minimum value of the range.</param>
		/// <param name="max">The maximum value of the range.</param>

		public Range(float min, float max)
		{
			Min = min;
			Max = max;
		}

		/// <summary>
		/// The length (or size) of the Range.
		/// </summary>

		public float Length
		{
			get => (Max - Min);
		}

		/// <summary>
		/// Returns the string representation of the Range.
		/// </summary>
		/// <returns>The string representation of the Range.</returns>

		public override string ToString()
		{
			return $"({Min}, {Max})";
		}
	}
}