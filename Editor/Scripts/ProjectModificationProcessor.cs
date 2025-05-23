using System.Collections.Generic;
using UnityEditor;

namespace WondeluxeEditor
{
	public class ProjectModificationProcessor : AssetModificationProcessor
	{
		public static readonly AssetEventListeners OnAssetSaved = new();

		private static string[] OnWillSaveAssets(string[] paths)
		{
			foreach (string path in paths)
			{
				// UnityEngine.Debug.Log($"Asset will be saved: \"{path}\".");

				OnAssetSaved.Dispatch(path);
			}

			return paths;
		}

		public delegate void AssetEventListener(string path);

		public class AssetEventListeners
		{
			private readonly Dictionary<string, HashSet<AssetEventListener>> listeners = new();

			public bool Add(string assetPath, AssetEventListener listener)
			{
				if (!listeners.TryGetValue(assetPath, out HashSet<AssetEventListener> assetListeners))
				{
					assetListeners = new HashSet<AssetEventListener>();
					listeners.Add(assetPath, assetListeners);
				}

				return assetListeners.Add(listener);
			}

			public bool Remove(string assetPath, AssetEventListener listener)
			{
				if (listeners.TryGetValue(assetPath, out HashSet<AssetEventListener> assetListeners))
				{
					return assetListeners.Remove(listener);
				}

				return false;
			}

			internal void Dispatch(string assetPath)
			{
				if (!listeners.TryGetValue(assetPath, out HashSet<AssetEventListener> assetListeners))
				{
					return;
				}

				foreach (AssetEventListener listener in assetListeners)
				{
					listener(assetPath);
				}
			}
		}
	}
}