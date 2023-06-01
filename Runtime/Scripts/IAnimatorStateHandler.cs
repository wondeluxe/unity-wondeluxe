using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Provides a mechanism for notifying an object when an Animator enters a new state.
	/// </summary>
	/// <remarks>
	/// This interface works in conjunction with <c>AnimationStateBehaviour</c>. Any Behaviour whose attached Animator's animation states
	/// have the AnimationStateBehaviour attached to them can receive notifications by implementing this interface.
	/// </remarks>

	public interface IAnimatorStateEnterHandler
	{
		/// <summary>
		/// Invoked when an Animator enters a new state.
		/// </summary>
		/// <param name="stateInfo">The AnimatorStateInfo of the new state.</param>
		/// <param name="layerIndex">The layer of the state.</param>

		void OnAnimatorStateEnter(AnimatorStateInfo stateInfo, int layerIndex);
	}

	/// <summary>
	/// Provides a mechanism for notifying an object when an Animator begins a repeat of a state.
	/// </summary>
	/// <remarks>
	/// This interface works in conjunction with <c>AnimationStateBehaviour</c>. Any Behaviour whose attached Animator's animation states
	/// have the AnimationStateBehaviour attached to them can receive notifications by implementing this interface.
	/// </remarks>

	public interface IAnimatorStateRepeatHandler
	{
		/// <summary>
		/// Invoked when an Animator begins a repeat of a state.
		/// </summary>
		/// <param name="stateInfo">The AnimatorStateInfo of the repeated state.</param>
		/// <param name="layerIndex">The layer of the state.</param>

		void OnAnimatorStateRepeat(AnimatorStateInfo stateInfo, int layerIndex);
	}

	/// <summary>
	/// Provides a mechanism for notifying an object when an Animator completes a state.
	/// </summary>
	/// <remarks>
	/// This interface works in conjunction with <c>AnimationStateBehaviour</c>. Any Behaviour whose attached Animator's animation states
	/// have the AnimationStateBehaviour attached to them can receive notifications by implementing this interface.
	/// </remarks>

	public interface IAnimatorStateCompleteHandler
	{
		/// <summary>
		/// Invoked when an Animator completes a state.
		/// </summary>
		/// <param name="stateInfo">The AnimatorStateInfo of the repeated state.</param>
		/// <param name="layerIndex">The layer of the state.</param>

		void OnAnimatorStateComplete(AnimatorStateInfo stateInfo, int layerIndex);
	}

	/// <summary>
	/// Provides a mechanism for notifying an object when an Animator exits a state.
	/// </summary>
	/// <remarks>
	/// This interface works in conjunction with <c>AnimationStateBehaviour</c>. Any Behaviour whose attached Animator's animation states
	/// have the AnimationStateBehaviour attached to them can receive notifications by implementing this interface.
	/// </remarks>

	public interface IAnimatorStateExitHandler
	{
		/// <summary>
		/// Invoked when an Animator exits a state.
		/// </summary>
		/// <param name="stateInfo">The AnimatorStateInfo of the repeated state.</param>
		/// <param name="layerIndex">The layer of the state.</param>

		void OnAnimatorStateExit(AnimatorStateInfo stateInfo, int layerIndex);
	}
}