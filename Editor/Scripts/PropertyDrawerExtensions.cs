using System.Reflection;
using UnityEditor;

namespace WondeluxeEditor
{
	public static class PropertyDrawerExtensions
	{
		public static FieldInfo[] GetFields(this PropertyDrawer propertyDrawer)
		{
			return propertyDrawer.GetFields(BindingFlags.Instance | BindingFlags.Public);
		}

		public static FieldInfo[] GetFields(this PropertyDrawer propertyDrawer, BindingFlags bindingFlags)
		{
			return propertyDrawer.fieldInfo.FieldType.GetFields(bindingFlags);
		}
	}
}