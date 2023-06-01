using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// An object that waits for the completion of an AsyncOperation.
	/// </summary>

	public class AsyncOperationAwaiter : ICriticalNotifyCompletion
	{
		/// <summary>
		/// The AsyncOperation being awaited.
		/// </summary>

		public AsyncOperation AsyncOperation { get; private set; }

		/// <summary>
		/// Indicates whether the AsyncOperation has completed.
		/// </summary>

		public bool IsCompleted => AsyncOperation.isDone;

		/// <summary>
		/// The action to perform when the wait operation completes.
		/// </summary>

		private Action Continuation { get; set; }

		/// <summary>
		/// An object that waits for the completion of an AsyncOperation.
		/// </summary>
		/// <param name="asyncOperation">The AsyncOperation to await.</param>

		public AsyncOperationAwaiter(AsyncOperation asyncOperation)
		{
			AsyncOperation = asyncOperation;
			AsyncOperation.completed += OnOperationComplete;
		}

		private void OnOperationComplete(AsyncOperation asyncOperation)
		{
			AsyncOperation.completed -= OnOperationComplete;
			Continuation?.Invoke();
		}

		/// <summary>
		/// Required to be implemented for TaskAwaiters. Implementation is empty.
		/// </summary>

		public void GetResult()
		{
		}

		/// <summary>
		/// Not for direct use. Schedules the continuation action for the AsyncOperation that is associated with this AsyncOperationAwaiter.
		/// </summary>
		/// <param name="continuation"></param>

		public void UnsafeOnCompleted(Action continuation)
		{
			OnCompleted(continuation);
		}

		/// <summary>
		/// Not for direct use. Sets the action to perform when the AsyncOperationAwaiter object stops waiting for the AsyncOperation task to complete.
		/// </summary>
		/// <param name="continuation">The action to perform when the wait operation completes.</param>

		public void OnCompleted(Action continuation)
		{
			Continuation = continuation;

			if (AsyncOperation.isDone)
			{
				Continuation();
			}
		}
	}
}