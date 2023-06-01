namespace Wondeluxe
{
	/// <summary>
	/// Mode used by an <c>Initializer</c> to search for initializables.
	/// </summary>

	public enum InitializerSearchMode
	{
		/// <summary>
		/// The Initializer will automatically search for initializables when scripts are reloaded, or when <c>SearchMode</c> is changed to this value in the Inspector.
		/// </summary>

		OnReload,

		/// <summary>
		/// The Initializer will search for initializables on Awake.
		/// </summary>

		OnAwake,

		/// <summary>
		/// User must manually search for initializables buy calling <c>FindInitializables</c>, or by assigning them or clicking Find Initializables in the Inspector.
		/// </summary>

		Manual
	}
}