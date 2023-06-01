namespace Wondeluxe
{
	/// <summary>
	/// Timing for when a PrefabPool creates its instances.
	/// </summary>

	public enum PrefabPoolFillMode
	{
		/// <summary>
		/// Instances are automatically created or destroyed when the PrefabPool is modified.
		/// </summary>

		OnUpdate,

		/// <summary>
		/// Instances are created automatically during Awake.
		/// </summary>

		OnAwake,

		/// <summary>
		/// User must manually call <c>Fill</c> or <c>Refill</c> at runtime or click <i>Refill</i> in the Inspector.
		/// </summary>

		Manual
	}
}