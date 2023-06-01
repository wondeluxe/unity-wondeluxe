using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Array = System.Array;

namespace WondeluxeEditor
{
	public delegate bool PathPredicate(string path);

	public static class AssetDatabaseExtensions
	{
		// TODO Make these methods.

		/// <summary>
		/// Supported asset file extensions that belong in each asset folder.
		/// </summary>

		private static readonly Dictionary<string, string[]> assetFileExtensions = new Dictionary<string, string[]>()
		{
			{ "Animation", new string[] { ".anim", ".controller" } },
			{ "Audio", new string[] { ".aif", ".aiff", ".mp3", ".ogg", ".wav" } },
			{ "Compute", new string[] { ".compute" } },
			{ "Fonts", new string[] { ".otf", ".ttf" } },
			{ "Materials", new string[] { ".mat" } },
			{ "Models", new string[] { ".psb", ".3ds", ".dae", ".fbx", ".obj" } },
			{ "Objects", new string[] { ".asset" } },
			{ "Physics", new string[] { ".physicMaterial" } },
			{ "Prefabs", new string[] { ".prefab" } },
			{ "Presets", new string[] { ".preset" } },
			{ "Scenes", new string[] { ".unity" } },
			{ "Scripts", new string[] { ".cs" } },
			{ "Shaders", new string[] { ".shader" } },
			{ "Sprites", new string[] { ".jpeg", ".jpg", ".png", ".tif", ".tiff" } },
			{ "Textures", new string[] { ".jpeg", ".jpg", ".png", ".renderTexture", ".tif", ".tiff" } }
		};

		/// <summary>
		/// Asset folder names for supported file extensions.
		/// </summary>

		private static readonly Dictionary<string, string[]> assetFolderNames = new Dictionary<string, string[]>()
		{
			{ ".anim", new string[] { "Animation" } },
			{ ".controller", new string[] { "Animation" } },
			{ ".aif", new string[] { "Audio" } },
			{ ".aiff", new string[] { "Audio" } },
			{ ".mp3", new string[] { "Audio" } },
			{ ".ogg", new string[] { "Audio" } },
			{ ".wav", new string[] { "Audio" } },
			{ ".compute", new string[] { "Compute" } },
			{ ".otf", new string[] { "Fonts" } },
			{ ".ttf", new string[] { "Fonts" } },
			{ ".mat", new string[] { "Materials" } },
			{ ".3ds", new string[] { "Models" } },
			{ ".dae", new string[] { "Models" } },
			{ ".fbx", new string[] { "Models" } },
			{ ".obj", new string[] { "Models" } },
			{ ".asset", new string[] { "Objects" } },
			{ ".physicMaterial", new string[] { "Physics" } },
			{ ".prefab", new string[] { "Prefabs" } },
			{ ".preset", new string[] { "Presets" } },
			{ ".unity", new string[] { "Scenes" } },
			{ ".cs", new string[] { "Scripts" } },
			{ ".shader", new string[] { "Shaders" } },
			{ ".jpeg", new string[] { "Sprites", "Textures" } },
			{ ".jpg", new string[] { "Sprites", "Textures" } },
			{ ".png", new string[] { "Sprites", "Textures" } },
			{ ".psb", new string[] { "Models" } },
			{ ".tif", new string[] { "Sprites", "Textures" } },
			{ ".tiff", new string[] { "Sprites", "Textures" } },
			{ ".renderTexture", new string[] { "Textures" } }
		};

		/// <summary>
		/// Gets the root directory of the Unity project (containing Assets folder and solution) on the user's system.
		/// </summary>

		// Application.dataPath includes Assets folder, GetDirectoryName strips this.

		public static string ProjectPath => Path.GetDirectoryName(Application.dataPath);

		/// <summary>
		/// Search the asset database for an asset's path.
		/// </summary>
		/// <param name="name">Name of the asset to search for.</param>
		/// <returns>Path of the first asset found with the given name.</returns>

		public static string FindAssetPath(string name)
		{
			string[] guids = AssetDatabase.FindAssets(name);

			foreach (string guid in guids)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);

				if (Path.GetFileName(path) == name || Path.GetFileNameWithoutExtension(path) == name)
				{
					return path;
				}
			}

			Debug.LogError($"Asset '{name}' not found.");

			return null;
		}

		/// <summary>
		/// Search the asset database for an asset's path.
		/// </summary>
		/// <param name="filter">Search data. This can be a label, asset type or a combination of both. See <see cref="https://docs.unity3d.com/ScriptReference/AssetDatabase.FindAssets.html">AssetDatabase.FindAssets</see> for more information.</param>
		/// <param name="name">Name of the asset to search for.</param>
		/// <returns>Path of the first asset found with the given name.</returns>

		public static string FindAssetPath(string filter, string name)
		{
			string[] guids = AssetDatabase.FindAssets($"{filter} {name}");

			foreach (string guid in guids)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);

				if (Path.GetFileName(path) == name || Path.GetFileNameWithoutExtension(path) == name)
				{
					return path;
				}
			}

			Debug.LogError($"Asset '{name}' not found.");

			return null;
		}

		/// <summary>
		/// Returns the path of the first asset found of a given type and name in a specified folder.
		/// The type parameter must be a valid search type string for use with AssetDatabase.FindAssets.
		/// </summary>
		/// <param name="filter">Search data. This can be a label, asset type or a combination of both. See <see cref="https://docs.unity3d.com/ScriptReference/AssetDatabase.FindAssets.html">AssetDatabase.FindAssets</see> for more information.</param>
		/// <param name="name">Name of the asset to search for.</param>
		/// <param name="folders">Folders where the search will start.</param>
		/// <returns>Path of the first asset found with the given name.</returns>

		public static string FindAssetPath(string filter, string name, params string[] folders)
		{
			string[] guids = AssetDatabase.FindAssets($"{filter} {name}", folders);

			foreach (string guid in guids)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);

				if (Path.GetFileName(path) == name || Path.GetFileNameWithoutExtension(path) == name)
				{
					return path;
				}
			}

			Debug.LogError($"Asset '{name}' not found.");

			return null;
		}

		/// <summary>
		/// Creates a filter string for AssetDatabase searches.
		/// </summary>
		/// <param name="labels">Asset labels to filter for.</param>
		/// <returns>Filter string.</returns>

		public static string LabelFilter(params string[] labels)
		{
			return $"l:{string.Join(" l:", labels)}";
		}

		/// <summary>
		/// Creates a filter string for AssetDatabase searches.
		/// </summary>
		/// <param name="types">Asset types to filter for.</param>
		/// <returns>Filter string.</returns>

		public static string TypeFilter(params string[] types)
		{
			return $"t:{string.Join(" t:", types)}";
		}

		/// <summary>
		/// Returns the paths of all assets found at a given directory's path.
		/// </summary>
		/// <param name="path">Path of the directory, relative to the project folder.</param>
		/// <param name="recursive">If true, the method will search all sub-directories, otherwise it will only search the directory given.</param>
		/// <returns>Array of asset paths found.</returns>

		public static string[] GetAssetPaths(string path, bool recursive)
		{
			string projectPath = ProjectPath;
			string searchPath = $"{projectPath}/{path}";

			SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

			// Search for meta files, as we know all meta files belong to assets in the project.
			// This prevents GetFiles from returning hidden files and other files that might not be part of the project.

			string[] metaFilePaths = Directory.GetFiles(searchPath, "*.meta", searchOption);

			List<string> assetPaths = new List<string>();

			foreach (string metaFilePath in metaFilePaths)
			{
				string assetFilePath = metaFilePath.Substring(0, metaFilePath.Length - 5);

				if (Directory.Exists(assetFilePath))
				{
					continue;
				}

				// Add one to strip the leading /.

				assetPaths.Add(assetFilePath.Remove(0, projectPath.Length + 1));
			}

			return assetPaths.ToArray();
		}

		/// <summary>
		/// Returns the paths of all assets found at a given directory's path.
		/// </summary>
		/// <param name="path">Path of the directory, relative to the project folder.</param>
		/// <param name="recursive">If true, the method will search all sub-directories, otherwise it will only search the directory given.</param>
		/// <param name="predicate">Predicate to provide additional validation for which files to return.</param>
		/// <returns>Array of asset paths found.</returns>

		public static string[] GetAssetPaths(string path, bool recursive, PathPredicate predicate)
		{
			string projectPath = ProjectPath;
			string searchPath = $"{projectPath}/{path}";

			SearchOption searchOption = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

			// Search for meta files, as we know all meta files belong to assets in the project.
			// This prevents GetFiles from returning hidden files and other files that might not be part of the project.

			string[] metaFilePaths = Directory.GetFiles(searchPath, "*.meta", searchOption);

			List<string> assetPaths = new List<string>();

			foreach (string metaFilePath in metaFilePaths)
			{
				string assetFilePath = metaFilePath.Substring(0, metaFilePath.Length - 5);

				if (Directory.Exists(assetFilePath))
				{
					continue;
				}

				// Add one to strip the leading /.

				assetFilePath = assetFilePath.Remove(0, projectPath.Length + 1);

				if (predicate(assetFilePath))
				{
					assetPaths.Add(assetFilePath);
				}
			}

			return assetPaths.ToArray();
		}

		/// <summary>
		/// Returns true if a folder at a given path is an asset folder.
		/// </summary>
		/// <param name="path">Path to the folder.</param>
		/// <returns>true if the folder is an asset folder, false otherwise.</returns>

		public static bool IsAssetFolder(string path)
		{
			return assetFileExtensions.ContainsKey(Path.GetFileName(path));
		}

		/// <summary>
		/// Returns true if an asset at a given path is in an asset folder.
		/// </summary>
		/// <param name="path">Path to the asset.</param>
		/// <returns>true if the asset is in an asset folder, false otherwise.</returns>

		public static bool IsInAssetFolder(string path)
		{
			string extension = Path.GetExtension(path);

			if (assetFolderNames.TryGetValue(extension, out string[] folderNames))
			{
				string folderPath = Path.GetDirectoryName(path);
				string folderName = Path.GetFileName(folderPath);

				return (Array.IndexOf(folderNames, folderName) != -1);
			}

			Debug.LogError($"Unknown file extension '{extension}'.");

			return false;
		}

		/// <summary>
		/// Returns the valid asset folder names for an asset at a given path.
		/// </summary>
		/// <param name="path">Path to the asset.</param>
		/// <returns>Valid folder names for the asset.</returns>

		public static string[] GetAssetFolderNames(string path)
		{
			string extension = Path.GetExtension(path);

			if (assetFolderNames.TryGetValue(extension, out string[] folderNames))
			{
				return folderNames;
			}

			Debug.LogError($"Unknown file extension '{extension}'.");

			return new string[] { "UNKNOWN FILE EXTENSION" };
		}

		/// <summary>
		/// Returns the file extension for an asset.
		/// </summary>
		/// <param name="assetObject">The asset to return the file extension for.</param>
		/// <returns>String file extension, including the leading ".".</returns>

		public static string GetAssetFileExtension(Object assetObject)
		{
			return Path.GetExtension(AssetDatabase.GetAssetPath(assetObject));
		}

		/// <summary>
		/// Loads the first asset found with a given name.
		/// </summary>
		/// <param name="name">Name of the asset to load.</param>
		/// <returns>The loaded asset.</returns>

		public static T LoadAsset<T>(string name) where T : Object
		{
			string path = FindAssetPath(name);

			if (path != name)
			{
				return AssetDatabase.LoadAssetAtPath<T>(path);
			}

			return default;
		}

		/// <summary>
		/// Load the first asset found at the given path. If the asset doesn't exist, a new one is created and returned.
		/// </summary>
		/// <param name="path">Path to the asset.</param>
		/// <returns>The loaded or created asset.</returns>

		public static T LoadOrCreateAssetAtPath<T>(string path) where T : Object, new()
		{
			T asset = AssetDatabase.LoadAssetAtPath<T>(path);

			if (asset == null)
			{
				asset = new T();
				AssetDatabase.CreateAsset(asset, path);
			}

			return asset;
		}

		/// <summary>
		/// Load all sprites in the given textures.
		/// </summary>
		/// <param name="textures">Textures containing sprites to be loaded.</param>
		/// <returns>A dictionory contining all of the loaded sprites, keyed by their name.</returns>

		public static Dictionary<string, Sprite> LoadSprites(params Texture2D[] textures)
		{
			Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

			foreach (Texture2D texture in textures)
			{
				string path = AssetDatabase.GetAssetPath(texture);

				Object[] assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);

				foreach (Object asset in assets)
				{
					sprites.Add(asset.name, asset as Sprite);
				}
			}

			return sprites;
		}
	}
}