using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Displays a button in the Inspector that calls the marked method when clicked.
	/// Can be used on any method (instance or static, public or non-public) declared on the Object.
	/// </summary>
	/// <remarks>
	/// Buttons are displayed after all serialized fields and field/properties displayed using the <c>ShowInInspector</c> attribute.
	/// </remarks>

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ButtonAttribute : PropertyAttribute
	{
		/// <summary>
		/// Set to <c>true</c> if the callback method should be invoked while the Editor is not in Play Mode. Defaults to <c>true</c>.
		/// </summary>

		public bool EditMode { get; private set; } = true;

		/// <summary>
		/// Set to <c>true</c> if the callback method should be invoked while the Editor is in Play Mode. Defaults to <c>true</c>.
		/// </summary>

		public bool PlayMode { get; private set; } = true;

		/// <summary>
		/// Line number on which the attribute is used.
		/// </summary>

		public int LineNumber { get; private set; }

		/// <summary>
		/// Returns <c>true</c> if the button should be drawn enabled.
		/// </summary>

		public bool Enabled
		{
			get => (Application.isPlaying ? PlayMode : EditMode);
		}

		/// <summary>
		/// Displays a button in the Inspector that calls the marked method when clicked.
		/// </summary>
		/// <param name="lineNumber">Line number on which the attribute is used. Automatically populated by the compiler using the <c>CallerLineNumber</c> attribute.</param>

		public ButtonAttribute([CallerLineNumber] int lineNumber = 0)
		{
			LineNumber = lineNumber;
		}

		/// <summary>
		/// Displays a button in the Inspector that calls the marked method when clicked.
		/// </summary>
		/// <param name="editMode">Should the butten be enabled when the editor is not playing (play mode not active).</param>
		/// <param name="playMode">Should the button be enabled when the editor is in play mode.</param>
		/// <param name="lineNumber">Line number on which the attribute is used. Automatically populated by the compiler using the <c>CallerLineNumber</c> attribute.</param>

		public ButtonAttribute(bool editMode, bool playMode, [CallerLineNumber] int lineNumber = 0)
		{
			EditMode = editMode;
			PlayMode = playMode;
			LineNumber = lineNumber;
		}
	}
}