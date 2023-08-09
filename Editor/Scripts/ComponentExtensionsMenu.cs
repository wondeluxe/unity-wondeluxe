using UnityEngine;
using UnityEditor;

namespace WondeluxeEditor
{
	internal static class ComponentExtensionsMenu
	{
		[InitializeOnLoadMethod]
		private static void OnInitialize()
		{
			EditorApplication.contextualPropertyMenu += OnPropertyContextMenu;
		}

		private static void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
		{
			if (property.propertyType != SerializedPropertyType.ObjectReference)
			{
				return;
			}

			if (property.objectReferenceValue is Component componentValue)
			{
				Component[] components = componentValue.GetComponents(property.GetDeclaredType());

				if (components.Length < 2)
				{
					return;
				}

				foreach (Component component in components)
				{
					menu.AddItem(new GUIContent($"Select Component/{ObjectNames.NicifyVariableName(component.GetType().Name)}"), false, () => OnComponentSelected(property, component));
				}
			}
		}

		private static void OnComponentSelected(SerializedProperty property, Component component)
		{
			property.objectReferenceValue = component;
			property.serializedObject.ApplyModifiedProperties();
		}
	}
}