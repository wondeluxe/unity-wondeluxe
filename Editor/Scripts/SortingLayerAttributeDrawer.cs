using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Wondeluxe;

namespace WondeluxeEditor
{
	[CustomPropertyDrawer(typeof(SortingLayerAttribute))]
	public class SortingLayerAttributeDrawer : WondeluxePropertyDrawer
	{
		public override bool HasCustomLayout => true;

		public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// BeginProperty/EndProperty calls not required.
			// Called in WondeluxePropertyDrawer before/after OnCustomGUI.

			switch (property.propertyType)
			{
				case SerializedPropertyType.Integer:
					OnIntGUI(position, property, label);
					break;
				case SerializedPropertyType.String:
					OnStringGUI(position, property, label);
					break;
				default:
					throw new Exception($"SortingLayer attribute used on unsupported property type ({property.propertyType}). SortingLayer attribute may only be used on int or string properties.");
			}
		}

		private void OnIntGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			int layerID = property.intValue;
			string layerName = SortingLayer.IDToName(layerID);

			string[] valueLabels = GetSortingLayerNames();
			int valueIndex = Mathf.Clamp(Array.IndexOf(valueLabels, layerName), 0, valueLabels.Length - 1);

			valueIndex = EditorGUIExtensions.DrawOptionsField(label.text, valueIndex, valueLabels, ref position);

			layerName = valueLabels[valueIndex];
			layerID = SortingLayer.NameToID(layerName);

			property.intValue = layerID;
		}

		private void OnStringGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			string layerName = property.stringValue;

			string[] valueLabels = GetSortingLayerNames();
			int valueIndex = Mathf.Clamp(Array.IndexOf(valueLabels, layerName), 0, valueLabels.Length - 1);

			valueIndex = EditorGUIExtensions.DrawOptionsField(label.text, valueIndex, valueLabels, ref position);

			layerName = valueLabels[valueIndex];

			property.stringValue = layerName;
		}

		private static string[] GetSortingLayerNames()
		{
			Type internalEditorUtilityType = typeof(InternalEditorUtility);
			PropertyInfo sortingLayerNamesInfo = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
			return sortingLayerNamesInfo.GetValue(null) as string[];
		}
	}
}