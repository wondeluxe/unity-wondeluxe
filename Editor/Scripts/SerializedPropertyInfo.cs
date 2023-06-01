using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace WondeluxeEditor
{
	internal class SerializedPropertyInfo
	{
		public string Path;
		public int DefaultIndex;
		public int Order;
		public SerializedPropertyGroupInfo Group;
		public readonly List<PropertyDrawer> PropertyDrawers = new List<PropertyDrawer>();
		public readonly List<SerializedPropertyInfo> Children = new List<SerializedPropertyInfo>();
		public readonly List<MemberInfo> ShownMembers = new List<MemberInfo>();
		public readonly List<MethodInfo> ButtonMethods = new List<MethodInfo>();

		public SerializedPropertyInfo(string path, int defaultIndex, int order, SerializedPropertyGroupInfo group)
		{
			Path = path;
			DefaultIndex = defaultIndex;
			Order = order;
			Group = group;
		}

		public bool RequiresCustomInspector
		{
			get
			{
				if (ButtonMethods.Count > 0)
				{
					return true;
				}

				if (ShownMembers.Count > 0)
				{
					return true;
				}

				if (Order != 0)
				{
					return true;
				}

				if (!string.IsNullOrEmpty(Group.Name))
				{
					return true;
				}

				foreach (SerializedPropertyInfo info in Children)
				{
					if (info.RequiresCustomInspector)
					{
						return true;
					}
				}

				return false;
			}
		}

		public override string ToString()
		{
			return $"(Path = {Path}, Order = {Order}, Group = {Group}, Children = {{{string.Join(", ", Children)}}})";
		}

		public static int Compare(SerializedPropertyInfo infoA, SerializedPropertyInfo infoB)
		{
			if (infoA.Group.StartIndex == infoB.Group.StartIndex)
			{
				// If GroupStartIndex equals DefaultIndex then the property started the group and should appear first.

				if (infoA.Group.StartIndex == infoA.DefaultIndex)
				{
					return -1;
				}

				if (infoB.Group.StartIndex == infoB.DefaultIndex)
				{
					return 1;
				}

				if (infoA.Order < infoB.Order)
				{
					return -1;
				}

				if (infoA.Order > infoB.Order)
				{
					return 1;
				}

				if (infoA.DefaultIndex < infoB.DefaultIndex)
				{
					return -1;
				}

				if (infoA.DefaultIndex > infoB.DefaultIndex)
				{
					return 1;
				}
			}
			else
			{
				// GroupOrder will match Order for properties that aren't in groups.

				if (infoA.Group.Order < infoB.Group.Order)
				{
					return -1;
				}

				if (infoA.Group.Order > infoB.Group.Order)
				{
					return 1;
				}

				// GroupStartIndex will match DefaultIndex for properties that aren't in groups.

				if (infoA.Group.StartIndex < infoB.Group.StartIndex)
				{
					return -1;
				}

				if (infoA.Group.StartIndex > infoB.Group.StartIndex)
				{
					return 1;
				}
			}

			return 0;
		}
	}
}