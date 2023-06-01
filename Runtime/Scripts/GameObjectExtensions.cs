using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for GameObject.
	/// </summary>

	public static class GameObjectExtensions
	{
		/// <summary>
		/// Gets all components of the specified type on this GameObject, if any exist.
		/// </summary>
		/// <param name="gameObject">The GameObject to get the components of.</param>
		/// <param name="components">The output argument that will contain the components or <c>null</c>.</param>
		/// <returns>Returns <c>true</c> if a component is found, <c>false</c> otherwise.</returns>

		public static bool TryGetComponents<T>(this GameObject gameObject, out T[] components)
		{
			components = gameObject.GetComponents<T>();

			return (components != null && components.Length > 0);
		}

		/// <summary>
		/// Gets the component of the specified type on this GameObject or any of its children, if it exists.
		/// </summary>
		/// <param name="gameObject">The GameObject to get the components of.</param>
		/// <param name="component">The output argument that will contain the component or <c>null</c>.</param>
		/// <returns>Returns <c>true</c> if the component is found, <c>false</c> otherwise.</returns>

		public static bool TryGetComponentInChildren<T>(this GameObject gameObject, out T component)
		{
			return gameObject.TryGetComponentInChildren<T>(false, out component);
		}

		/// <summary>
		/// Gets the component of the specified type on this GameObject or any of its children, if it exists.
		/// </summary>
		/// <param name="gameObject">The GameObject to get the components of.</param>
		/// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set?</param>
		/// <param name="component">The output argument that will contain the component or <c>null</c>.</param>
		/// <returns>Returns <c>true</c> if the component is found, <c>false</c> otherwise.</returns>

		public static bool TryGetComponentInChildren<T>(this GameObject gameObject, bool includeInactive, out T component)
		{
			component = gameObject.GetComponentInChildren<T>(includeInactive);

			return (component != null);
		}

		/// <summary>
		/// Returns all components of the specified type on this GameObject or any of its children, if any exist.
		/// </summary>
		/// <param name="gameObject">The GameObject to get the components of.</param>
		/// <param name="components">The output argument that will contain the components or <c>null</c>.</param>
		/// <returns>Returns <c>true</c> if a component is found, <c>false</c> otherwise.</returns>

		public static bool TryGetComponentsInChildren<T>(this GameObject gameObject, out T[] components)
		{
			return gameObject.TryGetComponentsInChildren<T>(false, out components);
		}

		/// <summary>
		/// Returns all components of the specified type on this GameObject or any of its children, if any exist.
		/// </summary>
		/// <param name="gameObject">The GameObject to get the components of.</param>
		/// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set?</param>
		/// <param name="components">The output argument that will contain the components or <c>null</c>.</param>
		/// <returns>Returns <c>true</c> if a component is found, <c>false</c> otherwise.</returns>

		public static bool TryGetComponentsInChildren<T>(this GameObject gameObject, bool includeInactive, out T[] components)
		{
			components = gameObject.GetComponentsInChildren<T>(includeInactive);

			return (components != null && components.Length > 0);
		}

		/// <summary>
		/// Gets the component of the specified type on this GameObject or any of its parents, if it exists.
		/// </summary>
		/// <param name="gameObject">The GameObject to get the components of.</param>
		/// <param name="component">The output argument that will contain the component or <c>null</c>.</param>
		/// <returns>Returns <c>true</c> if the component is found, <c>false</c> otherwise.</returns>

		public static bool TryGetComponentInParent<T>(this GameObject gameObject, out T component)
		{
			return gameObject.TryGetComponentInParent<T>(false, out component);
		}

		/// <summary>
		/// Gets the component of the specified type on this GameObject or any of its parents, if it exists.
		/// </summary>
		/// <param name="gameObject">The GameObject to get the components of.</param>
		/// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set?</param>
		/// <param name="component">The output argument that will contain the component or <c>null</c>.</param>
		/// <returns>Returns <c>true</c> if the component is found, <c>false</c> otherwise.</returns>

		public static bool TryGetComponentInParent<T>(this GameObject gameObject, bool includeInactive, out T component)
		{
			component = gameObject.GetComponentInParent<T>(includeInactive);

			return (component != null);
		}

		/// <summary>
		/// Returns all components of the specified type on this GameObject or any of its parents, if any exist.
		/// </summary>
		/// <param name="gameObject">The GameObject to get the components of.</param>
		/// <param name="components">The output argument that will contain the components or <c>null</c>.</param>
		/// <returns>Returns <c>true</c> if a component is found, <c>false</c> otherwise.</returns>

		public static bool TryGetComponentsInParent<T>(this GameObject gameObject, out T[] components)
		{
			return gameObject.TryGetComponentsInParent<T>(false, out components);
		}

		/// <summary>
		/// Returns all components of the specified type on this GameObject or any of its parents, if any exist.
		/// </summary>
		/// <param name="gameObject">The GameObject to get the components of.</param>
		/// <param name="includeInactive">Should Components on inactive GameObjects be included in the found set?</param>
		/// <param name="components">The output argument that will contain the components or <c>null</c>.</param>
		/// <returns>Returns <c>true</c> if a component is found, <c>false</c> otherwise.</returns>

		public static bool TryGetComponentsInParent<T>(this GameObject gameObject, bool includeInactive, out T[] components)
		{
			components = gameObject.GetComponentsInParent<T>(includeInactive);

			return (components != null && components.Length > 0);
		}

		/// <summary>
		/// Set the layer this game object and all of its children.
		/// </summary>
		/// <param name="gameObject">The GameObject whose layer to set.</param>
		/// <param name="layer">The layer to use.</param>

		public static void SetLayer(this GameObject gameObject, int layer)
		{
			gameObject.layer = layer;

			for (int i = 0; i < gameObject.transform.childCount; i++)
			{
				gameObject.transform.GetChild(i).gameObject.SetLayer(layer);
			}
		}
	}
}