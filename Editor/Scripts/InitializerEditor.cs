using UnityEngine.SceneManagement;
using UnityEditor.Callbacks;
#if !UNITY_2022_2_OR_NEWER
using UnityEditor.SceneManagement;
#endif
using Wondeluxe;

namespace WondeluxeEditor
{
	public static class InitializerEditor
	{
		[DidReloadScripts]
		private static void OnScriptsReloaded()
		{
#if UNITY_2022_2_OR_NEWER
			for (int i = 0; i < SceneManager.loadedSceneCount; i++)
#else
			for (int i = 0; i < EditorSceneManager.loadedSceneCount; i++)
#endif
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