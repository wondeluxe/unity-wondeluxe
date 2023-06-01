using UnityEngine;
using UnityEngine.SceneManagement;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for Scene.
	/// </summary>

	public static class SceneExtensions
	{
		/// <summary>
		/// Searches GameObjects in the scene for a component of the specified type.
		/// </summary>
		/// <returns>Returns a component if any GameObject in the scene has one attached, <c>null</c> if not.</returns>

		public static T FindComponent<T>(this Scene scene)
		{
			return scene.FindComponent<T>(false);
		}

		/// <summary>
		/// Searches GameObjects in the scene for a component of the specified type.
		/// </summary>
		/// <param name="includeInactive">Should components on inactive GameObjects be included in the search?</param>
		/// <returns>Returns a component if any GameObject in the scene has one attached, <c>null</c> if not.</returns>

		public static T FindComponent<T>(this Scene scene, bool includeInactive)
		{
			GameObject[] rootGameObjects = scene.GetRootGameObjects();

			foreach (GameObject gameObject in rootGameObjects)
			{
				if (gameObject.TryGetComponentInChildren(includeInactive, out T component))
				{
					return component;
				}
			}

			return default;
		}

		/// <summary>
		/// Searches GameObjects in the scene for a component of the specified type.
		/// </summary>
		/// <param name="component">The output argument that will contain the component or <c>null</c>.</param>
		/// <returns>Returns <c>true</c> if a component is found, <c>false</c> otherwise.</returns>

		public static bool TryFindComponent<T>(this Scene scene, out T component)
		{
			return scene.TryFindComponent<T>(false, out component);
		}

		/// <summary>
		/// Searches GameObjects in the scene for a component of the specified type.
		/// </summary>
		/// <param name="includeInactive">Should components on inactive GameObjects be included in the search?</param>
		/// <param name="component">The output argument that will contain the component or <c>null</c>.</param>
		/// <returns>Returns <c>true</c> if a component is found, <c>false</c> otherwise.</returns>

		public static bool TryFindComponent<T>(this Scene scene, bool includeInactive, out T component)
		{
			component = scene.FindComponent<T>(includeInactive);

			return (component != null);
		}

		/// <summary>
		/// Searches GameObjects in the scene for a component of the specified type.
		/// </summary>
		/// <returns>Returns all components of <c>T</c> type in the scene, or <c>null</c>.</returns>

		public static T[] FindComponents<T>(this Scene scene)
		{
			return scene.FindComponents<T>(false);
		}

		/// <summary>
		/// Searches GameObjects in the scene for a component of the specified type.
		/// </summary>
		/// <param name="includeInactive">Should components on inactive GameObjects be included in the search?</param>
		/// <returns>Returns all components of <c>T</c> type in the scene, or <c>null</c>.</returns>

		public static T[] FindComponents<T>(this Scene scene, bool includeInactive)
		{
			GameObject[] rootGameObjects = scene.GetRootGameObjects();

			T[] components = null;

			foreach (GameObject gameObject in rootGameObjects)
			{
				if (gameObject.TryGetComponentsInChildren<T>(includeInactive, out T[] foundComponents))
				{
					if (components == null)
					{
						components = foundComponents;
					}
					else
					{
						ArrayExtensions.Append(ref components, foundComponents);
					}
				}
			}

			return components;
		}

		/// <summary>
		/// Searches GameObjects in the scene for a component of the specified type.
		/// </summary>
		/// <param name="components">The output argument that will contain the components or <c>null</c>.</param>
		/// <returns>Returns <c>true</c> if a component is found, <c>false</c> otherwise.</returns>

		public static bool TryFindComponents<T>(this Scene scene, out T[] components)
		{
			return scene.TryFindComponents<T>(false, out components);
		}

		/// <summary>
		/// Searches GameObjects in the scene for a component of the specified type.
		/// </summary>
		/// <param name="includeInactive">Should components on inactive GameObjects be included in the search?</param>
		/// <param name="components">The output argument that will contain the components or <c>null</c>.</param>
		/// <returns>Returns <c>true</c> if a component is found, <c>false</c> otherwise.</returns>

		public static bool TryFindComponents<T>(this Scene scene, bool includeInactive, out T[] components)
		{
			components = null;

			GameObject[] rootGameObjects = scene.GetRootGameObjects();

			foreach (GameObject gameObject in rootGameObjects)
			{
				if (gameObject.TryGetComponentsInChildren<T>(includeInactive, out T[] foundComponents))
				{
					if (components == null)
					{
						components = foundComponents;
					}
					else
					{
						ArrayExtensions.Append(ref components, foundComponents);
					}
				}
			}

			return (components != null && components.Length > 0);
		}
	}
}