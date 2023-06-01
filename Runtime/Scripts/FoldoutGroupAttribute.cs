namespace Wondeluxe
{
	/// <summary>
	/// Used to display fields together in the Inspector under a foldout. The foldout will be displayed from the position
	/// of the first field marked with the attribute.
	/// </summary>

	public class FoldoutGroupAttribute : GroupAttribute
	{
		/// <summary>
		/// Used to display fields together in the Inspector under a foldout.
		/// </summary>
		/// <param name="name">The name of the group, used as the foldout label.</param>

		public FoldoutGroupAttribute(string name) : base(name) { }
	}
}