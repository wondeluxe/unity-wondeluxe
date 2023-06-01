using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Wondeluxe;

using Type = System.Type;

namespace WondeluxeEditor
{
	[CustomPropertyDrawer(typeof(InterfaceAttribute))]
	public class InterfaceAttributeDrawer : ObjectPropertyDrawer<Component>
	{
		private InterfaceAttribute Attribute => (attribute as InterfaceAttribute);

		protected override string GetObjectPickerFilter()
		{
			Object[] components = Object.FindObjectsOfType<Component>();
			HashSet<string> implementingTypeNames = new HashSet<string>();

			foreach (Object component in components)
			{
				Type componentType = component.GetType();

				if (Attribute.InterfaceType.IsAssignableFrom(componentType))
				{
					implementingTypeNames.Add(componentType.Name);
				}
			}

			for (int i = 0; i < components.Length; i++)
			{
				if (Attribute.InterfaceType.IsAssignableFrom(components[i].GetType()))
				{
					implementingTypeNames.Add(components[i].GetType().Name);
				}
			}

			if (implementingTypeNames.Count > 0)
			{
				return $"t:{string.Join(" t:", implementingTypeNames)}";
			}

			return null;
		}

		protected override Object GetValidValue(params Object[] objectReferences)
		{
			Type interfaceType = Attribute.InterfaceType;

			foreach (Object objectReference in objectReferences)
			{
				if (objectReference is GameObject)
				{
					GameObject gameObject = objectReference as GameObject;

					if (gameObject.TryGetComponent(interfaceType, out Component component))
					{
						return component;
					}
				}
				else if (objectReference is Component && interfaceType.IsAssignableFrom(objectReference.GetType()))
				{
					return objectReference;
				}
			}

			return null;
		}
	}
}