using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WondeluxeEditor
{
	public class TagAndLayerScriptSettingsProvider : SettingsProvider
	{
		private readonly SerializedObject serializedObject;

		public TagAndLayerScriptSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords) : base(path, scopes, keywords)
		{
			serializedObject = new SerializedObject(TagAndLayerScriptSettings.instance);
		}

		public override void OnGUI(string searchContext)
		{
			HideFlags defaultHideFlags = TagAndLayerScriptSettings.instance.hideFlags;
			float defaultLabelWidth = EditorGUIUtility.labelWidth;

			TagAndLayerScriptSettings.instance.hideFlags = HideFlags.None;

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
				TagAndLayerScriptSettings.instance.SaveAsset();
				TagAndLayerScriptSettings.instance.UpdateScripts();
			}

			TagAndLayerScriptSettings.instance.hideFlags = defaultHideFlags;
		}

		[SettingsProvider]
		public static SettingsProvider Register()
		{
			return new TagAndLayerScriptSettingsProvider("Project/Tags and Layers/Scripts", SettingsScope.Project, GetSearchKeywordsFromGUIContentProperties<TagAndLayerScriptSettings>());
		}
	}
}