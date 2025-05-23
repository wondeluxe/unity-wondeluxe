using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Wondeluxe;

using Object = UnityEngine.Object;

namespace WondeluxeEditor
{
	[FilePath("ProjectSettings/TagAndLayerScriptSettings.asset", FilePathAttribute.Location.ProjectFolder)]
	public class TagAndLayerScriptSettings : ScriptableSingleton<TagAndLayerScriptSettings>
	{
		[SerializeField]
		[Tooltip("Path to the project's \"Layer\" script. If provided, the values of the Tag Manager's layers will be written as constants.")]
		private TextAsset layerScript;

		internal void UpdateScripts()
		{
			if (layerScript == null)
			{
				return;
			}

			Object tagManager = AssetDatabase.LoadAssetAtPath<Object>("ProjectSettings/TagManager.asset");

			SerializedObject serializedTagManager = new(tagManager);
			SerializedProperty serializedLayers = serializedTagManager.FindProperty("layers");

			string[] layerDefinitions = GetLayerDefinitions(serializedLayers);

			AssetDatabaseExtensions.InjectMemberValues(layerScript, layerDefinitions);
		}

		internal void SaveAsset()
		{
			Save(true);
		}

		private void OnTagManagerSaved(string tagManagerPath)
		{
			UpdateScripts();
		}

		private void Initialize()
		{
			ProjectModificationProcessor.OnAssetSaved.Add("ProjectSettings/TagManager.asset", OnTagManagerSaved);
		}

		private static string[] GetLayerDefinitions(SerializedProperty layersProperty)
		{
			Dictionary<string, string> layers = new();

			for (int i = 0; i < layersProperty.arraySize; i++)
			{
				string layer = layersProperty.GetArrayElementAtIndex(i).stringValue;

				if (string.IsNullOrEmpty(layer))
				{
					continue;
				}

				layer = layer.ToPascal();

				if (!layers.TryAdd(layer, $"public const int {layer} = {i};"))
				{
					Debug.LogWarning($"Duplicate layer identifier found: \"{layer}\".");
				}
			}

			string[] layersArray = new string[layers.Count];

			layers.Values.CopyTo(layersArray, 0);

			return layersArray;
		}

		[InitializeOnLoad]
		private static class Initializer
		{
			static Initializer()
			{
				instance.Initialize();
			}
		}
	}
}