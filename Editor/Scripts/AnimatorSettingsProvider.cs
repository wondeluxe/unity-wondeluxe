using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WondeluxeEditor
{
	public class AnimatorSettingsProvider : SettingsProvider
	{
		private readonly SerializedObject serializedObject;

		public AnimatorSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords) : base(path, scopes, keywords)
		{
			serializedObject = new SerializedObject(AnimatorSettings.instance);
		}

		public override void OnGUI(string searchContext)
		{
			HideFlags defaultHideFlags = AnimatorSettings.instance.hideFlags;
			float defaultLabelWidth = EditorGUIUtility.labelWidth;

			AnimatorSettings.instance.hideFlags = HideFlags.None;

			SerializedProperty serializedProperty = serializedObject.GetIterator();

			// Skip of the script reference.
			serializedProperty.NextVisible(true);

			EditorGUILayout.Space();
			EditorGUI.indentLevel++;

			while (serializedProperty.NextVisible(false))
			{
				EditorGUILayout.PropertyField(serializedProperty);
			}

			EditorGUIUtility.labelWidth = defaultLabelWidth;
			EditorGUI.indentLevel--;

			// Unity docs use WithoutUndo version of method, just following their example, don't know why undo should be disabled.
			// serializedObject.ApplyModifiedPropertiesWithoutUndo();

			if (serializedObject.ApplyModifiedPropertiesWithoutUndo())
			{
				AnimatorSettings.instance.SaveAsset();
				AnimatorSettings.instance.UpdateAnimatorControllers();
			}

			AnimatorSettings.instance.hideFlags = defaultHideFlags;
		}

		[SettingsProvider]
		public static SettingsProvider Register()
		{
			return new AnimatorSettingsProvider("Project/Animators", SettingsScope.Project, GetSearchKeywordsFromGUIContentProperties<AnimatorSettings>());
		}
	}
}