using System;
using UnityEngine;
using UnityEditor;
using Wondeluxe;

using Object = UnityEngine.Object;

namespace WondeluxeEditor
{
	public static class EditorGUILayoutExtensions
	{
		/// <summary>
		/// Make a field for an object value.
		/// </summary>
		/// <param name="label">Label to display in front of or above the field.</param>
		/// <param name="value">The value to edit.</param>
		/// <param name="options">An optional list of layout options that specify extra layout properties.</param>
		/// <returns>The edited value entered by the user.</returns>

		public static object ValueField(string label, object value, params GUILayoutOption[] options)
		{
			// Casts required for some types to suppress errors.
			// Consider ordering values in approximate order of commonness.

			if (value is bool boolValue)
				return EditorGUILayout.Toggle(label, boolValue, options);

			if (value is int intValue)
				return EditorGUILayout.IntField(label, intValue, options);

			if (value is uint uintValue)
				return EditorGUILayout.IntField(label, (int)uintValue, options);

			if (value is short shortValue)
				return EditorGUILayout.IntField(label, shortValue, options);

			if (value is ushort ushortValue)
				return EditorGUILayout.IntField(label, ushortValue, options);

			if (value is long longValue)
				return EditorGUILayout.LongField(label, longValue, options);

			if (value is ulong ulongValue)
				return EditorGUILayout.LongField(label, (long)ulongValue, options);

			if (value is float floatValue)
				return EditorGUILayout.FloatField(label, floatValue, options);

			if (value is double doubleValue)
				return EditorGUILayout.DoubleField(label, doubleValue, options);

			if (value is string stringValue)
				return EditorGUILayout.TextField(label, stringValue, options);

			if (value is Vector2 vector2Value)
				return EditorGUILayout.Vector2Field(label, vector2Value, options);

			if (value is Vector2Int vector2IntValue)
				return EditorGUILayout.Vector2IntField(label, vector2IntValue, options);

			if (value is Vector3 vector3Value)
				return EditorGUILayout.Vector3Field(label, vector3Value, options);

			if (value is Vector3Int vector3IntValue)
				return EditorGUILayout.Vector3IntField(label, vector3IntValue, options);

			if (value is Vector4 vector4Value)
				return EditorGUILayout.Vector4Field(label, vector4Value, options);

			if (value is Rect rectValue)
				return EditorGUILayout.RectField(label, rectValue, options);

			if (value is RectInt rectIntValue)
				return EditorGUILayout.RectIntField(label, rectIntValue, options);

			if (value is Bounds boundsValue)
				return EditorGUILayout.BoundsField(label, boundsValue, options);

			if (value is BoundsInt boundsIntValue)
				return EditorGUILayout.BoundsIntField(label, boundsIntValue, options);

			if (value is Color colorValue)
				return EditorGUILayout.ColorField(label, colorValue, options);

			if (value is LayerMask layerMaskValue)
				return EditorGUILayout.LayerField(label, layerMaskValue, options);

			if (value is AnimationCurve curveValue)
				return EditorGUILayout.CurveField(label, curveValue, options);

			// TODO Implement method to decide if scene objects should be allowed. Maybe just check if it's a ScriptableObject or asset type?

			if (value is Object objectValue)
				return EditorGUILayout.ObjectField(label, objectValue, value.GetType(), true, options);

			Type valueType = value.GetType();

			if (valueType.IsEnum)
			{
				object[] flagsAttributes = valueType.GetCustomAttributes(typeof(FlagsAttribute), false);

				if (flagsAttributes != null || flagsAttributes.Length > 0)
					return EditorGUILayout.EnumFlagsField(label, (Enum)value, options);

				return EditorGUILayout.EnumPopup(label, (Enum)value, options);
			}

			if (valueType.IsArrayOrList())
			{
				throw new Exception($"Array and list values not supported. Label: '{label}'.");
			}

			throw new Exception($"{label} object type ({value.GetType()}) not supported.");
		}
	}
}