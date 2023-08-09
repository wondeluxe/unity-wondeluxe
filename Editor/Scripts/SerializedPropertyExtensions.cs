using System;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using Wondeluxe;

namespace WondeluxeEditor
{
	public static class SerializedPropertyExtensions
	{
		private const BindingFlags FieldBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
		private const BindingFlags PropertyBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase;

		/// <summary>
		/// Regular Expression for obtaining a SerializedProperty's parent property's path. The parent property's path will be found in group 1.
		/// Match will fail if the tested property path is a property directly on its serializedObject.
		/// </summary>

		private static readonly Regex ParentPathRegex = new Regex(@"(.*)(\.Array\.data\[\d+]|\.\w+)$");

		/// <summary>
		/// Regular Expression for obtaining the individual property components that make up the hierarchy of a SerializedProperty.
		/// Group 1 will contain the property's name and group 2 will contain an array/list index, if it's present.
		/// </summary>

		private static readonly Regex PropertyComponentsRegex = new Regex(@"\.?(Array\.data\[(\d+)]|\w+)");



		//public static int GetPropertyDepth(this SerializedProperty serializedProperty)
		//{
		//	MatchCollection matches = PropertyComponentsRegex.Matches(serializedProperty.propertyPath);

		//	return matches.Count - 1;
		//}

		/// <summary>
		/// Returns the attribute of type if the SerializedProperty has one.
		/// </summary>
		/// <param name="inherit">If true, the SerializedProperty's inheritance chain will be searched to find the attribute.</param>
		/// <returns>The attribute if it is present on the SerialiedProperty, null otherwise.</returns>

		public static T GetAttribute<T>(this SerializedProperty serializedProperty, bool inherit) where T : PropertyAttribute
		{
			MemberInfo memberInfo = serializedProperty.GetMemberInfo();

			if (memberInfo != null)
			{
				return memberInfo.GetCustomAttribute<T>(inherit);
			}

			return null;
		}

		/// <summary>
		/// Returns the attributes of type if the SerializedProperty has any.
		/// </summary>
		/// <param name="inherit">If true, the SerializedProperty's inheritance chain will be searched to find the attribute.</param>
		/// <returns>The attributes if any are present on the SerialiedProperty, null otherwise.</returns>

		public static T[] GetAttributes<T>(this SerializedProperty serializedProperty, bool inherit) where T : PropertyAttribute
		{
			MemberInfo memberInfo = serializedProperty.GetMemberInfo();

			if (memberInfo != null)
			{
				return memberInfo.GetCustomAttributes(typeof(T), inherit) as T[];
			}

			return null;
		}

		/// <summary>
		/// Returns the MemberInfo of the SerializedProperty.
		/// </summary>
		/// <returns>The MemberInfo of the SerializedProperty.</returns>

		public static MemberInfo GetMemberInfo(this SerializedProperty serializedProperty)
		{
			object parent = serializedProperty.GetParentObject();

			if (parent != null)
			{
				Type parentType = parent.GetType();

				FieldInfo fieldInfo = parentType.GetField(serializedProperty.name, FieldBindingFlags);

				if (fieldInfo != null)
				{
					return fieldInfo;
				}

				PropertyInfo propertyInfo = parentType.GetProperty(serializedProperty.name, PropertyBindingFlags);

				if (propertyInfo != null)
				{
					return propertyInfo;
				}

				// TODO Check parent type if desired.
			}
			else
			{
				Debug.LogWarning($"Parent object for serialized property is null.");
			}

			return null;
		}

		/// <summary>
		/// Returns the FieldInfo of the SerializedProperty.
		/// </summary>
		/// <returns>The FieldInfo of the SerializedProperty.</returns>

		public static FieldInfo GetFieldInfo(this SerializedProperty serializedProperty)
		{
			Type parentType = serializedProperty.GetParentObject().GetType();

			if (parentType.IsArrayOrList())
			{
				return null;
			}

			// Traverse up the type's hierarchy to account for serialized private fields in parent classes.

			while (parentType != null)
			{
				FieldInfo fieldInfo = parentType.GetField(serializedProperty.name, FieldBindingFlags);

				if (fieldInfo != null)
				{
					return fieldInfo;
				}

				parentType = parentType.BaseType;
			}

			return null;
		}

		/// <summary>
		/// Returns the PropertyInfo of the SerializedProperty.
		/// </summary>
		/// <returns>The PropertyInfo of the SerializedProperty.</returns>

		public static PropertyInfo GetPropertyInfo(this SerializedProperty serializedProperty)
		{
			return serializedProperty.GetParentObject().GetType().GetProperty(serializedProperty.name, PropertyBindingFlags);
		}

		/// <summary>
		/// Gets the parent SerializedProperty.
		/// </summary>
		/// <returns>A SerializedProperty if one exists, otherwise null (when the SerializedProperty is a direct property of it's serializedObject).</returns>

		public static SerializedProperty GetParentProperty(this SerializedProperty serializedProperty)
		{
			Match parentPathMatch = ParentPathRegex.Match(serializedProperty.propertyPath);

			if (parentPathMatch.Success)
			{
				return serializedProperty.serializedObject.FindProperty(parentPathMatch.Groups[1].Value);
			}

			// SerializedProperty is a root property of its serializedObject.

			return null;
		}

		/// <summary>
		/// Returns the System.Object that the SerializedProperty belongs to.
		/// </summary>
		/// <returns>The System.Object that the SerializedProperty belongs to.</returns>

		public static object GetParentObject(this SerializedProperty serializedProperty)
		{
			// Find the path to serializedProperty's parent. If the match fails, serializedProperty is a direct property on its serializedObject.
			// If a parent exists, step down the each property until we reach serializedProperty's parent.

			object target = serializedProperty.serializedObject.targetObject;

			Match parentPathMatch = ParentPathRegex.Match(serializedProperty.propertyPath);

			if (parentPathMatch.Success)
			{
				MatchCollection matches = PropertyComponentsRegex.Matches(parentPathMatch.Groups[1].Value);

				foreach (Match match in matches)
				{
					// Group 2 contains either a property name or Array.data[#].
					// Group 3 contains the Array.data index if present.

					target = (match.Groups[2].Success) ? GetArrayElementValue(target, Convert.ToInt32(match.Groups[2].Value)) : GetMemberValue(target, match.Groups[1].Value);
				}
			}

			return target;
		}

		private static object GetMemberValue(object target, string name)
		{
			if (target != null)
			{
				Type type = target.GetType();
				FieldInfo fieldInfo = type.GetField(name, FieldBindingFlags);

				if (fieldInfo == null)
				{
					PropertyInfo propertyInfo = type.GetProperty(name, PropertyBindingFlags);

					if (propertyInfo == null)
					{
						return null;
					}

					return propertyInfo.GetValue(target, null);
				}

				return fieldInfo.GetValue(target);
			}

			return null;
		}

		private static object GetArrayElementValue(object array, int index)
		{
			if (array != null)
			{
				if (array.GetType().IsGenericType && array is IList list)
				{
					return list[index];
				}

				return (array as Array).GetValue(index);
			}

			return null;
		}

		/// <summary>
		/// Returns the property's value as a System object.
		/// </summary>

		public static object GetValue(this SerializedProperty property)
		{
			// Must use GetParentObject here as property.serializedObject.targetObject will return the containing Unity object.
			// GetParentObject returns the direct parent struct or object.

			object parentObject = property.GetParentObject();
			Type parentType = parentObject.GetType();

			if (parentType.IsArrayOrList())
			{
				Regex arrayElementRegex = new Regex(@"\[(\d+)\]$");
				Match arrayElementMatch = arrayElementRegex.Match(property.propertyPath);

				if (arrayElementMatch.Success)
				{
					return GetArrayElementValue(parentObject, Convert.ToInt32(arrayElementMatch.Groups[1].Value));
				}

				throw new Exception($"Unable to get array element index for property ({property.propertyPath}).");
			}

			// Traverse up the type's hierarchy to account for serialized private fields in parent classes.

			while (parentType != null)
			{
				FieldInfo fieldInfo = parentType.GetField(property.name, FieldBindingFlags);

				if (fieldInfo != null)
				{
					return fieldInfo.GetValue(parentObject);
				}

				parentType = parentType.BaseType;
			}

			return null;
		}

		/// <summary>
		/// Returns the property's value cast to the specified type.
		/// </summary>

		public static T GetValue<T>(this SerializedProperty property)
		{
			return (T)property.GetValue();
		}

		public static bool GetImplicitBoolValue(this SerializedProperty property)
		{
			object value = property.GetValue();
			Type type = value.GetType();

			// This assumes only implicit bool is implemented.
			// TODO Need to handle multiple implicit operators.

			MethodInfo methodInfo = type.GetMethod("op_Implicit", BindingFlags.Static | BindingFlags.Public);

			return (bool)(methodInfo.Invoke(null, new object[] { value }));
		}

		/// <summary>
		/// Sets the value of the property which represents an object type.
		/// </summary>

		public static void SetObjectValue(this SerializedProperty property, object objectValue)
		{
			Type type = objectValue.GetType();

			FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);

			foreach (FieldInfo field in fields)
			{
				property.FindPropertyRelative(field.Name).SetValue(field.GetValue(objectValue));
			}
		}

		/// <summary>
		/// Returns the <c>true</c> if the property is an array or list. Used as an alternative to <c>SerializedProperty.isArray</c> which returns <c>true</c> for strings.
		/// </summary>
		/// <returns><c>true</c> if the property is an array or list, otherwise <c>false</c>.</returns>

		public static bool IsList(this SerializedProperty property)
		{
			return (property.propertyType == SerializedPropertyType.Generic && property.isArray);
		}

		/// <summary>
		/// Sets the value of the property which represents an array type.
		/// </summary>

		public static void SetArrayValue(this SerializedProperty property, object arrayValue)
		{
			if (arrayValue == null)
			{
				property.arraySize = 0;
			}
			else
			{
				object[] array = arrayValue as object[];

				property.arraySize = array.Length;

				for (int i = 0; i < array.Length; i++)
				{
					property.GetArrayElementAtIndex(i).SetValue(array[i]);
				}
			}
		}

		/// <summary>
		/// Sets the value of the property based on the property's type.
		/// </summary>

		public static void SetValue(this SerializedProperty property, object value)
		{
			if (value != null)
			{
				Type type = value.GetType();

				if (property.propertyType == SerializedPropertyType.Generic && type.IsValueType && !type.IsPrimitive && !type.IsEnum)
				{
					// Property is a struct.
					property.SetObjectValue(value);
					return;
				}

				if (property.isArray && type.IsArray)
				{
					property.SetArrayValue(value);
					return;
				}
			}

			switch (property.propertyType)
			{
				case SerializedPropertyType.Integer:
					property.intValue = (int)value;
					return;
				case SerializedPropertyType.Boolean:
					property.boolValue = (bool)value;
					return;
				case SerializedPropertyType.Float:
					property.floatValue = (float)value;
					return;
				case SerializedPropertyType.String:
					property.stringValue = value as string;
					return;
				case SerializedPropertyType.Color:
					property.colorValue = (Color)value;
					return;
				case SerializedPropertyType.ObjectReference:
					property.objectReferenceValue = value as UnityEngine.Object;
					return;
				case SerializedPropertyType.LayerMask:
					property.intValue = (int)value;
					return;
				case SerializedPropertyType.Enum:
					property.enumValueIndex = (int)value;
					return;
				case SerializedPropertyType.Vector2:
					property.vector2Value = (Vector2)value;
					return;
				case SerializedPropertyType.Vector3:
					property.vector3Value = (Vector3)value;
					return;
				case SerializedPropertyType.Vector4:
					property.vector4Value = (Vector4)value;
					return;
				case SerializedPropertyType.Rect:
					property.rectValue = (Rect)value;
					return;
				case SerializedPropertyType.AnimationCurve:
					property.animationCurveValue = value as AnimationCurve;
					return;
				case SerializedPropertyType.Bounds:
					property.boundsValue = (Bounds)value;
					return;
				case SerializedPropertyType.Quaternion:
					property.quaternionValue = (Quaternion)value;
					return;
				case SerializedPropertyType.ExposedReference:
					property.exposedReferenceValue = value as UnityEngine.Object;
					return;
				case SerializedPropertyType.Vector2Int:
					property.vector2IntValue = (Vector2Int)value;
					return;
				case SerializedPropertyType.Vector3Int:
					property.vector3IntValue = (Vector3Int)value;
					return;
				case SerializedPropertyType.RectInt:
					property.rectIntValue = (RectInt)value;
					return;
				case SerializedPropertyType.BoundsInt:
					property.boundsIntValue = (BoundsInt)value;
					return;
			}

			// Assume object type.
			property.SetObjectValue(value);
		}
	}
}