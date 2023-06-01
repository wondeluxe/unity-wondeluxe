using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Mark a string field as a folder path. Converts the Inspector field to an object field that only accepts folder assets from the project's Assets folder.
	/// </summary>

	public class FolderAttribute : PropertyAttribute
	{
	}
}