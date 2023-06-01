using UnityEngine;
using Wondeluxe;

namespace Wondeluxe
{
	/// <summary>
	/// Behaviour for initializing objects that are inactive/disabled on scene load.
	/// The Initializer component will call Initialize on its initializables when it receives the Awake message.
	/// </summary>

	public class Initializer : MonoBehaviour
	{
		#region Internal fields

		[SerializeField]
		[Tooltip("Where the Initializer searches for initializables. If set to Scene, all intializables in the scene are returned. If set to Hierarchy, only initializables in the the Initializer's hierarchy are returned.")]
		[OnModified("OnSearchModified", true, false)]
		private InitializerSearchLocation searchLocation;

		[SerializeField]
		[Tooltip("Method the Initializer uses to find initializables. On Reload: automatically search for initializables when scripts are reloaded, or when Search Location or Search Mode are modified. On Awake: automatically search for initializables when the Initializer receives the Awake message. Manual: user must manually assign initializables or click Find Initializables.")]
		[OnModified("OnSearchModified", true, false)]
		private InitializerSearchMode searchMode;

		[SerializeField]
		[Tooltip("MonoBehaviours implementing IInitializable to initialize.")]
		[Interface(typeof(IInitializable))]
		private MonoBehaviour[] initializables;

		#endregion

		#region Public API

		/// <summary>
		/// Where the Initializer searches for initializables in <c>FindInitializables</c>.
		/// </summary>

		public InitializerSearchLocation SearchLocation
		{
			get => searchLocation;
			set => searchLocation = value;
		}

		/// <summary>
		/// Method the Initializer uses to find initializables in <c>FindInitializables</c>.
		/// </summary>

		public InitializerSearchMode SearchMode
		{
			get => searchMode;
			set => searchMode = value;
		}

		/// <summary>
		/// Search for initializables.
		/// </summary>

		[Button(true, false)]
		public void FindInitializables()
		{
			switch (searchLocation)
			{
				case InitializerSearchLocation.Scene:
					FindInitializablesInScene();
					break;
				case InitializerSearchLocation.Hierarchy:
					FindInitializablesInHierarchy();
					break;
			}
		}

		#endregion

		#region Internal methods

		private void FindInitializablesInScene()
		{
			if (gameObject.scene.TryFindComponents<IInitializable>(true, out IInitializable[] components))
			{
				initializables = new MonoBehaviour[components.Length];

				for (int i = 0; i < components.Length; i++)
				{
					initializables[i] = components[i] as MonoBehaviour;
				}
			}
			else
			{
				initializables = new MonoBehaviour[0];
			}
		}

		private void FindInitializablesInHierarchy()
		{
			IInitializable[] components = GetComponentsInChildren<IInitializable>(true);

			initializables = new MonoBehaviour[components.Length];

			for (int i = 0; i < components.Length; i++)
			{
				initializables[i] = components[i] as MonoBehaviour;
			}
		}

#if UNITY_EDITOR
		private void OnSearchModified()
		{
			if (searchMode == InitializerSearchMode.OnReload)
			{
				FindInitializables();
			}
		}
#endif

		#endregion

		#region Unity messages

		private void Awake()
		{
			if (searchMode == InitializerSearchMode.OnAwake)
			{
				FindInitializables();
			}

			foreach (MonoBehaviour initializable in initializables)
			{
				(initializable as IInitializable).Initialize();
			}
		}

		private void Reset()
		{
			FindInitializables();
		}

		#endregion
	}
}