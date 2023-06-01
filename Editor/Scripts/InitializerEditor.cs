using UnityEngine.SceneManagement;
using UnityEditor.Callbacks;
using UnityEditor.SceneManagement;
using Wondeluxe;

namespace WondeluxeEditor
{
	public static class InitializerEditor
	{
		[DidReloadScripts]
		private static void OnScriptsReloaded()
		{
			for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
			{
				Scene scene = SceneManager.GetSceneAt(i);

				if (scene.TryFindComponents<Initializer>(true, out Initializer[] initializers))
				{
					foreach (Initializer initializer in initializers)
					{
						if (initializer.SearchMode == InitializerSearchMode.OnReload)
						{
							initializer.FindInitializables();
						}
					}
				}
			}
		}
	}
}