using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Object = UnityEngine.Object;

namespace WondeluxeEditor
{
	public static class EditorGUIExtensions
	{
		#region Internal delegates

		private delegate T EditorGUIFieldDelegate<T>(Rect rect, T value);
		private delegate T EditorGUILabeledFieldDelegate<T>(Rect rect, string label, T value);

		#endregion

		#region Static fields

		// This value shouldn't be used. Use EditorGUI.IndentedRect(position) instead.
		//public const float IndentationWidth = 30f;

		private static readonly MethodInfo drawDefaultPropertyFieldMethodInfo = Type.GetType("UnityEditor.EditorGUI, UnityEditor").GetMethod("DefaultPropertyField", BindingFlags.Static | BindingFlags.NonPublic);
		private static readonly MethodInfo vector2FieldMethodInfo = Type.GetType("UnityEditor.EditorGUI, UnityEditor").GetMethod("Vector2Field", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(Rect), typeof(Vector2) }, null);
		private static readonly MethodInfo vector2IntFieldMethodInfo = Type.GetType("UnityEditor.EditorGUI, UnityEditor").GetMethod("Vector2IntField", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(Rect), typeof(Vector2Int) }, null);
		private static readonly MethodInfo vector3FieldMethodInfo = Type.GetType("UnityEditor.EditorGUI, UnityEditor").GetMethod("Vector3Field", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(Rect), typeof(Vector3) }, null);
		private static readonly MethodInfo vector3IntFieldMethodInfo = Type.GetType("UnityEditor.EditorGUI, UnityEditor").GetMethod("Vector3IntField", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(Rect), typeof(Vector3Int) }, null);
		private static readonly MethodInfo vector4FieldMethodInfo = Type.GetType("UnityEditor.EditorGUI, UnityEditor").GetMethod("Vector4FieldNoIndent", BindingFlags.Static | BindingFlags.NonPublic, null, new Type[] { typeof(Rect), typeof(Vector4) }, null);

		private static readonly object[] methodParams2 = new object[2];
		private static readonly object[] methodParams3 = new object[3];

		#endregion

		#region Public constants

		/// <summary>
		/// Width of indentation per indent level.
		/// Value matches <c>kIndentPerLevel</c> as defined in <a href="https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/EditorGUI.cs"><c>EdutirGUI</c></a> source.
		/// </summary>

		public const float IndentWidth = 15f;

		/// <summary>
		/// Horizontal spacing between fields in a multi-property field.
		/// Value matches <c>kSpacingSubLabel</c> as defined in <a href="https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/EditorGUI.cs"><c>EdutirGUI</c></a> source.
		/// </summary>

		public const float SubLabelSpacing = 4f;

		#endregion

		#region Static API

		/// <summary>
		/// Makes an X and Y field for entering a Vector2.
		/// </summary>
		/// <param name="position">Rectangle on the screen to use for the field.</param>
		/// <param name="value">The value to edit.</param>
		/// <returns>The Vector2 value entered by the user.</returns>

		public static Vector2 Vector2Field(Rect position, Vector2 value)
		{
			methodParams2[0] = position;
			methodParams2[1] = value;

			return (Vector2)vector2FieldMethodInfo.Invoke(null, methodParams2);
		}

		/// <summary>
		/// Makes an X and Y field for entering a Vector2Int.
		/// </summary>
		/// <param name="position">Rectangle on the screen to use for the field.</param>
		/// <param name="value">The value to edit.</param>
		/// <returns>The Vector2Int value entered by the user.</returns>

		public static Vector2Int Vector2IntField(Rect position, Vector2Int value)
		{
			methodParams2[0] = position;
			methodParams2[1] = value;

			return (Vector2Int)vector2IntFieldMethodInfo.Invoke(null, methodParams2);
		}

		/// <summary>
		/// Makes an X, Y and Z field for entering a Vector3.
		/// </summary>
		/// <param name="position">Rectangle on the screen to use for the field.</param>
		/// <param name="value">The value to edit.</param>
		/// <returns>The Vector3 value entered by the user.</returns>

		public static Vector3 Vector3Field(Rect position, Vector3 value)
		{
			methodParams2[0] = position;
			methodParams2[1] = value;

			return (Vector3)vector3FieldMethodInfo.Invoke(null, methodParams2);
		}

		/// <summary>
		/// Makes an X, Y and Z field for entering a Vector3Int.
		/// </summary>
		/// <param name="position">Rectangle on the screen to use for the field.</param>
		/// <param name="value">The value to edit.</param>
		/// <returns>The Vector3Int value entered by the user.</returns>

		public static Vector3Int Vector3IntField(Rect position, Vector3Int value)
		{
			methodParams2[0] = position;
			methodParams2[1] = value;

			return (Vector3Int)vector3IntFieldMethodInfo.Invoke(null, methodParams2);
		}

		/// <summary>
		/// Makes an X, Y, Z and W field for entering a Vector4.
		/// </summary>
		/// <param name="position">Rectangle on the screen to use for the field.</param>
		/// <param name="value">The value to edit.</param>
		/// <returns>The Vector4 value entered by the user.</returns>

		public static Vector4 Vector4Field(Rect position, Vector4 value)
		{
			methodParams2[0] = position;
			methodParams2[1] = value;

			return (Vector4)vector4FieldMethodInfo.Invoke(null, methodParams2);
		}

		/// <summary>
		/// Returns a copy of <c>position</c> that's only a single line high (using <c>EditorGUIUtility.singleLineHeight</c>).
		/// Useful when drawing properties that provide full position rects for children.
		/// </summary>
		/// <param name="position">Source rect to get the single line rect from.</param>
		/// <returns>A copy of <c>position</c> that's only a single line high.</returns>

		public static Rect SingleLineRect(Rect position)
		{
			return new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
		}

		/// <summary>
		/// Returns a copy of <c>position</c> that's only tall enough as required to draw the field for <c>property</c> in the Inspector.
		/// </summary>
		/// <param name="position">Source rect to get the property's rect from.</param>
		/// <param name="property">Property to get the position rect for.</param>
		/// <param name="includeChildren">If <c>true</c> the returned rect will include the height required for the property's children, otherwise only the height needed for the property's control itself will be used.</param>
		/// <returns>A copy of <c>position</c> that's only tall enough as required to draw the field for <c>property</c>.</returns>

		public static Rect PropertyRect(Rect position, SerializedProperty property, bool includeChildren)
		{
			return new Rect(position.x, position.y, position.width, EditorGUI.GetPropertyHeight(property, includeChildren));
		}

		/// <summary>
		/// Subtracts the rect of a single drawn control/item from a total position rect.
		/// </summary>
		/// <param name="position">The full position rect.</param>
		/// <param name="rect">The rect of the drawn control.</param>
		/// <param name="spacing">How much space to separate the drawn control with.</param>
		/// <returns>A reduced position rect.</returns>

		public static Rect DrawnFieldRect(Rect position, Rect rect, float spacing)
		{
			return new Rect(position.x, position.y + rect.height + spacing, position.width, position.height - rect.height - spacing);
		}

		/// <summary>
		/// Draws a field for a SerializedProperty in the Editor and adjusts the remaining available space in a position rect. Uses the label of the property itself.
		/// </summary>
		/// <param name="property">The SerializedProperty to make a field for.</param>
		/// <param name="position">Rectangle on the screen to use for the property field, this will be adjusted to include only the space remaining.</param>
		/// <returns><c>true</c> if the property has children and is expanded; otherwise <c>false</c>.</returns>

		public static bool DrawPropertyField(SerializedProperty property, ref Rect position)
		{
			return DrawPropertyField(property, new GUIContent(property.displayName), true, ref position);
		}

		/// <summary>
		/// Draws a field for a SerializedProperty in the Editor and adjusts the remaining available space in a position rect.
		/// </summary>
		/// <param name="property">The SerializedProperty to make a field for.</param>
		/// <param name="label">The label to use.</param>
		/// <param name="includeChildren">If <c>true</c> the property including children is drawn; otherwise only the control itself (such as only a foldout but nothing below it).</param>
		/// <param name="position">Rectangle on the screen to use for the property field, this will be adjusted to include only the space remaining.</param>
		/// <returns><c>true</c> if the property has children and is expanded and <c>includeChildren</c> was set to false; otherwise <c>false</c>.</returns>

		public static bool DrawPropertyField(SerializedProperty property, string label, bool includeChildren, ref Rect position)
		{
			return DrawPropertyField(property, new GUIContent(label), includeChildren, ref position);
		}

		/// <summary>
		/// Draws a field for a SerializedProperty in the Editor and adjusts the remaining available space in a position rect.
		/// </summary>
		/// <param name="property">The SerializedProperty to make a field for.</param>
		/// <param name="label">The label to use.</param>
		/// <param name="includeChildren">If <c>true</c> the property including children is drawn; otherwise only the control itself (such as only a foldout but nothing below it).</param>
		/// <param name="position">Rectangle on the screen to use for the property field, this will be adjusted to include only the space remaining.</param>
		/// <returns><c>true</c> if the property has children and is expanded and <c>includeChildren</c> was set to false; otherwise <c>false</c>.</returns>

		public static bool DrawPropertyField(SerializedProperty property, GUIContent label, bool includeChildren, ref Rect position)
		{
			// We need to do a garbo custom implementation because EditorGUI doesn't correctly draw the foldout for arrays/lists when includeChildren is false.

			if (property.IsList())
			{
				if (!includeChildren)
				{
					// Foldouts need to be manually shifted for some reason?

					Rect totalRect = new Rect(position.x - 3f, position.y, position.width + 3f, EditorGUIUtility.singleLineHeight);
					Rect foldoutRect = new Rect(totalRect.x, totalRect.y, totalRect.width - EditorGUIUtility.fieldWidth, totalRect.height);
					Rect arraySizeRect = new Rect(totalRect.x + foldoutRect.width, totalRect.y, EditorGUIUtility.fieldWidth, totalRect.height);

					position = DrawnFieldRect(position, totalRect, EditorGUIUtility.standardVerticalSpacing);

					property.arraySize = EditorGUI.IntField(arraySizeRect, property.arraySize);

					return EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true, EditorStyles.foldoutHeader);
				}
			}

			Rect rect = PropertyRect(position, property, includeChildren);

			position = DrawnFieldRect(position, rect, EditorGUIUtility.standardVerticalSpacing);

			return EditorGUI.PropertyField(rect, property, label, includeChildren);
		}

		public static bool DrawDefaultPropertyField(SerializedProperty property, ref Rect position)
		{
			return DrawDefaultPropertyField(property, new GUIContent(property.displayName), ref position);
		}

		public static bool DrawDefaultPropertyField(SerializedProperty property, string label, ref Rect position)
		{
			return DrawDefaultPropertyField(property, new GUIContent(label), ref position);
		}

		public static bool DrawDefaultPropertyField(SerializedProperty property, GUIContent label, ref Rect position)
		{
			Rect rect = SingleLineRect(position);

			position = DrawnFieldRect(position, rect, EditorGUIUtility.standardVerticalSpacing);

			methodParams3[0] = rect;
			methodParams3[1] = property;
			methodParams3[2] = label;

			return (bool)drawDefaultPropertyFieldMethodInfo.Invoke(null, methodParams3);
		}

		public static bool DrawToggle(bool value, ref Rect position)
		{
			return DrawField<bool>(ref position, value, EditorGUI.Toggle);
		}

		public static bool DrawToggle(string label, bool value, ref Rect position)
		{
			return DrawField<bool>(ref position, label, value, EditorGUI.Toggle);
		}

		public static int DrawIntField(int value, ref Rect position)
		{
			return DrawField<int>(ref position, value, EditorGUI.IntField);
		}

		public static int DrawIntField(string label, int value, ref Rect position)
		{
			return DrawField<int>(ref position, label, value, EditorGUI.IntField);
		}

		public static long DrawLongField(long value, ref Rect position)
		{
			return DrawField<long>(ref position, value, EditorGUI.LongField);
		}

		public static long DrawLongField(string label, long value, ref Rect position)
		{
			return DrawField<long>(ref position, label, value, EditorGUI.LongField);
		}

		public static float DrawFloatField(float value, ref Rect position)
		{
			return DrawField<float>(ref position, value, EditorGUI.FloatField);
		}

		public static float DrawFloatField(string label, float value, ref Rect position)
		{
			return DrawField<float>(ref position, label, value, EditorGUI.FloatField);
		}

		public static double DrawDoubleField(double value, ref Rect position)
		{
			return DrawField<double>(ref position, value, EditorGUI.DoubleField);
		}

		public static double DrawDoubleField(string label, double value, ref Rect position)
		{
			return DrawField<double>(ref position, label, value, EditorGUI.DoubleField);
		}

		public static string DrawTextField(string value, ref Rect position)
		{
			return DrawField<string>(ref position, value, EditorGUI.TextField);
		}

		public static string DrawTextField(string label, string value, ref Rect position)
		{
			return DrawField<string>(ref position, label, value, EditorGUI.TextField);
		}

		public static Vector2 DrawVector2Field(string label, Vector2 value, ref Rect position)
		{
			return DrawField<Vector2>(ref position, label, value, EditorGUI.Vector2Field);
		}

		public static Vector2Int DrawVector2IntField(string label, Vector2Int value, ref Rect position)
		{
			return DrawField<Vector2Int>(ref position, label, value, EditorGUI.Vector2IntField);
		}

		public static Vector3 DrawVector3Field(string label, Vector3 value, ref Rect position)
		{
			return DrawField<Vector3>(ref position, label, value, EditorGUI.Vector3Field);
		}

		public static Vector3Int DrawVector3IntField(string label, Vector3Int value, ref Rect position)
		{
			return DrawField<Vector3Int>(ref position, label, value, EditorGUI.Vector3IntField);
		}

		public static Vector4 DrawVector4Field(string label, Vector4 value, ref Rect position)
		{
			return DrawField<Vector4>(ref position, label, value, EditorGUI.Vector4Field);
		}

		public static Rect DrawRectField(string label, Rect value, ref Rect position)
		{
			return DrawField<Rect>(ref position, label, value, EditorGUI.RectField);
		}

		public static RectInt DrawRectIntField(string label, RectInt value, ref Rect position)
		{
			return DrawField<RectInt>(ref position, label, value, EditorGUI.RectIntField);
		}

		public static Bounds DrawBoundsField(string label, Bounds value, ref Rect position)
		{
			return DrawField<Bounds>(ref position, label, value, EditorGUI.BoundsField);
		}

		public static BoundsInt DrawBoundsIntField(string label, BoundsInt value, ref Rect position)
		{
			return DrawField<BoundsInt>(ref position, label, value, EditorGUI.BoundsIntField);
		}

		public static Color DrawColorField(string label, Color value, ref Rect position)
		{
			return DrawField<Color>(ref position, label, value, EditorGUI.ColorField);
		}

		public static LayerMask DrawLayerField(string label, LayerMask value, ref Rect position)
		{
			return DrawField<int>(ref position, label, value, EditorGUI.LayerField);
		}

		public static AnimationCurve DrawCurveField(string label, AnimationCurve value, ref Rect position)
		{
			return DrawField<AnimationCurve>(ref position, label, value, EditorGUI.CurveField);
		}

		public static Enum DrawEnumField(string label, Enum value, ref Rect position)
		{
			return DrawEnumField(ref position, label, value.GetType(), value);
		}

		public static T DrawObjectField<T>(string label, T value, bool allowSceneObjects, ref Rect position) where T : Object
		{
			return DrawObjectField(label, value, typeof(T), allowSceneObjects, ref position) as T;
		}

		public static Object DrawObjectField(string label, Object value, Type type, bool allowSceneObjects, ref Rect position)
		{
			Rect rect = SingleLineRect(position);

			position = DrawnFieldRect(position, rect, EditorGUIUtility.standardVerticalSpacing);

			return EditorGUI.ObjectField(rect, label, value, type, allowSceneObjects);
		}

		public static bool DrawFoldout(string label, bool expanded, ref Rect position)
		{
			return DrawFoldout(label, expanded, true, EditorStyles.foldout, ref position);
		}

		public static bool DrawFoldout(string label, bool expanded, bool toggleOnLabelClick, GUIStyle style, ref Rect position)
		{
			Rect rect = SingleLineRect(position);

			position = DrawnFieldRect(position, rect, EditorGUIUtility.standardVerticalSpacing);

			return EditorGUI.Foldout(rect, expanded, label, toggleOnLabelClick, style);
		}

		public static int DrawOptionsField(string label, int selectedIndex, string[] displayedOptions, ref Rect position)
		{
			Rect rect = SingleLineRect(position);

			position = DrawnFieldRect(position, rect, EditorGUIUtility.standardVerticalSpacing);

			rect = EditorGUI.PrefixLabel(rect, new GUIContent(label));

			return EditorGUI.Popup(rect, selectedIndex, displayedOptions);
		}

		public static Rect ObjectPickerButtonRect(Rect position)
		{
			RectOffset objectFieldPadding = EditorStyles.objectField.padding;

			return new Rect(position.max.x - objectFieldPadding.right, position.y, objectFieldPadding.right, position.height);
		}

		public static bool DrawButton(Rect rect, string text, ref Rect position)
		{
			position = DrawnFieldRect(position, rect, EditorGUIUtility.standardVerticalSpacing);

			return GUI.Button(rect, text);
		}

		/// <summary>
		/// Make a field for an object value.
		/// </summary>
		/// <param name="label">Label to display in front of or above the field.</param>
		/// <param name="value">The value to edit.</param>
		/// <param name="position">Rectangle on the screen to use for the property field, this will be adjusted to include only the space remaining.</param>
		/// <returns>The edited value entered by the user.</returns>

		public static object DrawValueField(string label, object value, ref Rect position)
		{
			// Casts required for some types to suppress errors.
			// Consider ordering values in approximate order of commonness.

			if (value is bool boolValue)
				return DrawToggle(label, boolValue, ref position);

			if (value is int intValue)
				return DrawIntField(label, intValue, ref position);

			if (value is uint uintValue)
				return DrawIntField(label, (int)uintValue, ref position);

			if (value is short shortValue)
				return DrawIntField(label, shortValue, ref position);

			if (value is ushort ushortValue)
				return DrawIntField(label, ushortValue, ref position);

			if (value is long longValue)
				return DrawLongField(label, longValue, ref position);

			if (value is ulong ulongValue)
				return DrawLongField(label, (long)ulongValue, ref position);

			if (value is float floatValue)
				return DrawFloatField(label, floatValue, ref position);

			if (value is double doubleValue)
				return DrawDoubleField(label, doubleValue, ref position);

			if (value is string stringValue)
				return DrawTextField(label, stringValue, ref position);

			if (value is Vector2 vector2Value)
				return DrawVector2Field(label, vector2Value, ref position);

			if (value is Vector2Int vector2IntValue)
				return DrawVector2IntField(label, vector2IntValue, ref position);

			if (value is Vector3 vector3Value)
				return DrawVector3Field(label, vector3Value, ref position);

			if (value is Vector3Int vector3IntValue)
				return DrawVector3IntField(label, vector3IntValue, ref position);

			if (value is Vector4 vector4Value)
				return DrawVector4Field(label, vector4Value, ref position);

			if (value is Rect rectValue)
				return DrawRectField(label, rectValue, ref position);

			if (value is RectInt rectIntValue)
				return DrawRectIntField(label, rectIntValue, ref position);

			if (value is Bounds boundsValue)
				return DrawBoundsField(label, boundsValue, ref position);

			if (value is BoundsInt boundsIntValue)
				return DrawBoundsIntField(label, boundsIntValue, ref position);

			if (value is Color colorValue)
				return DrawColorField(label, colorValue, ref position);

			if (value is LayerMask layerMaskValue)
				return DrawLayerField(label, layerMaskValue, ref position);

			if (value is AnimationCurve curveValue)
				return DrawCurveField(label, curveValue, ref position);

			// TODO Implement method to decide if scene objects should be allowed. Maybe just check if it's a ScriptableObject or asset type?

			if (value is Object objectValue)
				return DrawObjectField(label, objectValue, typeof(Object), true, ref position);

			Type valueType = value.GetType();

			if (valueType.IsEnum)
			{
				return DrawEnumField(ref position, label, valueType, (Enum)value);
			}

			throw new Exception($"Object type ({value.GetType()}) not supported.");
		}

		public static float GetPropertyHeight(Type propertyType, GUIContent label)
		{
			if (typeof(Object).IsAssignableFrom(propertyType))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.ObjectReference, label);

			if (propertyType.IsEnum)
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Enum, label);

			if (propertyType == typeof(bool))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Boolean, label);

			if (propertyType == typeof(int))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Integer, label);

			if (propertyType == typeof(uint))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Integer, label);

			if (propertyType == typeof(short))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Integer, label);

			if (propertyType == typeof(ushort))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Integer, label);

			if (propertyType == typeof(long))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Integer, label);

			if (propertyType == typeof(ulong))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Integer, label);

			if (propertyType == typeof(float))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Float, label);

			if (propertyType == typeof(double))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Float, label);

			if (propertyType == typeof(string))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.String, label);

			if (propertyType == typeof(Vector2))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector2, label);

			if (propertyType == typeof(Vector2Int))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector2Int, label);

			if (propertyType == typeof(Vector3))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector3, label);

			if (propertyType == typeof(Vector3Int))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector3Int, label);

			if (propertyType == typeof(Vector4))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Vector4, label);

			if (propertyType == typeof(Rect))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Rect, label);

			if (propertyType == typeof(RectInt))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.RectInt, label);

			if (propertyType == typeof(Bounds))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Bounds, label);

			if (propertyType == typeof(BoundsInt))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.BoundsInt, label);

			if (propertyType == typeof(Color))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.Color, label);

			if (propertyType == typeof(LayerMask))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.LayerMask, label);

			if (propertyType == typeof(AnimationCurve))
				return EditorGUI.GetPropertyHeight(SerializedPropertyType.AnimationCurve, label);

			Debug.LogWarning($"Type ({propertyType}) not supported.");

			return EditorGUIUtility.singleLineHeight;
		}

		#endregion

		#region Static methods

		private static Enum DrawEnumField(ref Rect position, string label, Type valueType, Enum value)
		{
			FlagsAttribute attribute = valueType.GetCustomAttribute<FlagsAttribute>(false);

			if (attribute != null)
			{
				return DrawField<Enum>(ref position, label, value, EditorGUI.EnumFlagsField);
			}

			return DrawField<Enum>(ref position, label, value, EditorGUI.EnumPopup);
		}

		private static T DrawField<T>(ref Rect position, T value, EditorGUIFieldDelegate<T> fieldDelegate)
		{
			Rect rect = SingleLineRect(position);

			position = DrawnFieldRect(position, rect, EditorGUIUtility.standardVerticalSpacing);

			return fieldDelegate(rect, value);
		}

		private static T DrawField<T>(ref Rect position, string label, T value, EditorGUILabeledFieldDelegate<T> fieldDelegate)
		{
			Rect rect = SingleLineRect(position);

			position = DrawnFieldRect(position, rect, EditorGUIUtility.standardVerticalSpacing);

			return fieldDelegate(rect, label, value);
		}

		#endregion
	}
}