using System.IO;
using UnityEngine;
using UnityEditor;
using Wondeluxe;

namespace WondeluxeEditor
{
	[CustomPropertyDrawer(typeof(FolderAttribute))]
	public class FolderAttributeDrawer : ObjectValuePropertyDrawer<string, DefaultAsset>
	{
		protected override string GetValidValue(params Object[] objectReferences)
		{
			string projectPath = Path.GetDirectoryName(Application.dataPath);

			foreach (Object objectReference in objectReferences)
			{
				string assetPath = AssetDatabase.GetAssetPath(objectReference);

				if (string.IsNullOrEmpty(assetPath))
				{
					continue;
				}

				string path = Path.Combine(projectPath, assetPath);

				if (Directory.Exists(path))
				{
					return AssetDatabase.AssetPathToGUID(assetPath);
				}
			}

			return null;
		}

		protected override Object GetObjectReference(string stringValue)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(stringValue);

			if (string.IsNullOrEmpty(assetPath))
			{
				return null;
			}

			return AssetDatabase.LoadAssetAtPath<DefaultAsset>(assetPath);
		}

		protected override string GetObjectPickerFilter()
		{
			return "t:Folder";
		}
	}
}