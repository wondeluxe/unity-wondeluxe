using System;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Wondeluxe;

namespace WondeluxeEditor
{
	[CustomPropertyDrawer(typeof(LayerAttribute))]
	public class LayerAttributeDrawer : WondeluxePropertyDrawer
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
					throw new Exception($"Layer attribute used on unsupported property type ({property.propertyType}). Layer attribute may only be used on int or string properties.");
			}
		}

		private void OnIntGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			int layer = property.intValue;
			string layerName = LayerMask.LayerToName(layer);

			string[] valueLabels = InternalEditorUtility.layers;
			int valueIndex = Mathf.Clamp(Array.IndexOf(valueLabels, layerName), 0, valueLabels.Length - 1);

			valueIndex = EditorGUIExtensions.DrawOptionsField(label.text, valueIndex, valueLabels, ref position);

			layerName = valueLabels[valueIndex];
			layer = LayerMask.NameToLayer(layerName);

			property.intValue = layer;
		}

		private void OnStringGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			string layerName = property.stringValue;

			string[] valueLabels = InternalEditorUtility.layers;
			int valueIndex = Mathf.Clamp(Array.IndexOf(valueLabels, layerName), 0, valueLabels.Length - 1);

			valueIndex = EditorGUIExtensions.DrawOptionsField(label.text, valueIndex, valueLabels, ref position);

			layerName = valueLabels[valueIndex];

			property.stringValue = layerName;
		}
	}
}