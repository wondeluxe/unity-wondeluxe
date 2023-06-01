namespace Wondeluxe
{
	/// <summary>
	/// Structure used by <c>AnimatorController</c> for managing queued animations.
	/// </summary>

	internal struct AnimationPlaybackInfo
	{
		/// <summary>
		/// The animation state name hash to play.
		/// </summary>

		public int NameHash;

		/// <summary>
		/// The layer on which to play the animation state.
		/// </summary>

		public int Layer;

		/// <summary>
		/// The time offset of the animation state to play.
		/// </summary>

		public float NormalizedTime;

		/// <summary>
		/// Create a new AnimationPlaybackInfo.
		/// </summary>
		/// <param name="nameHash">The animation state name hash to play.</param>
		/// <param name="layer">The layer on which to play the animation state.</param>
		/// <param name="normalizedTime">The time offset of the animation state to play.</param>

		public AnimationPlaybackInfo(int nameHash, int layer, float normalizedTime)
		{
			NameHash = nameHash;
			Layer = layer;
			NormalizedTime = normalizedTime;
		}

		/// <summary>
		/// Returns a string representation of this AnimationPlaybackInfo.
		/// </summary>
		/// <returns>A string representation of this AnimationPlaybackInfo.</returns>

		public override string ToString()
		{
			return $"(NameHash = {NameHash}, Layer = {Layer}, NormalizedTime = {NormalizedTime})";
		}
	}
}