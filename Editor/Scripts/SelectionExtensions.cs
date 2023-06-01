using System.IO;
using UnityEditor;
using UnityEngine;

namespace WondeluxeEditor
{
	public static class SelectionExtensions
	{
		/// <summary>
		/// Gets the currently selected folder.
		/// </summary>
		/// <returns>Path to the folder within the project.</returns>

		public static string GetFolder()
		{
			Object[] selectedObjects = Selection.GetFiltered<Object>(SelectionMode.Assets);

			if (selectedObjects != null && selectedObjects.Length > 0)
			{
				string projectPath = AssetDatabaseExtensions.ProjectPath;

				foreach (Object selectedObject in selectedObjects)
				{
					string assetPath = AssetDatabase.GetAssetPath(selectedObject);

					string filePath = Path.Combine(projectPath, assetPath);

					//Debug.Log($"File path: {filePath}");

					FileAttributes fileAttributes = File.GetAttributes(filePath);

					if ((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
					{
						return assetPath;
					}
				}
			}

			// No folder is selected.

			return null;
		}
	}
}