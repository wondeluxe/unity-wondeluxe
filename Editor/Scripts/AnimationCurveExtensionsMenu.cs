using UnityEngine;
using UnityEditor;
using Wondeluxe;

namespace WondeluxeEditor
{
	internal static class AnimationCurveExtensionsMenu
	{
		[InitializeOnLoadMethod]
		private static void OnInitialize()
		{
			EditorApplication.contextualPropertyMenu += OnPropertyContextMenu;
		}

		private static void OnPropertyContextMenu(GenericMenu menu, SerializedProperty property)
		{
			if (property.propertyType != SerializedPropertyType.AnimationCurve)
			{
				return;
			}

			menu.AddItem(new GUIContent($"Set From Equation/Quadratic/Ease In"), false, () => OnEquationSelected(property, AnimationCurveExtensions.QuadraticEaseIn));
			menu.AddItem(new GUIContent($"Set From Equation/Quadratic/Ease Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.QuadraticEaseOut));
			menu.AddItem(new GUIContent($"Set From Equation/Quadratic/Ease In & Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.QuadraticEaseInOut));
			menu.AddItem(new GUIContent($"Set From Equation/Cubic/Ease In"), false, () => OnEquationSelected(property, AnimationCurveExtensions.CubicEaseIn));
			menu.AddItem(new GUIContent($"Set From Equation/Cubic/Ease Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.CubicEaseOut));
			menu.AddItem(new GUIContent($"Set From Equation/Cubic/Ease In & Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.CubicEaseInOut));
			menu.AddItem(new GUIContent($"Set From Equation/Quartic/Ease In"), false, () => OnEquationSelected(property, AnimationCurveExtensions.QuarticEaseIn));
			menu.AddItem(new GUIContent($"Set From Equation/Quartic/Ease Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.QuarticEaseOut));
			menu.AddItem(new GUIContent($"Set From Equation/Quartic/Ease In & Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.QuarticEaseInOut));
			menu.AddItem(new GUIContent($"Set From Equation/Quintic/Ease In"), false, () => OnEquationSelected(property, AnimationCurveExtensions.QuinticEaseIn));
			menu.AddItem(new GUIContent($"Set From Equation/Quintic/Ease Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.QuinticEaseOut));
			menu.AddItem(new GUIContent($"Set From Equation/Quintic/Ease In & Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.QuinticEaseInOut));
			menu.AddItem(new GUIContent($"Set From Equation/Exponential/Ease In"), false, () => OnEquationSelected(property, AnimationCurveExtensions.ExponentialEaseIn));
			menu.AddItem(new GUIContent($"Set From Equation/Exponential/Ease Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.ExponentialEaseOut));
			menu.AddItem(new GUIContent($"Set From Equation/Exponential/Ease In & Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.ExponentialEaseInOut));
			menu.AddItem(new GUIContent($"Set From Equation/Sine/Ease In"), false, () => OnEquationSelected(property, AnimationCurveExtensions.SineEaseIn));
			menu.AddItem(new GUIContent($"Set From Equation/Sine/Ease Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.SineEaseOut));
			menu.AddItem(new GUIContent($"Set From Equation/Sine/Ease In & Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.SineEaseInOut));
			menu.AddItem(new GUIContent($"Set From Equation/Circular/Ease In"), false, () => OnEquationSelected(property, AnimationCurveExtensions.CircularEaseIn));
			menu.AddItem(new GUIContent($"Set From Equation/Circular/Ease Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.CircularEaseOut));
			menu.AddItem(new GUIContent($"Set From Equation/Circular/Ease In & Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.CircularEaseInOut));
			menu.AddItem(new GUIContent($"Set From Equation/Back/In"), false, () => OnEquationSelected(property, AnimationCurveExtensions.BackIn));
			menu.AddItem(new GUIContent($"Set From Equation/Back/Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.BackOut));
			menu.AddItem(new GUIContent($"Set From Equation/Back/In & Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.BackInOut));
			menu.AddItem(new GUIContent($"Set From Equation/Bounce/In"), false, () => OnEquationSelected(property, AnimationCurveExtensions.BounceIn));
			menu.AddItem(new GUIContent($"Set From Equation/Bounce/Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.BounceOut));
			menu.AddItem(new GUIContent($"Set From Equation/Bounce/In & Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.BounceInOut));
			menu.AddItem(new GUIContent($"Set From Equation/Elastic/In"), false, () => OnEquationSelected(property, AnimationCurveExtensions.ElasticIn));
			menu.AddItem(new GUIContent($"Set From Equation/Elastic/Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.ElasticOut));
			menu.AddItem(new GUIContent($"Set From Equation/Elastic/In & Out"), false, () => OnEquationSelected(property, AnimationCurveExtensions.ElasticInOut));
		}

		private static void OnEquationSelected(SerializedProperty property, CreateCurveFunc createCurve)
		{
			property.animationCurveValue = createCurve(0f, 0f, 1f, 1f);
			property.serializedObject.ApplyModifiedProperties();
		}

		private delegate AnimationCurve CreateCurveFunc(float timeStart, float valueStart, float timeEnd, float valueEnd);
	}
}