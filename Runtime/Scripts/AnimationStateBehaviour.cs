using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// An animation state machine behaviour allowing game object components to listen to animation events by implementing state handler interfaces.
	/// </summary>

	public class AnimationStateBehaviour : StateMachineBehaviour
	{
		// TODO Investigate whether handlers can be cached in OnEnable, or otherwise cache at runtime.

		private float previousTime;
		private bool complete;

		public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			previousTime = 0f;
			complete = false;

			IAnimatorStateEnterHandler[] handlers = animator.GetComponents<IAnimatorStateEnterHandler>();

			foreach (IAnimatorStateEnterHandler handler in handlers)
			{
				handler.OnAnimatorStateEnter(stateInfo, layerIndex);
			}
		}

		public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			OnStateUpdate(animator, stateInfo, layerIndex, stateInfo.loop);
		}

		private void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, bool loop)
		{
			float totalTime = stateInfo.normalizedTime;
			float currentTime = totalTime - Mathf.Floor(totalTime);

			bool finished = (currentTime < previousTime);

			previousTime = currentTime;

			if (finished)
			{
				// Loop parameter is used instead of stateInfo.loop so that a false value can be given when called from OnStateExit.
				// If stateInfo.loop is used directly, OnAnimatorStateRepeat will always be invoked, even when a transition triggers
				// OnStateExit at a normalized time greater than 1. e.g. Transition is triggered at normalized time of 3, so
				// OnAnimatorStateRepeat is invoked twice, then OnAnimatorStateComplete is invoked on state exit.

				if (loop)
				{
					IAnimatorStateRepeatHandler[] handlers = animator.GetComponents<IAnimatorStateRepeatHandler>();

					foreach (IAnimatorStateRepeatHandler handler in handlers)
					{
						handler.OnAnimatorStateRepeat(stateInfo, layerIndex);
					}
				}
				else if (!complete)
				{
					complete = true;

					IAnimatorStateCompleteHandler[] handlers = animator.GetComponents<IAnimatorStateCompleteHandler>();

					foreach (IAnimatorStateCompleteHandler handler in handlers)
					{
						handler.OnAnimatorStateComplete(stateInfo, layerIndex);
					}
				}
			}
		}

		public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
		{
			// Run an update to ensure OnAnimatorStateComplete fires before OnAnimatorStateExit.
			// OnStateUpdate is not called on the last frame when a state completes.

			OnStateUpdate(animator, stateInfo, layerIndex, false);

			IAnimatorStateExitHandler[] handlers = animator.GetComponents<IAnimatorStateExitHandler>();

			foreach (IAnimatorStateExitHandler handler in handlers)
			{
				handler.OnAnimatorStateExit(stateInfo, layerIndex);
			}
		}
	}
}