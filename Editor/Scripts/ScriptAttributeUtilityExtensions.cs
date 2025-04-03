using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace WondeluxeEditor
{
	/// <summary>
	/// Provides access utility methods implemented in <c href="https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/Inspector/Core/ScriptAttributeGUI/ScriptAttributeUtility.cs">UnityEditor.ScriptAttributeUtility</c>, as well as additional related methods.
	/// </summary>

	public static class ScriptAttributeUtilityExtensions
	{
		#region Static fields

		private static MethodInfo getDrawerTypeForTypeMethodInfo = Type.GetType("UnityEditor.ScriptAttributeUtility, UnityEditor").GetMethod("GetDrawerTypeForType", BindingFlags.Static | BindingFlags.NonPublic);

#if UNITY_2022_3_OR_NEWER
		// Type GetDrawerTypeForType(Type propertyType, Boolean isPropertyTypeAManagedReference)
		private static readonly object[] getDrawerTypeForTypeParams = { null, false };
#else
		// Type GetDrawerTypeForType(Type propertyType)
		private static readonly object[] getDrawerTypeForTypeParams = { null };
#endif

		#endregion

		#region Static API

		/// <summary>
		/// Gets the custom PropertyDrawers for a SerializedProperty.
		/// </summary>
		/// <param name="property">The SerializedProperty to get the custom PropertyDrawers for.</param>
		/// <returns>A list of PropertyDrawers.</returns>

		public static List<PropertyDrawer> GetDrawers(SerializedProperty property)
		{
			List<PropertyDrawer> propertyDrawers = new List<PropertyDrawer>();

			GetDrawers(property, propertyDrawers);

			return propertyDrawers;
		}

		/// <summary>
		/// Gets the custom PropertyDrawers for a SerializedProperty.
		/// </summary>
		/// <param name="property">The SerializedProperty to get the custom PropertyDrawers for.</param>
		/// <param name="propertyDrawers">The output argument that will contain the PropertyDrawers.</param>

		public static void GetDrawers(SerializedProperty property, List<PropertyDrawer> propertyDrawers)
		{
			SerializedProperty parentProperty = property.GetParentProperty();

			if (parentProperty != null && parentProperty.IsList())
			{
				GetDrawers(parentProperty.GetFieldInfo(), propertyDrawers);
			}
			else
			{
				GetDrawers(property.GetFieldInfo(), propertyDrawers);
			}
		}

		/// <summary>
		/// Gets the custom PropertyDrawers for a field.
		/// </summary>
		/// <param name="fieldInfo">FieldInfo to get the custom PropertyDrawers for.</param>
		/// <returns>A list of PropertyDrawers.</returns>

		public static List<PropertyDrawer> GetDrawers(FieldInfo fieldInfo)
		{
			List<PropertyDrawer> propertyDrawers = new List<PropertyDrawer>();

			GetDrawers(fieldInfo, propertyDrawers);

			return propertyDrawers;
		}

		public static void GetDrawers(FieldInfo fieldInfo, List<PropertyDrawer> propertyDrawers)
		{
			if (fieldInfo == null)
			{
				return;
			}

			Type fieldType = fieldInfo.FieldType.IsArray ? fieldInfo.FieldType.GetElementType() : fieldInfo.FieldType;

			if (TryGetDrawer(fieldType, fieldInfo, null, out PropertyDrawer propertyDrawer))
			{
				propertyDrawers.Add(propertyDrawer);
			}

			IEnumerable<PropertyAttribute> attributes = fieldInfo.GetCustomAttributes<PropertyAttribute>(true);

			foreach (PropertyAttribute attribute in attributes)
			{
				if (TryGetDrawer(attribute.GetType(), fieldInfo, attribute, out PropertyDrawer attributeDrawer))
				{
					propertyDrawers.Add(attributeDrawer);
				}
			}
		}

		/// <summary>
		/// Gets the custom PropertyDrawer Type for a given Type.
		/// </summary>
		/// <param name="type">The Type to get the PropertyDrawer Type for.</param>
		/// <returns>A Type, or null.</returns>

		public static Type GetDrawerTypeForType(Type type)
		{
			// Would be really handy if this method was just accessible out of the box.
			// If the below Invoke call throws a parameter count error, uncomment the below code to confirm the GetDrawerTypeForType method signature.

			// ParameterInfo[] paramInfos = getDrawerTypeForTypeMethodInfo.GetParameters();
			//
			// string[] paramStrings = new string[paramInfos.Length];
			//
			// for (int i = 0; i < paramInfos.Length; i++)
			// {
			// 	paramStrings[i] = $"{paramInfos[i].ParameterType.Name} {paramInfos[i].Name}";
			// }
			//
			// Debug.Log($"{getDrawerTypeForTypeMethodInfo.ReturnType.Name} GetDrawerTypeForType({string.Join(", ",  paramStrings)})");

			getDrawerTypeForTypeParams[0] = type;

			return getDrawerTypeForTypeMethodInfo.Invoke(null, getDrawerTypeForTypeParams) as Type;
		}

		/// <summary>
		/// Gets a custom PropertyDrawer instance.
		/// </summary>
		/// <param name="type">The Type to get the PropertyDrawer for.</param>
		/// <param name="fieldInfo">The FieldInfo to assign to the PropertyDrawer.</param>
		/// <param name="attribute">The PropertyAttribute to assign to the PropertyDrawer.</param>
		/// <returns>A PropertyDrawer instance if <c>type</c> has a custom PropertyDrawer, otherwise <c>null</c>.</returns>

		public static PropertyDrawer GetDrawer(Type type, FieldInfo fieldInfo, PropertyAttribute attribute)
		{
			Type propertyDrawerType = GetDrawerTypeForType(type);

			if (propertyDrawerType != null && typeof(PropertyDrawer).IsAssignableFrom(propertyDrawerType))
			{
				PropertyDrawer propertyDrawer = Activator.CreateInstance(propertyDrawerType) as PropertyDrawer;
				propertyDrawerType.GetField("m_FieldInfo", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(propertyDrawer, fieldInfo);
				propertyDrawerType.GetField("m_Attribute", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(propertyDrawer, attribute);

				return propertyDrawer;
			}

			return null;
		}

		/// <summary>
		/// Gets a custom PropertyDrawer instance, if one can be created.
		/// </summary>
		/// <param name="type">The Type to get the PropertyDrawer for.</param>
		/// <param name="fieldInfo">The FieldInfo to assign to the PropertyDrawer.</param>
		/// <param name="attribute">The PropertyAttribute to assign to the PropertyDrawer.</param>
		/// <param name="propertyDrawer">The output argument that will contain the PropertyDrawer, or <c>null</c>.</param>
		/// <returns><c>true</c> if a PropertyDrawer was created, otherwise <c>false</c>.</returns>

		public static bool TryGetDrawer(Type type, FieldInfo fieldInfo, PropertyAttribute attribute, out PropertyDrawer propertyDrawer)
		{
			propertyDrawer = GetDrawer(type, fieldInfo, attribute);

			return (propertyDrawer != null);
		}

		

		#endregion
	}
}