using System;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for Array.
	/// </summary>

	public static class ArrayExtensions
	{
		/// <summary>
		/// Random instance used in various extension methods.
		/// </summary>

		private static readonly Random random = new Random();

		/// <summary>
		/// Indicates whether an array is null or contains no elements.
		/// </summary>
		/// <param name="array">The array to test.</param>
		/// <returns><c>true</c> if <c>array</c> is <c>null</c> or contains no elements, otherwise <c>false</c>.</returns>

		public static bool IsNullOrEmpty(Array array)
		{
			return (array == null || array.Length == 0);
		}

		/// <summary>
		/// Resizes an array and assigns a value to the last element.
		/// </summary>
		/// <typeparam name="T">The type of elements in the array.</typeparam>
		/// <param name="array">The array to append to.</param>
		/// <param name="value">The value to assign to the last element.</param>

		public static void Append<T>(ref T[] array, T value)
		{
			Array.Resize(ref array, array.Length + 1);
			array[array.Length - 1] = value;
		}

		/// <summary>
		/// Resizes an array and copies values from another array to the end of the resized array.
		/// </summary>
		/// <typeparam name="T">The type of elements in the array.</typeparam>
		/// <param name="array">The array to append to.</param>
		/// <param name="values">The values to copy to the end of <c>array</c>.</param>

		public static void Append<T>(ref T[] array, T[] values)
		{
			int index = array.Length;
			Array.Resize(ref array, array.Length + values.Length);
			Array.Copy(values, 0, array, index, values.Length);
		}

		/// <summary>
		/// Randomises the order of elements in an array.
		/// </summary>
		/// <typeparam name="T">The type of elements in the array.</typeparam>
		/// <param name="array">The array to shuffle.</param>

		public static void Shuffle<T>(T[] array)
		{
			int n = array.Length;

			while (n > 1)
			{
				int i = random.Next(0, n) % n;

				n--;

				T value = array[i];
				array[i] = array[n];
				array[n] = value;
			}
		}

		/// <summary>
		/// Return the value of a random element in an array.
		/// </summary>
		/// <typeparam name="T">The type of elements in the array.</typeparam>
		/// <param name="array">The array to the the element value from.</param>
		/// <returns>The value of an element of <c>array</c>.</returns>

		public static T Any<T>(T[] array)
		{
			return array[random.Next(array.Length)];
		}

		/// <summary>
		/// Sets every element of an array.
		/// </summary>
		/// <typeparam name="T">The type of elements in the array.</typeparam>
		/// <param name="array">The array whose elements to set.</param>
		/// <param name="value">The value to set each element to.</param>

		public static void Set<T>(ref T[] array, T value)
		{
			Set(ref array, 0, array.Length, value);
		}

		/// <summary>
		/// Sets a range of elements in an array.
		/// </summary>
		/// <typeparam name="T">The type of elements in the array.</typeparam>
		/// <param name="array">The array whose elements to set.</param>
		/// <param name="index">The starting index of the range of elements to set.</param>
		/// <param name="length">The number of elements to set.</param>
		/// <param name="value">The value to set each element to.</param>

		public static void Set<T>(ref T[] array, int index, int length, T value)
		{
			try
			{
				int indexLimit = index + length;

				for (int i = index; i < indexLimit; i++)
				{
					array[i] = value;
				}
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine(exception.Message);
			}
		}

		/// <summary>
		/// Searches an array for a specified value.
		/// </summary>
		/// <typeparam name="T">The type of elements in the array.</typeparam>
		/// <param name="array">The array to search.</param>
		/// <param name="value">The value to search for.</param>
		/// <returns><c>true</c> if <c>value</c> is found, otherwise <c>false</c>.</returns>

		public static bool Contains<T>(T[] array, T value)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (value.Equals(array[i]))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Searches a range of elements in an array for a specified value.
		/// </summary>
		/// <typeparam name="T">The type of elements in the array.</typeparam>
		/// <param name="array">The array to search.</param>
		/// <param name="index">The index to start the search from.</param>
		/// <param name="length">The number of elements to search.</param>
		/// <param name="value">The value to search for.</param>
		/// <returns><c>true</c> if <c>value</c> is found, otherwise <c>false</c>.</returns>

		public static bool Contains<T>(T[] array, int index, int length, T value)
		{
			int limit = index + length;

			for (int i = index; i < limit; i++)
			{
				if (value.Equals(array[i]))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Moves the values of a range of elements in an array.
		/// </summary>
		/// <typeparam name="T">The type of elements in the array.</typeparam>
		/// <param name="array">The array whose elements to move.</param>
		/// <param name="fromIndex">The starting index to move elements from.</param>
		/// <param name="toIndex">The starting index to move elements to.</param>
		/// <param name="length">The number of elements to move.</param>

		public static void Shift<T>(T[] array, int fromIndex, int toIndex, int length)
		{
			if (toIndex < fromIndex)
			{
				for (int i = 0; i < length; i++)
				{
					array[toIndex + i] = array[fromIndex + i];
				}
			}
			else if (toIndex > fromIndex)
			{
				for (int i = length - 1; i >= 0; i--)
				{
					array[toIndex + i] = array[fromIndex + i];
				}
			}
		}
	}
}