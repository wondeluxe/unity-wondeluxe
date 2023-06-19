using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Wondeluxe;

namespace WondeluxeEditor
{
	/// <summary>
	/// Base Editor for the Wondeluxe custom inspector solution.
	/// WondeluxeEditor handles the attributes: ShowInInspector, Button, Order, Group, FoldoutGroup.
	/// </summary>

	[CanEditMultipleObjects]
	[CustomEditor(typeof(UnityEngine.Object), true)]
	public class WondeluxeEditor : Editor
	{
		#region Constants

		/// <summary>
		/// Height used for buttons created using the Button attribute.
		/// </summary>

		internal const float ButtonHeight = 25f;

		#endregion

		#region Internal fields

		/// <summary>
		/// Members to display using the ShowInInspector attribute.
		/// Fields and properties and all stored in the same list so they can be sorted and displayed in order of declaration.
		/// </summary>

		//private readonly SerializedPropertyInfo rootPropertyInfo = new SerializedPropertyInfo(null, 0, 0, 0, 0);
		private SerializedPropertyInfo rootPropertyInfo = new SerializedPropertyInfo(null, 0, 0, new SerializedPropertyGroupInfo(null, 0, 0, false));

		private readonly Stack<List<SerializedPropertyInfo>> serializedListElementInfosStack = new Stack<List<SerializedPropertyInfo>>();

		private readonly Dictionary<string, ReorderableList> reorderableLists = new Dictionary<string, ReorderableList>();
		private readonly Dictionary<int, FoldoutGroup> foldoutGroups = new Dictionary<int, FoldoutGroup>();

		private int indentLevel;

		/// <summary>
		/// childrenHandled field is used for the complicated case when a custom property drawer is invoked via <c>EditorGUILayout.PropertyField(...)</c> and handles the drawing of a property's children.
		/// When this is the case, the children shouldn't be drawn from the WondeluxeEditor instance.
		/// </summary>

		private bool childrenHandled;

		#endregion

		#region Unity messages

		protected virtual void OnEnable()
		{
			// First property is Base.
			//SerializedProperty serializedProperty = serializedObject.GetIterator();

			// Second property is Script reference.
			//serializedProperty.NextVisible(true);

			//GetSerializedPropertyInfo(serializedProperty, rootPropertyInfo.Children);
			//GetShownMembers(target, rootPropertyInfo.ShownMembers);
			//GetButtonMethods(target, rootPropertyInfo.ButtonMethods);
		}

		protected virtual void OnDisable()
		{
		}

		public override void OnInspectorGUI()
		{
			//Debug.Log($"<b>{name} OnInspectorGUI</b>");

			rootPropertyInfo = new SerializedPropertyInfo(null, 0, 0, new SerializedPropertyGroupInfo(null, 0, 0, false));

			// First property is Base.
			SerializedProperty serializedProperty = serializedObject.GetIterator();

			// Second property is Script reference.
			if (serializedProperty.NextVisible(true))
			{
				GetSerializedPropertyInfo(serializedProperty, rootPropertyInfo.Children);
				GetShownMembers(target, rootPropertyInfo.ShownMembers);
				GetButtonMethods(target, rootPropertyInfo.ButtonMethods);

				if (rootPropertyInfo.RequiresCustomInspector)
				{
					//Debug.Log($"<b>{name} DrawDefaultInspector</b>");

					//DrawDefaultInspector();

					indentLevel = EditorGUI.indentLevel;
					WondeluxePropertyDrawer.IndentLevel = EditorGUI.indentLevel;
					WondeluxePropertyDrawer.InWondeluxeEditor = true;
					WondeluxePropertyDrawer.OnChildrenHandled += OnPropertyChildrenHandled;

					//EditorGUILayout.Space();
					//EditorGUILayout.LabelField("Custom Inspector", EditorStyles.boldLabel);

					//Debug.Log($"<b>{name} DrawCustomInspector</b>");

					DrawCustomInspector();

					WondeluxePropertyDrawer.InWondeluxeEditor = false;
					WondeluxePropertyDrawer.OnChildrenHandled -= OnPropertyChildrenHandled;
				}
				else
				{
					DrawDefaultInspector();
				}
			}
		}

		#endregion

		#region Internal methods

		private int IndentLevel
		{
			get => indentLevel;
			set
			{
				EditorGUI.indentLevel = value + (EditorGUI.indentLevel - indentLevel);
				indentLevel = value;
				WondeluxePropertyDrawer.IndentLevel = value;
			}
		}

		private void DrawCustomInspector()
		{
			bool guiEnabled = GUI.enabled;

			SerializedProperty scriptProperty = serializedObject.FindProperty("m_Script");

			GUI.enabled = false;
			EditorGUILayout.PropertyField(scriptProperty);
			GUI.enabled = guiEnabled;

			DrawPropertiesLayout(rootPropertyInfo.Children);
			DrawShownMembersLayout(rootPropertyInfo.ShownMembers, target);
			DrawButtonsLayout(rootPropertyInfo.ButtonMethods, target);

			serializedObject.ApplyModifiedProperties();
		}

		private static void DrawShownMembersLayout(List<MemberInfo> shownMembers, object target)
		{
			bool guiEnabled = GUI.enabled;

			GUI.enabled = false;

			foreach (MemberInfo member in shownMembers)
			{
				if (member is FieldInfo field)
				{
					EditorGUILayoutExtensions.ValueField(ObjectNames.NicifyVariableName(field.Name), field.GetValue(target));
					continue;
				}

				if (member is PropertyInfo property)
				{
					EditorGUILayoutExtensions.ValueField(ObjectNames.NicifyVariableName(property.Name), property.GetValue(target));
					continue;
				}
			}

			GUI.enabled = guiEnabled;
		}

		private static void DrawShownMembers(List<MemberInfo> shownMembers, object target, ref Rect rect)
		{
			bool guiEnabled = GUI.enabled;

			GUI.enabled = false;

			foreach (MemberInfo member in shownMembers)
			{
				if (member is FieldInfo field)
				{
					EditorGUIExtensions.DrawValueField(ObjectNames.NicifyVariableName(field.Name), field.GetValue(target), ref rect);
					continue;
				}

				if (member is PropertyInfo property)
				{
					EditorGUIExtensions.DrawValueField(ObjectNames.NicifyVariableName(property.Name), property.GetValue(target), ref rect);
					continue;
				}
			}

			GUI.enabled = guiEnabled;
		}

		private static void DrawButtonsLayout(List<MethodInfo> buttonMethods, object target)
		{
			bool guiEnabled = GUI.enabled;

			foreach (MethodInfo method in buttonMethods)
			{
				ButtonAttribute attribute = method.GetCustomAttribute<ButtonAttribute>();

				GUI.enabled = (guiEnabled && attribute.Enabled);

				// Manually create rect as GUILayout.Button doesn't respect indentation.

				Rect rect = EditorGUI.IndentedRect(GUILayoutUtility.GetRect(0f, ButtonHeight));

				if (GUI.Button(rect, ObjectNames.NicifyVariableName(method.Name)))
				{
					method.Invoke(target, null);
				}

				GUI.enabled = guiEnabled;
			}
		}

		private static void DrawButtons(List<MethodInfo> buttonMethods, object target, ref Rect position)
		{
			bool guiEnabled = GUI.enabled;

			foreach (MethodInfo method in buttonMethods)
			{
				ButtonAttribute attribute = method.GetCustomAttribute<ButtonAttribute>();

				GUI.enabled = (guiEnabled && attribute.Enabled);

				Rect rect = EditorGUI.IndentedRect(new Rect(position.x, position.y, position.width, ButtonHeight));

				if (EditorGUIExtensions.DrawButton(rect, ObjectNames.NicifyVariableName(method.Name), ref position))
				{
					method.Invoke(target, null);
				}

				GUI.enabled = guiEnabled;
			}
		}

		private void DrawPropertiesLayout(List<SerializedPropertyInfo> serializedPropertyInfos)
		{
			serializedPropertyInfos.Sort(SerializedPropertyInfo.Compare);

			FoldoutGroup currentFoldoutGroup = null;

			foreach (SerializedPropertyInfo info in serializedPropertyInfos)
			{
				FoldoutGroup foldoutGroup = info.Group.Foldout ? GetFoldoutGroup(info.Group) : null;

				if (foldoutGroup != currentFoldoutGroup)
				{
					if (currentFoldoutGroup != null)
					{
						if (currentFoldoutGroup.Expanded)
						{
							IndentLevel--;
							WondeluxePropertyDrawer.GroupIndentLevel--;
						}
					}

					if (foldoutGroup != null)
					{
						foldoutGroup.Expanded = EditorGUILayout.Foldout(foldoutGroup.Expanded, foldoutGroup.Name, true);

						if (foldoutGroup.Expanded)
						{
							IndentLevel++;
							WondeluxePropertyDrawer.GroupIndentLevel++;
						}
					}

					currentFoldoutGroup = foldoutGroup;
				}

				if (foldoutGroup == null || foldoutGroup.Expanded)
				{
					DrawPropertyLayout(info);
				}
			}

			if (currentFoldoutGroup != null && currentFoldoutGroup.Expanded)
			{
				IndentLevel--;
				WondeluxePropertyDrawer.GroupIndentLevel--;
			}
		}

		private void DrawProperties(List<SerializedPropertyInfo> serializedPropertyInfos, ref Rect position)
		{
			serializedPropertyInfos.Sort(SerializedPropertyInfo.Compare);

			FoldoutGroup currentFoldoutGroup = null;

			foreach (SerializedPropertyInfo info in serializedPropertyInfos)
			{
				FoldoutGroup foldoutGroup = info.Group.Foldout ? GetFoldoutGroup(info.Group) : null;

				if (foldoutGroup != currentFoldoutGroup)
				{
					if (currentFoldoutGroup != null)
					{
						if (currentFoldoutGroup.Expanded)
						{
							IndentLevel--;
							WondeluxePropertyDrawer.GroupIndentLevel--;
						}
					}

					if (foldoutGroup != null)
					{
						foldoutGroup.Expanded = EditorGUIExtensions.DrawFoldout(foldoutGroup.Name, foldoutGroup.Expanded, ref position);

						if (foldoutGroup.Expanded)
						{
							IndentLevel++;
							WondeluxePropertyDrawer.GroupIndentLevel++;
						}
					}

					currentFoldoutGroup = foldoutGroup;
				}

				if (foldoutGroup == null || foldoutGroup.Expanded)
				{
					DrawProperty(info, ref position);
				}
			}

			if (currentFoldoutGroup != null && currentFoldoutGroup.Expanded)
			{
				IndentLevel--;
				WondeluxePropertyDrawer.GroupIndentLevel--;
			}
		}

		private void DrawPropertyLayout(SerializedPropertyInfo serializedPropertyInfo)
		{
			SerializedProperty serializedProperty = serializedObject.FindProperty(serializedPropertyInfo.Path);

			//Debug.Log($"<b>DrawPropertyLayout {serializedProperty.displayName} (propertyPath = {serializedProperty.propertyPath}, propertyType = {serializedProperty.propertyType}, isArray = {serializedProperty.isArray}, propertyDrawers = PropertyDrawer[{serializedPropertyInfo.PropertyDrawers.Count}])</b>");

			if (serializedPropertyInfo.Children.Count > 0)
			{
				bool isList = serializedProperty.IsList();

				//Debug.Log($"isList = {isList}");

				bool drawChildren;

				if (isList)
				{
					// Need a custom draw method for lists as EditorGUILayout.PropertyField doesn't draw the foldout and array size fields when includeChildren is false.
					serializedProperty.isExpanded = DrawListHeaderLayout(serializedProperty);
					drawChildren = serializedProperty.isExpanded;
				}
				else
				{
					// EditorGUILayout.PropertyField uses custom property drawers, however we only modify serializedProperty.isExpanded if childrenHandled is false.
					// If childrenHandled is true, a custom property drawer drew the children and has managed the isExpanded property.

					bool hasChildrenAndIsExpanded = EditorGUILayout.PropertyField(serializedProperty, new GUIContent(serializedProperty.displayName), false);

					if (childrenHandled)
					{
						childrenHandled = false;
						drawChildren = false;
					}
					else
					{
						serializedProperty.isExpanded = hasChildrenAndIsExpanded;
						drawChildren = hasChildrenAndIsExpanded;
					}
				}

				if (drawChildren)
				{
					if (isList)
					{
						DrawListLayout(serializedProperty, serializedPropertyInfo.Children);
					}
					else
					{
						object propertyValue = serializedProperty.GetValue();

						IndentLevel++;
						DrawPropertiesLayout(serializedPropertyInfo.Children);
						DrawShownMembersLayout(serializedPropertyInfo.ShownMembers, propertyValue);
						DrawButtonsLayout(serializedPropertyInfo.ButtonMethods, propertyValue);
						IndentLevel--;
					}
				}
			}
			else
			{
				// Handles custom property drawers.
				EditorGUILayout.PropertyField(serializedProperty, true);
			}
		}

		private void DrawProperty(SerializedPropertyInfo serializedPropertyInfo, ref Rect rect)
		{
			SerializedProperty serializedProperty = serializedObject.FindProperty(serializedPropertyInfo.Path);

			//Debug.Log($"<b>DrawProperty {serializedProperty.displayName} (propertyPath = {serializedProperty.propertyPath}, propertyType = {serializedProperty.propertyType}, isArray = {serializedProperty.isArray}, propertyDrawers = PropertyDrawer[{serializedPropertyInfo.PropertyDrawers.Count}], rect = {rect})</b>");

			if (serializedPropertyInfo.Children.Count > 0)
			{
				bool isList = serializedProperty.IsList();

				if (isList)
				{
					serializedProperty.isExpanded = DrawListHeader(serializedProperty, ref rect);
				}
				else
				{
					// TODO Handle isExpanded...
					//serializedProperty.isExpanded = EditorGUIExtensions.DrawFoldout(serializedProperty.displayName, serializedProperty.isExpanded, ref rect);
					DrawField(serializedPropertyInfo, serializedProperty, ref rect);
				}

				if (serializedProperty.isExpanded)
				{
					if (isList)
					{
						DrawList(serializedProperty, serializedPropertyInfo.Children, ref rect);
					}
					else
					{
						object propertyValue = serializedProperty.GetValue();

						IndentLevel++;
						DrawProperties(serializedPropertyInfo.Children, ref rect);
						DrawShownMembers(serializedPropertyInfo.ShownMembers, propertyValue, ref rect);
						DrawButtons(serializedPropertyInfo.ButtonMethods, propertyValue, ref rect);
						IndentLevel--;
					}
				}
			}
			else
			{
				//Debug.Log($"EditorGUIExtensions.DrawPropertyField (serializedProperty = {serializedProperty.propertyPath}, rect = {rect})");

				EditorGUIExtensions.DrawPropertyField(serializedProperty, ref rect);
			}
		}

		private bool DrawListHeaderLayout(SerializedProperty serializedProperty)
		{
			Rect rect = EditorGUILayout.GetControlRect(true);
			return DrawListHeader(serializedProperty, ref rect);
		}

		private bool DrawListHeader(SerializedProperty serializedProperty, ref Rect position)
		{
			//Debug.Log($"DrawListHeader (serializedProperty = {serializedProperty.displayName}, position = {position})");

			// TODO Need to handle array size changes.

			return EditorGUIExtensions.DrawPropertyField(serializedProperty, serializedProperty.displayName, false, ref position);
		}

		private void DrawListLayout(SerializedProperty serializedProperty, List<SerializedPropertyInfo> elementPropertyInfos)
		{
			//Debug.Log($"DrawListLayout (serializedProperty = {serializedProperty.displayName} (IndentLevel = {IndentLevel}, EditorGUI.indentLevel = {EditorGUI.indentLevel}))");

			serializedListElementInfosStack.Push(elementPropertyInfos);

			// NOTE ReorderableList resets EditorGUI.indentLevel so garbo workaround is required.

			ReorderableList reorderableList = GetReorderableList(serializedProperty);

			float listHeight = reorderableList.GetHeight();

			Rect sourceRect = GUILayoutUtility.GetRect(0f, listHeight);
			Rect listRect = EditorGUI.IndentedRect(sourceRect);

			float indentOffset = (IndentLevel + 2) * EditorGUIExtensions.IndentWidth;
			float labelWidth = EditorGUIUtility.labelWidth;

			//Debug.Log($"propertyDepth = {propertyDepth}, EditorGUI.indentLevel = {EditorGUI.indentLevel}");
			//Debug.Log($"sourceRect = {sourceRect}, listRect = {listRect}");

			//Debug.Log($"EditorGUIUtility.labelWidth = {EditorGUIUtility.labelWidth}, EditorGUI.indentLevel = {EditorGUI.indentLevel}, propertyDepth = {propertyDepth}");

			EditorGUIUtility.labelWidth -= indentOffset;

			reorderableList.DoList(listRect);

			EditorGUIUtility.labelWidth = labelWidth;

			serializedListElementInfosStack.Pop();
		}

		private void DrawList(SerializedProperty serializedProperty, List<SerializedPropertyInfo> elementPropertyInfos, ref Rect rect)
		{
			//Debug.Log($"DrawList (serializedProperty = {serializedProperty.displayName}, rect = {rect})");

			serializedListElementInfosStack.Push(elementPropertyInfos);

			// NOTE ReorderableList resets EditorGUI.indentLevel so garbo workaround is required.

			ReorderableList reorderableList = GetReorderableList(serializedProperty);
			reorderableList.DoList(rect);

			float height = rect.height - reorderableList.GetHeight();

			if (height < 0f)
			{
				Debug.LogError($"Calculated height is negative ({height})!");
			}

			rect = new Rect(rect.x, rect.y, rect.width, height);

			serializedListElementInfosStack.Pop();

			//Debug.Log($"End drawing list '{serializedProperty.displayName}'.");
		}

		private void DrawField(SerializedPropertyInfo serializedPropertyInfo, SerializedProperty serializedProperty, ref Rect position)
		{
			//Debug.Log($"DrawField (serializedProperty = {serializedProperty.propertyPath}, position = {position})");

			// TODO Need to return if field is expanded or not.

			GUIContent label = new GUIContent(serializedProperty.displayName);

			float height = GetPropertyHeight(serializedPropertyInfo, serializedProperty, label, false);

			//Debug.Log($"height = {height}");

			Rect rect = new Rect(position.x, position.y, position.width, height);

			WondeluxePropertyDrawer.DoGUI(rect, serializedProperty, label, serializedPropertyInfo.PropertyDrawers, false);

			position = EditorGUIExtensions.DrawnFieldRect(position, rect, EditorGUIUtility.standardVerticalSpacing);
		}

		private float OnGetListElementHeight(int index)
		{
			SerializedPropertyInfo serializedElementInfo = serializedListElementInfosStack.Peek()[index];
			SerializedProperty serializedProperty = serializedObject.FindProperty(serializedElementInfo.Path);
			GUIContent label = new GUIContent(serializedProperty.displayName);

			// TODO Implement ReorderableList height if element is array/list.

			return GetPropertyHeight(serializedElementInfo, serializedProperty, label, serializedProperty.isExpanded);
		}

		private void OnDrawListElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			//Debug.Log($"OnDrawListElement (rect = {rect}, index = {index})");

			// Rect needs to be shifted over so that the control doesn't overlap the element's handle.

			rect.x += 8f;
			rect.width -= 8f;

			DrawProperty(serializedListElementInfosStack.Peek()[index], ref rect);
		}

		private ReorderableList GetReorderableList(SerializedProperty serializedProperty)
		{
			// https://blog.terresquall.com/2020/03/creating-reorderable-lists-in-the-unity-inspector/
			// https://github.com/Unity-Technologies/UnityCsReference/blob/master/Editor/Mono/GUI/ReorderableList.cs

			// https://docs.unity3d.com/ScriptReference/EditorStyles.html
			// https://docs.unity3d.com/ScriptReference/GUIStyle.html
			// https://docs.unity3d.com/ScriptReference/GUIStyleState.html

			if (!reorderableLists.ContainsKey(serializedProperty.propertyPath))
			{
				ReorderableList reorderableList = new ReorderableList(serializedObject, serializedProperty, true, false, true, true);
				reorderableList.elementHeightCallback = OnGetListElementHeight;
				reorderableList.drawElementCallback = OnDrawListElement;

				reorderableLists.Add(serializedProperty.propertyPath, reorderableList);
			}

			return reorderableLists[serializedProperty.propertyPath];
		}

		//private FoldoutGroup GetFoldoutGroup(SerializedPropertyGroupInfo groupInfo, SerializedProperty serializedProperty)
		private FoldoutGroup GetFoldoutGroup(SerializedPropertyGroupInfo info)
		{
			if (!foldoutGroups.ContainsKey(info.StartIndex))
			{
				//foldoutGroups.Add(groupInfo.StartIndex, new FoldoutGroup(groupInfo.Name, serializedProperty.GetPropertyDepth()));
				foldoutGroups.Add(info.StartIndex, new FoldoutGroup(info.Name));
			}

			return foldoutGroups[info.StartIndex];
		}

		private void OnPropertyChildrenHandled(SerializedProperty property)
		{
			childrenHandled = true;
		}

		#endregion

		#region Static methods

		private static void GetSerializedPropertyInfo(SerializedProperty property, List<SerializedPropertyInfo> serializedPropertyInfos)
		{
			Regex depthSeparatorRegex = new Regex(Regex.Escape("."));

			Stack<List<SerializedPropertyInfo>> propertyInfosStack = new Stack<List<SerializedPropertyInfo>>();
			List<SerializedPropertyInfo> currentPropertyInfos = serializedPropertyInfos;
			int currentPropertyDepth = 0;
			int propertyIndex = 0;

			Stack<Dictionary<string, SerializedPropertyGroupInfo>> groupInfosStack = new Stack<Dictionary<string, SerializedPropertyGroupInfo>>();
			Dictionary<string, SerializedPropertyGroupInfo> currentGroupInfos = new Dictionary<string, SerializedPropertyGroupInfo>();

			while (property.NextVisible(true))
			{
				//Debug.Log($"property = {property.propertyPath}");

				if (property.propertyPath.Contains("Array.size"))
				{
					continue;
				}

				int depth = property.depth;

				if (depth > currentPropertyDepth)
				{
					propertyInfosStack.Push(currentPropertyInfos);
					currentPropertyInfos = currentPropertyInfos[currentPropertyInfos.Count - 1].Children;
					currentPropertyDepth++;

					groupInfosStack.Push(currentGroupInfos);
					currentGroupInfos = new Dictionary<string, SerializedPropertyGroupInfo>();
				}
				else if (depth < currentPropertyDepth)
				{
					while (currentPropertyDepth > depth)
					{
						currentPropertyInfos = propertyInfosStack.Pop();
						currentPropertyDepth--;

						currentGroupInfos = groupInfosStack.Pop();
					}
				}

				int order = 0;

				OrderAttribute orderAttribute = property.GetAttribute<OrderAttribute>(true);

				if (orderAttribute != null)
				{
					order = orderAttribute.PropertyOrder;
				}

				//bool inGroup = false;
				SerializedPropertyGroupInfo groupInfo = new SerializedPropertyGroupInfo(null, order, propertyIndex, false);

				GroupAttribute groupAttribute = property.GetAttribute<GroupAttribute>(true);

				if (groupAttribute != null)
				{
					//inGroup = true;

					if (currentGroupInfos.ContainsKey(groupAttribute.Name))
					{
						groupInfo = currentGroupInfos[groupAttribute.Name];
					}
					else
					{
						groupInfo.Name = groupAttribute.Name;
						groupInfo.Foldout = (groupAttribute is FoldoutGroupAttribute);
						currentGroupInfos.Add(groupAttribute.Name, groupInfo);
					}
				}

				//SerializedPropertyInfo propertyInfo = new SerializedPropertyInfo(property.propertyPath, propertyIndex++, order, groupInfo.Order, groupInfo.StartIndex);
				SerializedPropertyInfo propertyInfo = new SerializedPropertyInfo(property.propertyPath, propertyIndex++, order, groupInfo);

				object propertyValue = property.GetValue();

				ScriptAttributeUtilityExtensions.GetDrawers(property, propertyInfo.PropertyDrawers);
				GetShownMembers(propertyValue, propertyInfo.ShownMembers);
				GetButtonMethods(propertyValue, propertyInfo.ButtonMethods);

				currentPropertyInfos.Add(propertyInfo);
			}
		}

		private static void GetShownMembers(object target, List<MemberInfo> shownMembers)
		{
			if (target == null)
			{
				return;
			}

			Type targetType = target.GetType();

			FieldInfo[] fields = targetType.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

			foreach (FieldInfo field in fields)
			{
				if (field.GetCustomAttribute<ShowInInspectorAttribute>() != null)
				{
					shownMembers.Add(field);
				}
			}

			PropertyInfo[] properties = targetType.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

			foreach (PropertyInfo property in properties)
			{
				if (property.GetMethod != null && property.GetCustomAttribute<ShowInInspectorAttribute>() != null)
				{
					shownMembers.Add(property);
				}
			}

			shownMembers.Sort(CompareShownMembers);
		}

		private static int CompareShownMembers(MemberInfo memberA, MemberInfo memberB)
		{
			ShowInInspectorAttribute attributeA = memberA.GetCustomAttribute<ShowInInspectorAttribute>();
			ShowInInspectorAttribute attributeB = memberB.GetCustomAttribute<ShowInInspectorAttribute>();

			return (attributeA.LineNumber < attributeB.LineNumber) ? -1 : (attributeA.LineNumber > attributeB.LineNumber) ? 1 : 0;
		}

		private static void GetButtonMethods(object target, List<MethodInfo> buttonMethods)
		{
			if (target == null)
			{
				return;
			}

			Type targetType = target.GetType();

			MethodInfo[] methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

			foreach (MethodInfo method in methods)
			{
				if (method.GetCustomAttribute<ButtonAttribute>() != null)
				{
					buttonMethods.Add(method);
				}
			}

			buttonMethods.Sort(CompareButtonMethods);
		}

		private static int CompareButtonMethods(MethodInfo memberA, MethodInfo memberB)
		{
			ButtonAttribute attributeA = memberA.GetCustomAttribute<ButtonAttribute>();
			ButtonAttribute attributeB = memberB.GetCustomAttribute<ButtonAttribute>();

			return (attributeA.LineNumber < attributeB.LineNumber) ? -1 : (attributeA.LineNumber > attributeB.LineNumber) ? 1 : 0;
		}

		private float GetPropertyHeight(SerializedPropertyInfo serializedPropertyInfo, bool includeChildren)
		{
			SerializedProperty serializedProperty = serializedObject.FindProperty(serializedPropertyInfo.Path);
			GUIContent label = new GUIContent(serializedProperty.displayName);

			return GetPropertyHeight(serializedPropertyInfo, serializedProperty, label, includeChildren);
		}

		private float GetPropertyHeight(SerializedPropertyInfo serializedPropertyInfo, SerializedProperty serializedProperty, GUIContent label, bool includeChildren)
		{
			float height = WondeluxePropertyDrawer.GetHeight(serializedProperty, label, serializedPropertyInfo.PropertyDrawers, includeChildren);

			if (includeChildren)
			{
				HashSet<int> foldoutGroups = new HashSet<int>();

				foreach (SerializedPropertyInfo childPropertyInfo in serializedPropertyInfo.Children)
				{
					if (childPropertyInfo.Group.Foldout)
					{
						foldoutGroups.Add(childPropertyInfo.Group.StartIndex);

						FoldoutGroup foldoutGroup = GetFoldoutGroup(childPropertyInfo.Group);

						if (!foldoutGroup.Expanded)
						{
							height -= GetPropertyHeight(childPropertyInfo, true) + EditorGUIUtility.standardVerticalSpacing;
						}
					}
				}

				height += foldoutGroups.Count * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);

				foreach (MemberInfo shownMember in serializedPropertyInfo.ShownMembers)
				{
					height += EditorGUIUtility.standardVerticalSpacing;

					if (shownMember is FieldInfo fieldInfo)
					{
						height += EditorGUIExtensions.GetPropertyHeight(fieldInfo.FieldType, label);
					}
					else if (shownMember is PropertyInfo propertyInfo)
					{
						height += EditorGUIExtensions.GetPropertyHeight(propertyInfo.PropertyType, label);
					}
				}

				height += serializedPropertyInfo.ButtonMethods.Count * (EditorGUIUtility.standardVerticalSpacing + ButtonHeight);
			}

			return height;
		}

		#endregion
	}
}