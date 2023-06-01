using System;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Invokes a method when a field value is modified in the Inspector.
	/// The method can be any method (instance or static, public or non-public) declared on the field's parent object.
	/// </summary>

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class OnModifiedAttribute : PropertyAttribute
	{
		/// <summary>
		/// Name of the callback method to call when the value is modified. The method can be any method (instance or static, public or non-public) declared on the property's parent object.
		/// </summary>

		public string CallbackName { get; set; }

		/// <summary>
		/// Set to <c>true</c> if the callback method should be invoked while the Editor is not in Play Mode. Defaults to <c>true</c>.
		/// </summary>

		public bool EditMode { get; set; } = true;

		/// <summary>
		/// Set to <c>true</c> if the callback method should be invoked while the Editor is in Play Mode. Defaults to <c>true</c>.
		/// </summary>

		public bool PlayMode { get; set; } = true;

		/// <summary>
		/// Invokes a method when a field value is modified in the Inspector.
		/// </summary>
		/// <param name="callbackName">Name of the method to call when the property value is modified.</param>

		public OnModifiedAttribute(string callbackName)
		{
			CallbackName = callbackName;
		}

		/// <summary>
		/// Invokes a method when a field value is modified in the Inspector.
		/// </summary>
		/// <param name="callbackName">Name of the method to call when the value is modified.</param>
		/// <param name="editMode">Should the callback method be invoked while the Editor is not in Play Mode?</param>
		/// <param name="playMode">Should the callback method be invoked while the Editor is in Play Mode?</param>

		public OnModifiedAttribute(string callbackName, bool editMode, bool playMode)
		{
			CallbackName = callbackName;
			EditMode = editMode;
			PlayMode = playMode;
		}

		/// <summary>
		/// Retuns <c>true</c> if the callback method can be invoked for the current Editor state.
		/// </summary>

		public bool CanInvokeCallback
		{
			get
			{
				if (Application.isPlaying)
				{
					return PlayMode;
				}

				return EditMode;
			}
		}

		public override string ToString()
		{
			return $"(CallbackName = {CallbackName}, EditMode = {EditMode}, PlayMode = {PlayMode})";
		}
	}
}