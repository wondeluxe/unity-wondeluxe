using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using Wondeluxe;

using AnimatorController = UnityEditor.Animations.AnimatorController;

namespace WondeluxeEditor
{
	[CustomPropertyDrawer(typeof(AnimatorStateAttribute))]
	public class AnimatorStateAttributeDrawer : WondeluxePropertyDrawer
	{
		private AnimatorStateAttribute Attribute => (attribute as AnimatorStateAttribute);

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
					throw new Exception($"AnimatorState attribute used on unsupported property type ({property.propertyType}). AnimatorState attribute may only be used on int or string properties.");
			}
		}

		private void OnIntGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			int stateNameHash = property.intValue;
			string[] stateNames = GetStateNames(property);
			string stateName = GetStateName(stateNameHash, stateNames);

			int valueIndex = Mathf.Clamp(Array.IndexOf(stateNames, stateName), 0, stateNames.Length - 1);

			valueIndex = EditorGUIExtensions.DrawOptionsField(label.text, valueIndex, stateNames, ref position);

			stateName = stateNames[valueIndex];
			stateNameHash = Animator.StringToHash(stateName);

			property.intValue = stateNameHash;
		}

		private void OnStringGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			string[] stateNames = GetStateNames(property);
			string stateName = property.stringValue;

			int valueIndex = Mathf.Clamp(Array.IndexOf(stateNames, stateName), 0, stateNames.Length - 1);

			valueIndex = EditorGUIExtensions.DrawOptionsField(label.text, valueIndex, stateNames, ref position);

			stateName = stateNames[valueIndex];

			property.stringValue = stateName;
		}

		private string[] GetStateNames(SerializedProperty property)
		{
			Component component = property.serializedObject.targetObject as Component;
			Animator animator = component.GetComponent<Animator>();
			AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;

			int firstLayer = Attribute.Layer;
			int lastLayer = firstLayer;

			if (firstLayer < 0)
			{
				firstLayer = 0;
				lastLayer = animatorController.layers.Length - 1;
			}

			HashSet<string> stateNamesSet = new HashSet<string>();

			for (int i = firstLayer; i <= lastLayer; i++)
			{
				AnimatorControllerLayer layer = animatorController.layers[i];

				foreach (ChildAnimatorState state in layer.stateMachine.states)
				{
					stateNamesSet.Add(state.state.name);
				}
			}

			string[] stateNamesArray = new string[stateNamesSet.Count];

			stateNamesSet.CopyTo(stateNamesArray);

			return stateNamesArray;
		}

		private static string GetStateName(int stateNameHash, string[] stateNames)
		{
			foreach (string stateName in stateNames)
			{
				if (Animator.StringToHash(stateName) == stateNameHash)
				{
					return stateName;
				}
			}

			return null;
		}
	}
}