using UnityEngine;
using UnityEditor;
using Wondeluxe;

namespace WondeluxeEditor
{
	[CustomPropertyDrawer(typeof(ScreenResolution))]
	public class ScreenResolutionDrawer : WondeluxePropertyDrawer
	{
		public override bool HasCustomLayout => true;

		public override float GetCustomPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, false);
		}

		public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// BeginProperty/EndProperty calls not required.
			// Called in WondeluxePropertyDrawer before/after OnCustomGUI.

			ScreenResolution screenResolution = property.GetValue<ScreenResolution>();

			position = EditorGUI.PrefixLabel(position, label);

			float fieldWidth = (position.width - EditorGUIExtensions.SubLabelSpacing * 2f) / 3f;

			GUIContent widthLabel = new GUIContent("W");
			GUIContent heightLabel = new GUIContent("H");
			GUIContent aspectLabel = new GUIContent($"({GetAspectRatio(screenResolution)})");

			float widthLabelWidth = EditorStyles.label.CalcSize(widthLabel).x;
			float heightLabelWidth = EditorStyles.label.CalcSize(heightLabel).x;

			Rect widthPosition = new Rect(position.x, position.y, fieldWidth, position.height);
			Rect heightPosition = new Rect(position.x + fieldWidth + EditorGUIExtensions.SubLabelSpacing, position.y, fieldWidth, position.height);
			Rect aspectPosition = new Rect(position.x + (fieldWidth + EditorGUIExtensions.SubLabelSpacing) * 2f, position.y, fieldWidth, position.height);

			int indentLevel = EditorGUI.indentLevel;
			float labelWidth = EditorGUIUtility.labelWidth;

			EditorGUI.indentLevel = 0;
			EditorGUIUtility.labelWidth = Mathf.Max(widthLabelWidth, heightLabelWidth);

			screenResolution.Width = EditorGUI.FloatField(widthPosition, widthLabel, screenResolution.Width);
			screenResolution.Height = EditorGUI.FloatField(heightPosition, heightLabel, screenResolution.Height);

			EditorGUI.LabelField(aspectPosition, aspectLabel);

			EditorGUI.indentLevel = indentLevel;
			EditorGUIUtility.labelWidth = labelWidth;

			property.FindPropertyRelative("Width").floatValue = screenResolution.Width;
			property.FindPropertyRelative("Height").floatValue = screenResolution.Height;
		}

		private static string GetAspectRatio(ScreenResolution screenResolution)
		{
			if (screenResolution.Width == 0 || screenResolution.Height == 0)
			{
				return "–";
			}

			float resolutionFactor = MathExtensions.HCF(screenResolution.Width, screenResolution.Height);
			float aspectWidth = screenResolution.Width / resolutionFactor;
			float aspectHeight = screenResolution.Height / resolutionFactor;

			return $"{aspectWidth}:{aspectHeight}";
		}
	}
}