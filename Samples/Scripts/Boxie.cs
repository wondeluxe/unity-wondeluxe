using System;
using UnityEngine;

namespace Wondeluxe.Samples
{
	public class Boxie : MonoBehaviour, IAnimatorStateRepeatHandler, IAnimatorStateCompleteHandler
	{
		[SerializeField]
		[AnimatorState]
		[FoldoutGroup("Animations")]
		[OnModified("OnIdleModified")]
		private int idle;

		[SerializeField]
		[AnimatorState]
		[FoldoutGroup("Animations")]
		[OnModified("OnJumpModified")]
		private int jump;

		[SerializeField]
		[AnimatorParameter]
		[FoldoutGroup("Parameters")]
		private string grounded;

		[SerializeField]
		[AnimatorParameter]
		[FoldoutGroup("Parameters")]
		private string speed;

		[SerializeField]
		private Animator animator;

		private void OnIdleModified()
		{
			Debug.Log($"idle = {jump}");
		}

		private void OnJumpModified()
		{
			Debug.Log($"jump = {jump}");
		}

		public void OnAnimatorStateRepeat(AnimatorStateInfo stateInfo, int layerIndex)
		{
			Debug.Log($"Animator state repeat: {GetAnimationName(stateInfo.shortNameHash)}");
		}

		public void OnAnimatorStateComplete(AnimatorStateInfo stateInfo, int layerIndex)
		{
			Debug.Log($"Animator state complete: {GetAnimationName(stateInfo.shortNameHash)}");
		}

		[Button(false, true)]
		private void Jump()
		{
			animator.Play(jump);
		}

		private string GetAnimationName(int hash)
		{
			if (hash == idle)
				return "Idle";

			if (hash == jump)
				return "Jump";

			throw new Exception($"Animation hash ({hash}) not implemented.");
		}

		private void Reset()
		{
			animator = GetComponent<Animator>();
		}
	}
}