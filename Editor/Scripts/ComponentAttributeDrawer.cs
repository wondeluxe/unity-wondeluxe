using System;
using UnityEngine;
using UnityEditor;
using Wondeluxe;

using Object = UnityEngine.Object;

namespace WondeluxeEditor
{
	[CustomPropertyDrawer(typeof(ComponentAttribute))]
	public class ComponentAttributeDrawer : WondeluxePropertyDrawer
	{
		private const string DefaultOption = "<Select Component>";

		public override bool HasCustomLayout => true;

		public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// BeginProperty/EndProperty calls not required.
			// Called in WondeluxePropertyDrawer before/after OnCustomGUI.

			position = EditorGUI.PrefixLabel(position, label);

			float controlWidth = (position.width / 2f) - (EditorGUIExtensions.SubLabelSpacing / 2f);

			Rect objectPosition = new Rect(position.x, position.y, controlWidth, position.height);
			Rect optionsPosition = new Rect(position.x + controlWidth + EditorGUIExtensions.SubLabelSpacing, position.y, controlWidth, position.height);

			Component component = property.objectReferenceValue as Component;
			GameObject gameObject = (component != null) ? component.gameObject : property.objectReferenceValue as GameObject;

			GameObject selectedGameObject = EditorGUI.ObjectField(objectPosition, gameObject, typeof(GameObject), true) as GameObject;

			Component[] components = (selectedGameObject != null) ? selectedGameObject.GetComponents<Component>() : null;

			if (components == null)
			{
				string[] options = new string[] { DefaultOption };

				bool guiEnabled = GUI.enabled;

				GUI.enabled = false;
				EditorGUI.Popup(optionsPosition, 0, options);
				GUI.enabled = guiEnabled;

				property.objectReferenceValue = selectedGameObject;
			}
			else
			{
				string[] options = new string[components.Length + 1];
				int optionIndex = 0;

				options[0] = DefaultOption;

				for (int i = 0; i < components.Length; i++)
				{
					int index = i + 1;

					options[index] = components[i].GetType().Name;

					if (components[i] == component)
					{
						optionIndex = index;
					}
				}

				optionIndex = EditorGUI.Popup(optionsPosition, optionIndex, options);

				property.objectReferenceValue = (optionIndex > 0) ? (components[optionIndex - 1] as Object) : (selectedGameObject as Object);
			}
		}
	}
}