using System.Collections.Generic;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Component for managing pre-initialized instances of a prefab.
	/// </summary>

	public class PrefabPool : MonoBehaviour
	{
		#region Internal fields

		[SerializeField]
		[Tooltip("The prefab to create instances of.")]
		[OnModified("OnPrefabModified")]
		private GameObject prefab;

		[SerializeField]
		[Tooltip("The transform inactive instances of the prefab will be parented to.")]
		[OnModified("OnParentModified")]
		private Transform parent;

		[SerializeField]
		[Tooltip("Total capacity of the PrefabPool. How many instances of the prefab to instantiate/have been instantiated.")]
		[OnModified("OnSizeModified")]
		private int size;

		[SerializeField]
		[Tooltip("If true, additional instances of the prefab will be instantiated at runtime when requesting an instance and the allocation is exhausted.")]
		private bool expand;

		[SerializeField]
		[Tooltip("When are the PrefabPool's instances created? On Update: Instances are automatically created or destroyed when the PrefabPool is modified. On Awake: Instances are created automatically during Awake. Manual: User must manually call Fill at runtime or click Fill in the Inspector.")]
		[OnModified("OnFillModeModified")]
		private PrefabPoolFillMode fillMode;

		/// <summary>
		/// All instances created by this PrefabPool.
		/// </summary>

		[SerializeField]
		[HideInInspector]
		private List<GameObject> instances = new List<GameObject>();

		/// <summary>
		/// Indexes of available instances in the <c>instances</c> List.
		/// </summary>
		/// <remarks>
		/// A Stack would be the preferred collection type for this use case, however we're using a List as Stacks aren't serializable in the Unity editor.
		/// </remarks>

		[SerializeField]
		[HideInInspector]
		private List<int> availableInstanceIndexes = new List<int>();

		#endregion

		#region Public API

		/// <summary>
		/// The prefab to create instances of.
		/// </summary>

		public GameObject Prefab
		{
			get => prefab;
			set
			{
				if (prefab != value)
				{
					prefab = value;
					OnPrefabModified();
				}
			}
		}

		/// <summary>
		/// The transform available/inactive (not requested) instances of the prefab will be parented to. If <c>null</c>, instances will be parented to the PrefabPool's transform.
		/// </summary>

		public Transform Parent
		{
			get => parent;
			set
			{
				if (parent != value)
				{
					parent = value;
					OnParentModified();
				}
			}
		}

		/// <summary>
		/// Total capacity of the PrefabPool. Indicates the number of instances created by the pool.
		/// </summary>

		public int Size
		{
			get => size;
			set
			{
				if (size != value)
				{
					size = value;
					OnSizeModified();
				}
			}
		}

		/// <summary>
		/// Mode defining when prefab instances are created.
		/// </summary>

		public PrefabPoolFillMode FillMode
		{
			get => fillMode;
			set
			{
				if (fillMode != value)
				{
					fillMode = value;
					OnFillModeModified();
				}
			}
		}

		/// <summary>
		/// If <c>true</c>, additional instances of <c>Prefab</c> will be instantiated if the allocation is exhausted when <c>Request</c> is called.
		/// </summary>

		public bool Expand
		{
			get => expand;
			set => expand = value;
		}

		/// <summary>
		/// The number of available (not requested) prefab instances remaining in the PrefabPool.
		/// </summary>

		[ShowInInspector]
		public int AvailableCount
		{
			get => availableInstanceIndexes.Count;
		}

		/// <summary>
		/// Empties the PrefabPool and destroys all available (not requested) instances.
		/// </summary>

		public void Clear()
		{
			for (int i = 0; i < availableInstanceIndexes.Count; i++)
			{
#if UNITY_EDITOR
				if (!Application.isPlaying)
				{
					DestroyImmediate(instances[availableInstanceIndexes[i]]);
					continue;
				}
#endif
				Destroy(instances[availableInstanceIndexes[i]]);
			}

			instances.Clear();
			availableInstanceIndexes.Clear();
		}

		/// <summary>
		/// Creates the number of prefab instances defined by the PrefabPool's <c>Size</c>.
		/// </summary>

		public void Fill()
		{
			ExpandAndInstantiate();
		}

		/// <summary>
		/// Used to create or destroy prefab instances after the PrefabPool's <c>Size</c> has been modified. Instantiates additional instances of <c>Prefab</c> if Size has been increased,
		/// destroys available (not requested) instances if Size has been decreased.
		/// </summary>

		public void Refill()
		{
			if (instances.Count < size)
			{
				ExpandAndInstantiate();
			}
			else if (instances.Count > size)
			{
				ShrinkAndDestroy();
			}
		}

		/// <summary>
		/// Reparents available (not requested) prefab instances to the PrefabPool's <c>Parent</c> transform. This method does nothing if Parent is <c>null</c>.
		/// </summary>

		public void ReparentAvailableInstances()
		{
			if (parent == null)
			{
				return;
			}

			foreach (int index in availableInstanceIndexes)
			{
				instances[index].transform.SetParent(parent);
			}
		}

		/// <summary>
		/// Gets a prefab instance from the PrefabPool if one is available, or if <c>Expand</c> is <c>true</c>.
		/// </summary>
		/// <param name="active">Should the prefab instance be set active?</param>
		/// <returns>An instance of the PrefabPool's <c>Prefab</c>, or <c>null</c>.</returns>

		public GameObject Request(bool active)
		{
			int availableIndex = availableInstanceIndexes.Count - 1;

			if (availableIndex > -1)
			{
				GameObject instance = instances[availableInstanceIndexes[availableIndex]];
				instance.SetActive(active);

				availableInstanceIndexes.RemoveAt(availableIndex);

				return instance;
			}

			if (expand)
			{
				size++;

				GameObject instance = Instantiate(active, size);
				instances.Add(instance);

				return instance;
			}

			return null;
		}

		/// <summary>
		/// Gets a prefab instance from the PrefabPool if one is available, or if <c>Expand</c> is <c>true</c>.
		/// </summary>
		/// <param name="active">Should the prefab instance be set active?</param>
		/// <param name="instance">The output argument that will contain the available prefab instance or <c>null</c>.</param>
		/// <returns><c>true</c> if an available prefab instance was found, otherwise <c>false</c>.</returns>

		public bool Request(bool active, out GameObject instance)
		{
			instance = Request(active);
			return (instance != null);
		}

		/// <summary>
		/// Gets a prefab instance from the PrefabPool if one is available, or if <c>Expand</c> is <c>true</c>, and returns one of its components.
		/// </summary>
		/// <typeparam name="T">The type of Component to get.</typeparam>
		/// <param name="active">Should the prefab instance be set active?</param>
		/// <returns>A Component on a prefab instance, or <c>null</c>.</returns>

		public T Request<T>(bool active) where T : Component
		{
			if (prefab.TryGetComponent<T>(out T component))
			{
				if (Request(active, out GameObject instance))
				{
					return instance.GetComponent<T>();
				}
			}

			return null;
		}

		/// <summary>
		/// Gets a prefab instance from the PrefabPool if one is available, or if <c>Expand</c> is <c>true</c>, and returns one of its components.
		/// </summary>
		/// <typeparam name="T">The type of Component to get.</typeparam>
		/// <param name="active">Should the prefab instance be set active?</param>
		/// <param name="component">The output argument that will contain the available prefab instance's component or <c>null</c>.</param>
		/// <returns><c>true</c> if an available prefab instance was found, otherwise <c>false</c>.</returns>

		public bool Request<T>(bool active, out T component) where T : Component
		{
			component = Request<T>(active);
			return (component != null);
		}

		/// <summary>
		/// Returns a prefab instance to the PrefabPool, making it available for future requests.
		/// </summary>
		/// <param name="instance">The prefab instance to return.</param>
		/// <returns><c>true</c> if the prefab instance was created by the PrefabPool, otherwise <c>false</c>.</returns>

		public bool Return(GameObject instance)
		{
			int instanceIndex = instances.IndexOf(instance);

			if (instanceIndex < 0)
			{
				return false;
			}

			availableInstanceIndexes.Add(instanceIndex);
			instance.SetActive(false);

			if (parent != null)
			{
				instance.transform.SetParent(parent);
			}

			return true;
		}

		/// <summary>
		/// Returns the GameObject of a Component to the PrefabPool, making it available for future requests.
		/// </summary>
		/// <param name="component">The Component whose GameObject to return.</param>
		/// <returns><c>true</c> if the Component's GameObject was created by the PrefabPool, otherwise <c>false</c>.</returns>

		public bool Return(Component component)
		{
			return Return(component.gameObject);
		}

		#endregion

		#region Internal methods

		/// <summary>
		/// Creates new prefab instances to match <c>Size</c>.
		/// </summary>

		private void ExpandAndInstantiate()
		{
			if (prefab == null)
			{
				return;
			}

			while (instances.Count < size)
			{
				int instanceIndex = instances.Count;
				availableInstanceIndexes.Add(instanceIndex);
				instances.Add(Instantiate(false, instanceIndex + 1));
			}
		}

		/// <summary>
		/// Destroys available prefab instances and removes instances from the <c>instances</c> list to match <c>Size</c>.
		/// </summary>

		private void ShrinkAndDestroy()
		{
			while (instances.Count > size && availableInstanceIndexes.Count > 0)
			{
				int availableIndex = availableInstanceIndexes.Count - 1;
				int instanceIndex = availableInstanceIndexes[availableIndex];

				GameObject instance = instances[instanceIndex];

				instances.RemoveAt(instanceIndex);
				availableInstanceIndexes.RemoveAt(availableIndex);

#if UNITY_EDITOR
				if (!Application.isPlaying)
				{
					DestroyImmediate(instance);
					continue;
				}
#endif
				Destroy(instance);
			}

			while (instances.Count > size)
			{
				instances.RemoveAt(instances.Count - 1);
			}
		}

		private void OnPrefabModified()
		{
			Clear();

			if (fillMode == PrefabPoolFillMode.OnUpdate)
			{
				ExpandAndInstantiate();
			}
		}

		private void OnParentModified()
		{
			if (fillMode == PrefabPoolFillMode.OnUpdate)
			{
				ReparentAvailableInstances();
			}
		}

		private void OnSizeModified()
		{
			if (fillMode == PrefabPoolFillMode.OnUpdate)
			{
				Refill();
			}
		}

		private void OnFillModeModified()
		{
			if (fillMode == PrefabPoolFillMode.OnUpdate)
			{
				Refill();
			}
			else
			{
				Clear();
			}
		}

		private GameObject Instantiate(bool active, int number)
		{
			Transform initialParent = (parent != null) ? parent : transform;

#if UNITY_EDITOR
			GameObject instance = Application.isPlaying ? Instantiate(prefab, initialParent) : UnityEditor.PrefabUtility.InstantiatePrefab(prefab, initialParent) as GameObject;
#else
			GameObject instance = Instantiate(prefab, initialParent);
#endif
			instance.name = $"{prefab.name}{number}";
			instance.SetActive(active);

			return instance;
		}

#endregion

#region Unity messages

		private void Awake()
		{
			if (fillMode == PrefabPoolFillMode.OnAwake)
			{
				ExpandAndInstantiate();
			}
		}

		private void Reset()
		{
			parent = transform;
		}

#endregion
	}
}