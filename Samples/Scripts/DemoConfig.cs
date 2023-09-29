using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
#if UNITY_EDITOR
using WondeluxeEditor;
#endif

namespace Wondeluxe.Samples
{
	[CreateAssetMenu(menuName = "Wondeluxe/Samples/DemoConfig", fileName = "DemoConfig")]
	public class DemoConfig : ScriptableObject
	{
		[SerializeField]
		[Folder]
		private string dataFolder;

		[ShowInInspector]
		private readonly string fileType = "json";

#if UNITY_EDITOR
		[Button]
		private void LoadData()
		{
			string dataFolderPath = AssetDatabase.GUIDToAssetPath(dataFolder);

			string[] files = AssetDatabaseExtensions.GetAssetPaths(dataFolderPath, false, AssetIsFileType);
			string[] languages = new string[files.Length];

			for (int i = 0; i < files.Length; i++)
			{
				string content = File.ReadAllText(files[i]);
				DemoData data = JsonUtility.FromJson<DemoData>(content);

				languages[i] = data.Language;
			}

			Debug.Log($"Languages: {string.Join(", ", languages)}");
		}

		private bool AssetIsFileType(string assetPath)
		{
			Regex regex = new Regex($".{fileType}$");

			return regex.IsMatch(assetPath);
		}
#endif
	}
}