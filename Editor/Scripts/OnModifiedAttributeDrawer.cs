using System;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using Wondeluxe;

namespace WondeluxeEditor
{
	[CustomPropertyDrawer(typeof(OnModifiedAttribute))]
	public class OnModifiedAttributeDrawer : WondeluxePropertyDrawer
	{
		private OnModifiedAttribute Attribute => (attribute as OnModifiedAttribute);

		public override void OnPostGUI(SerializedProperty property, GUIContent label, bool valueChanged)
		{
			if (valueChanged && Attribute.CanInvokeCallback)
			{
				// Use GetParentObjects to handle
				object[] targets = property.GetParentObjects();
				Type targetType = targets[0].GetType();
				MethodInfo callbackMethod = targetType.GetMethod(Attribute.CallbackName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly, null, Type.EmptyTypes, null);

				if (callbackMethod == null)
				{
					Debug.LogError($"OnModified callback '{Attribute.CallbackName}' is null.", property.serializedObject.targetObject);
				}
				else if (callbackMethod.GetParameters().Length > 0)
				{
					Debug.LogError($"OnModified callback '{Attribute.CallbackName}' expects parameters. Callback must take no parameters.", property.serializedObject.targetObject);
				}
				else
				{
					// Modifications must be applied so that the new value is updated in the serialized object.
					property.serializedObject.ApplyModifiedProperties();

					foreach (object target in targets)
					{
						callbackMethod.Invoke(target, new object[] { });
					}
				}
			}
		}
	}
}