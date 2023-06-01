using UnityEngine;
using UnityEditor;
using Wondeluxe;

namespace WondeluxeEditor
{
	[CustomPropertyDrawer(typeof(ReadonlyAttribute))]
	public class ReadonlyAttributeDrawer : WondeluxePropertyDrawer
	{
		private bool guiEnabled;

		public override void OnPreGUI(SerializedProperty property, GUIContent label)
		{
			guiEnabled = GUI.enabled;
			GUI.enabled = false;
		}

		public override void OnPostGUI(SerializedProperty property, GUIContent label, bool valueChanged)
		{
			GUI.enabled = guiEnabled;
		}
	}
}