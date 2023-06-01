using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Used to display fields together in the Inspector. Fields marked with the same Group Name will appear one after the other,
	/// overriding the default order in which they would normally appear. Fields will displayed from the first field marked with the attribute.
	/// </summary>

	public class GroupAttribute : PropertyAttribute
	{
		/// <summary>
		/// Name of the group.
		/// </summary>

		public string Name { get; private set; }

		/// <summary>
		/// Used to display fields together in the Inspector.
		/// </summary>
		/// <param name="name">Name of the group.</param>

		public GroupAttribute(string name)
		{
			Name = name;
		}
	}
}