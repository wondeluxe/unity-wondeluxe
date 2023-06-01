using System;
using System.Collections.Generic;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for Type.
	/// </summary>

	public static class TypeExtensions
	{
		/// <summary>
		/// Check if the Type is an array or generic List.
		/// </summary>
		/// <param name="type">The Type to check.</param>
		/// <returns><c>true</c> if the Type is an array or generic List, otherwise <c>false</c>.</returns>

		public static bool IsArrayOrList(this Type type)
		{
			// Same method as used in Unity's EditorExtensionMethods:
			// https://github.com/Unity-Technologies/UnityCsReference/blob/90a56242216dd93f531dcad02e824d3fea8a3ab1/Editor/Mono/Utils/EditorExtensionMethods.cs#L42

			return (type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)));
		}

		/// <summary>
		/// Gets the element type of an array or list type.
		/// </summary>
		/// <param name="type">The array or list type to get the element type for.</param>
		/// <returns>A <c>Type</c>, or <c>null</c>.</returns>

		public static Type GetArrayOrListElementType(this Type type)
		{
			if (type.IsArray)
			{
				return type.GetElementType();
			}

			if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
			{
				return type.GetGenericArguments()[0];
			}

			return null;
		}
	}
}