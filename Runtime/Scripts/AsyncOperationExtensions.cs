using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Extension and utility methods for AsyncOperation.
	/// </summary>

	public static class AsyncOperationExtensions
	{
		/// <summary>
		/// Gets an awaiter used to await this AsyncOperation.
		/// </summary>
		/// <param name="asyncOperation">The AsyncOperation to be awaited.</param>
		/// <returns>An awaiter instance.</returns>

		public static AsyncOperationAwaiter GetAwaiter(this AsyncOperation asyncOperation)
		{
			return new AsyncOperationAwaiter(asyncOperation);
		}
	}
}