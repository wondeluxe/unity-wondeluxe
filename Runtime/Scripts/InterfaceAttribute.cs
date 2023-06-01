using System;
using UnityEngine;

namespace Wondeluxe
{
	// TODO Look at ways to allow multiple InterfaceAttributes.

	/// <summary>
	/// Requires that a script component reference implements an interface.
	/// </summary>

	public class InterfaceAttribute : PropertyAttribute
	{
		/// <summary>
		/// The Type of the interface that must be implemented.
		/// </summary>

		public Type InterfaceType { get; private set; }

		/// <summary>
		/// Requires that a script component reference implements an interface.
		/// </summary>
		/// <param name="interfaceType">The Type of the interface that must be implemented.</param>

		public InterfaceAttribute(Type interfaceType)
		{
			InterfaceType = interfaceType;
		}
	}
}