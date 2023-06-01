namespace Wondeluxe
{
	/// <summary>
	/// Where an <c>Initializer</c> searches for initializables.
	/// </summary>

	public enum InitializerSearchLocation
	{
		/// <summary>
		/// All initializables in the scene will be returned.
		/// </summary>

		Scene,

		/// <summary>
		/// Only initializables in the Initializer's GameObject hierarchy will be returned.
		/// </summary>

		Hierarchy
	}
}