using System;
using System.Collections.Generic;
using System.Reflection;

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

		/// <summary>
		/// Searches for a field, using the specified binding constraints, in a given Type or one of
		/// its base types.
		/// </summary>
		/// <remarks>
		/// The FlattenHierarchy BindingFlag won't surface private fields in base types, so this 
		/// method must be used in instances that the field being searched for is or could be private.
		/// </remarks>
		/// <param name="type">The Type to search.</param>
		/// <param name="name">The name of the field to search for.</param>
		/// <param name="bindingAttr"> bitwise combination of the enumeration values that specify how the search is conducted.</param>
		/// <returns>An object representing the field that matches the specified requirements, if found; otherwise, <c>null</c>.</returns>

		public static FieldInfo GetFieldInHierarchy(this Type type, string name, BindingFlags bindingAttr)
		{
			// Traverse up the type's hierarchy to account for serialized private fields in parent classes.

			Type parentType = type;

			while (parentType != null)
			{
				FieldInfo fieldInfo = parentType.GetField(name, bindingAttr);

				if (fieldInfo != null)
				{
					return fieldInfo;
				}

				parentType = parentType.BaseType;
			}

			return null;
		}
	}
}