using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Displays a non-serialized field or a property with a get method in the Inspector.
	/// </summary>
	/// <remarks>
	/// Fields displayed using this attribute will appear after all serialized fields are displayed.
	/// </remarks>

	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
	public class ShowInInspectorAttribute : PropertyAttribute
	{
		/// <summary>
		/// Line number on which the attribute is used.
		/// </summary>

		public int LineNumber { get; private set; }

		/// <summary>
		/// Displays a non-serialized field or a property with a get method in the Inspector.
		/// </summary>
		/// <param name="lineNumber">Line number on which the attribute is used. Automatically populated by the compiler using the <c>CallerLineNumber</c> attribute.</param>

		public ShowInInspectorAttribute([CallerLineNumber] int lineNumber = 0)
		{
			LineNumber = lineNumber;
		}
	}
}