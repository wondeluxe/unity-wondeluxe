using System.Collections.Generic;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Behaviour for managing animation playback of an Animator. This behaviour provides additional functionality for setting (without replacing)
	/// and queueing animations that doesn't exist by default on Animator.
	/// </summary>
	/// <remarks>
	/// <c>AnimationStateBehaviour</c> must be added to each animation state on the attached AnimatorController in order for queueing to work.
	/// Sets <c>Animator.keepAnimatorControllerStateOnDisable = true</c> on Awake.
	/// </remarks>

	public class AnimatorController : MonoBehaviour, IAnimatorStateCompleteHandler
	{
		#region Internal fields

		[SerializeField]
		private Animator animator;

		private readonly Dictionary<int, Queue<AnimationPlaybackInfo>> queues = new Dictionary<int, Queue<AnimationPlaybackInfo>>();

		#endregion

		#region Public API

		/// <summary>
		/// The controlled animator.
		/// </summary>

		public Animator Animator
		{
			get => animator;
		}

		/// <summary>
		/// Plays an animation state, replacing the existing state.
		/// </summary>
		/// <param name="stateName">The state name.</param>
		/// <param name="layer">The layer index. If layer is -1, it plays the first state with the given state name.</param>
		/// <param name="normalizedTime">The time offset between zero and one.</param>
		/// <remarks>
		/// This method will restart the animation if it is already the current state.
		/// </remarks>

		public void Play(string stateName, int layer = 0, float normalizedTime = float.NegativeInfinity)
		{
			Play(Animator.StringToHash(stateName), layer, normalizedTime);
		}

		/// <summary>
		/// Plays an animation state, replacing the existing state.
		/// </summary>
		/// <param name="stateNameHash">The state name hash. If stateNameHash is 0, it changes the current state time.</param>
		/// <param name="layer">The layer index. If layer is -1, it plays the first state with the given hash.</param>
		/// <param name="normalizedTime">The time offset between zero and one.</param>
		/// <remarks>
		/// This method will restart the animation if it is already the current state.
		/// </remarks>

		public void Play(int stateNameHash, int layer = 0, float normalizedTime = float.NegativeInfinity)
		{
			Clear(layer);
			animator.Play(stateNameHash, layer, normalizedTime);
		}

		/// <summary>
		/// Plays an animation state if it isn't the current state.
		/// </summary>
		/// <param name="stateName">The state name.</param>
		/// <param name="layer">The layer index. If layer is -1, it plays the first state with the given state name.</param>
		/// <param name="normalizedTime">The time offset between zero and one.</param>

		public void Set(string stateName, int layer = 0, float normalizedTime = float.NegativeInfinity)
		{
			Set(Animator.StringToHash(stateName), layer, normalizedTime);
		}

		/// <summary>
		/// Plays an animation state if it isn't the current state.
		/// </summary>
		/// <param name="stateNameHash">The state name hash. If stateNameHash is 0, it changes the current state time.</param>
		/// <param name="layer">The layer index. If layer is -1, it plays the first state with the given hash.</param>
		/// <param name="normalizedTime">The time offset between zero and one.</param>

		public void Set(int stateNameHash, int layer = 0, float normalizedTime = float.NegativeInfinity)
		{
			AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layer);

			if (stateInfo.fullPathHash != stateNameHash)
			{
				Clear(layer);
				animator.Play(stateNameHash, layer, normalizedTime);
			}
		}

		/// <summary>
		/// Queues an animation state to play after the existing state completes.
		/// </summary>
		/// <param name="stateName">The state name.</param>
		/// <param name="layer">The layer index. If layer is -1, it plays the first state with the given state name.</param>
		/// <param name="normalizedTime">The time offset between zero and one.</param>

		public void Queue(string stateName, int layer = 0, float normalizedTime = float.NegativeInfinity)
		{
			Queue(Animator.StringToHash(stateName), layer, normalizedTime);
		}

		/// <summary>
		/// Queues an animation state to play after the existing state completes.
		/// </summary>
		/// <param name="stateNameHash">The state name hash. If stateNameHash is 0, it changes the current state time.</param>
		/// <param name="layer">The layer index. If layer is -1, it plays the first state with the given hash.</param>
		/// <param name="normalizedTime">The time offset between zero and one.</param>

		public void Queue(int stateNameHash, int layer = 0, float normalizedTime = float.NegativeInfinity)
		{
			if (!queues.ContainsKey(layer))
			{
				queues.Add(layer, new Queue<AnimationPlaybackInfo>());
			}

			queues[layer].Enqueue(new AnimationPlaybackInfo(stateNameHash, layer, normalizedTime));
		}

		/// <summary>
		/// Clears the animation queue for a layer.
		/// </summary>
		/// <param name="layer">The layer for which to clear the queue.</param>

		public void Clear(int layer = 0)
		{
			if (queues.ContainsKey(layer))
			{
				queues[layer].Clear();
			}
		}

		public void OnAnimatorStateComplete(AnimatorStateInfo stateInfo, int layerIndex)
		{
			if (queues.TryGetValue(layerIndex, out Queue<AnimationPlaybackInfo> queue) && queue.Count > 0)
			{
				AnimationPlaybackInfo playbackInfo = queue.Dequeue();

				animator.Play(playbackInfo.NameHash, playbackInfo.Layer, playbackInfo.NormalizedTime);
			}
		}

		#endregion

		#region Internal methods

		#endregion

		#region Unity messages

		private void Awake()
		{
			animator.keepAnimatorStateOnDisable = true;// TODO Could control this through property.
		}

		private void OnEnable()
		{
			animator.enabled = true;

			//if (queuedAnimation != null)
			//{
			//	Animator.Play(queuedAnimation.Name, queuedAnimation.Layer, queuedAnimation.NormalizedTime);
			//	queuedAnimation = null;
			//}
			//else if (queuedTrigger != null)
			//{
			//	Animator.SetTrigger(queuedTrigger);
			//	queuedTrigger = null;
			//}
		}

		private void OnDisable()
		{
			animator.enabled = enabled;
		}

		private void Reset()
		{
			animator = GetComponent<Animator>();
		}

		#endregion
	}
}