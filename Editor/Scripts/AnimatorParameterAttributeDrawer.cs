using System;
using UnityEngine;
using UnityEditor;
using Wondeluxe;

using AnimatorController = UnityEditor.Animations.AnimatorController;

namespace WondeluxeEditor
{
	[CustomPropertyDrawer(typeof(AnimatorParameterAttribute))]
	public class AnimatorParameterAttributeDrawer : WondeluxePropertyDrawer
	{
		public override bool HasCustomLayout => true;

		public override void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// BeginProperty/EndProperty calls not required.
			// Called in WondeluxePropertyDrawer before/after OnCustomGUI.

			switch (property.propertyType)
			{
				case SerializedPropertyType.Integer:
					OnIntGUI(position, property, label);
					break;
				case SerializedPropertyType.String:
					OnStringGUI(position, property, label);
					break;
				default:
					throw new Exception($"AnimatorParameter attribute used on unsupported property type ({property.propertyType}). AnimatorParameter attribute may only be used on int or string properties.");
			}
		}

		private void OnIntGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			int parameterNameHash = property.intValue;

			AnimatorControllerParameter[] parameters = GetParameters(property);
			string[] parameterNames = GetParameterNames(parameters);

			int valueIndex = GetParameterIndex(parameters, parameterNameHash);
			valueIndex = EditorGUIExtensions.DrawOptionsField(label.text, valueIndex, parameterNames, ref position);

			property.intValue = parameters[valueIndex].nameHash;
		}

		private void OnStringGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			string parameterName = property.stringValue;

			AnimatorControllerParameter[] parameters = GetParameters(property);
			string[] parameterNames = GetParameterNames(parameters);

			int valueIndex = GetParameterIndex(parameters, parameterName);
			valueIndex = EditorGUIExtensions.DrawOptionsField(label.text, valueIndex, parameterNames, ref position);

			property.stringValue = parameters[valueIndex].name;
		}

		private static AnimatorControllerParameter[] GetParameters(SerializedProperty property)
		{
			Component component = property.serializedObject.targetObject as Component;
			Animator animator = component.GetComponent<Animator>();
			AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;

			return controller.parameters;
		}

		private static string[] GetParameterNames(AnimatorControllerParameter[] parameters)
		{
			string[] parameterNames = new string[parameters.Length];

			for (int i = 0; i < parameters.Length; i++)
			{
				parameterNames[i] = parameters[i].name;
			}

			return parameterNames;
		}

		private static int GetParameterIndex(AnimatorControllerParameter[] parameters, int parameterNameHash)
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].nameHash == parameterNameHash)
				{
					return i;
				}
			}

			return 0;
		}

		private static int GetParameterIndex(AnimatorControllerParameter[] parameters, string parameterName)
		{
			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].name == parameterName)
				{
					return i;
				}
			}

			return 0;
		}
	}
}