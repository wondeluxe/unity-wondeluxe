using UnityEngine;
using UnityEditor;
using Wondeluxe;

namespace WondeluxeEditor
{
	[CustomPropertyDrawer(typeof(Range))]
	public class RangeDrawer : WondeluxePropertyDrawer
	{
		public override bool HasCustomLayout => true;

		public override float GetCustomPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, false);
		}

		public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Range range = (Range)property.GetValue();

			float[] subValues = new float[] { range.Min, range.Max };
			GUIContent[] subLabels = new GUIContent[] { new GUIContent("Min"), new GUIContent("Max") };

			// BeginProperty/EndProperty calls not required.
			// Called in WondeluxePropertyDrawer before/after OnCustomGUI.

			EditorGUI.MultiFloatField(position, label, subLabels, subValues);
			property.FindPropertyRelative("Min").floatValue = subValues[0];
			property.FindPropertyRelative("Max").floatValue = subValues[1];
		}
	}
}