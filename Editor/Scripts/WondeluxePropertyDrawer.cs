using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Object = UnityEngine.Object;

namespace WondeluxeEditor
{
	// TODO Change members to internal.
	// TODO Add property validation (validate attribute is used on correct type).

	/// <summary>
	/// Base class for creating custom PropertyDrawers for use in the Wondeluxe custom inspector solution.
	/// Supports multiple PropertyDrawers used on the same member as long as only one PropertyDrawer may modify the property's label,
	/// and only one PropertyDrawer may modify the property's layout (way GUI is drawn). Can be mixed with Unity's default PropertyDrawers
	/// as long as no other PropertyDrawers modify the property's layout.
	/// </summary>

	public abstract class WondeluxePropertyDrawer : PropertyDrawer
	{
		#region Constants

		public const int ObjectPickerControlID = 863595;

		#endregion

		#region Internal fields

		private List<PropertyDrawer> propertyDrawers;

		#endregion

		#region Internal methods

		private List<PropertyDrawer> PropertyDrawers
		{
			get
			{
				if (propertyDrawers == null)
				{
					//Debug.Log($"Create propertyDrawers (Type = {fieldInfo.FieldType})");

					propertyDrawers = ScriptAttributeUtilityExtensions.GetDrawers(fieldInfo);
				}

				return propertyDrawers;
			}
		}

		#endregion

		#region Virtual members

		/// <summary>
		/// Override to return <c>true</c> if the property drawer changes the property's label. If <c>true</c>, the property drawer must override <c href="GetCustomLabel">GetCustomLabel</c>.
		/// </summary>

		public virtual bool HasCustomLabel => false;

		/// <summary>
		/// Override to return <c>true</c> if the property drawer changes the appearance of the property. If <c>true</c>, the property drawer must override <c href="OnCustomGUI">OnCustomGUI</c> and optionally <c href="GetCustomPropertyHeight">GetCustomPropertyHeight</c>.
		/// </summary>

		public virtual bool HasCustomLayout => false;

		/// <summary>
		/// Override to return <c>true</c> if the property drawer appends content to the property's appearance. If <c>true</c>, the property drawer must override <c href="GetAdditionalPropertyHeight">GetAdditionalPropertyHeight</c> and <c href="OnAdditionalGUI">OnAdditionalGUI</c>.
		/// </summary>

		public virtual bool HasAdditionalLayout => false;

		/// <summary>
		/// Override to return <c>true</c> if the property drawer is a validator for an object property. If <c>true</c>, the property drawer must override <c href="GetValidObjectReference">GetValidObjectReference</c>.
		/// </summary>

		public virtual bool ValidateObjectSelection => false;

		/// <summary>
		/// Override to provide a modified label for the displayed property. Will only be called if <c href="HasCustomLabel">HasCustomLabel</c> is overridden to return <c>true</c>.
		/// </summary>
		/// <param name="property">The SerializedProperty to make the custom GUI for.</param>
		/// <param name="label">The label of the property.</param>
		/// <returns>The new label for the property.</returns>

		public virtual GUIContent GetCustomLabel(SerializedProperty property, GUIContent label)
		{
			throw new NotImplementedException($"HasCustomLabel returns true, GetCustomLabel must be overridden.");
		}

		/// <summary>
		/// Override to provide a modified height for the property. The returned height should not factor in additional height for appended content. Will only be called if <c href="HasCustomLayout">HasCustomLayout</c> is overridden to return <c>true</c>.
		/// </summary>
		/// <param name="property">The SerializedProperty to make the custom GUI for.</param>
		/// <param name="label">The label of the property.</param>
		/// <returns>The new height for the property.</returns>

		public virtual float GetCustomPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property, label, true);
		}

		/// <summary>
		/// Override to provide additional space for appended GUI content. Will only be called if <c href="HasAdditionalLayout">HasAdditionalLayout</c> is overridden to return <c>true</c>.
		/// </summary>
		/// <param name="property">The SerializedProperty to make the custom GUI for.</param>
		/// <param name="label">The label of the property.</param>
		/// <returns>The additional space for the property.</returns>

		public virtual float GetAdditionalPropertyHeight(SerializedProperty property, GUIContent label)
		{
			throw new NotImplementedException($"HasAdditionalLayout returns true, GetAdditionalPropertyHeight must be overridden.");
		}

		/// <summary>
		/// Override to validate the selection of an object reference property. Will only be called if <c href="ValidateObjectSelection">ValidateObjectSelection</c> is overridden to return <c>true</c>.
		/// The implementation should handle object picker and drag and drop events.
		/// </summary>
		/// <param name="position">Rectangle on the screen to use for the property GUI.</param>
		/// <param name="property">The SerializedProperty to validate selection for.</param>
		/// <returns>A valid Object, or <c>null</c>.</returns>

		public virtual Object GetValidObjectSelection(Rect position, SerializedProperty property)
		{
			throw new NotImplementedException($"ValidateObjectSelection returns true, GetValidObjectReference must be overridden.");
		}

		/// <summary>
		/// Called before the property's GUI is drawn.
		/// </summary>
		/// <param name="property">The SerializedProperty to make the custom GUI for.</param>
		/// <param name="label">The label of the property.</param>

		public virtual void OnPreGUI(SerializedProperty property, GUIContent label)
		{
		}

		/// <summary>
		/// Override to draw custom GUI for the property. Will only be called if <c href="HasCustomLayout">HasCustomLayout</c> is overridden to return <c>true</c>.
		/// </summary>
		/// <param name="position">Rectangle on the screen to use for the property GUI.</param>
		/// <param name="property">The SerializedProperty to make the custom GUI for.</param>
		/// <param name="label">The label of the property.</param>

		public virtual void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
		{
		}

		/// <summary>
		/// Override to append additional GUI content to the property. Will only be called if <c href="HasAdditionalLayout">HasAdditionalLayout</c> is overridden to return <c>true</c>.
		/// </summary>
		/// <param name="position">Rectangle on the screen to use for the property GUI.</param>
		/// <param name="property">The SerializedProperty to make the custom GUI for.</param>
		/// <param name="label">The label of the property.</param>

		public virtual void OnAdditionalGUI(Rect position, SerializedProperty property, GUIContent label)
		{
		}

		/// <summary>
		/// Called after the property's GUI is drawn.
		/// </summary>
		/// <param name="property">The SerializedProperty to make the custom GUI for.</param>
		/// <param name="label">The label of the property.</param>
		/// <param name="valueChanged"><c>true</c> if the property's value was changed.</param>

		public virtual void OnPostGUI(SerializedProperty property, GUIContent label, bool valueChanged)
		{
		}

		#endregion

		#region Public API

		sealed public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			//Debug.Log($"GetPropertyHeight ({label.text} = {property.propertyPath} ({fieldInfo.FieldType}, attribute = {attribute}))");

			return GetHeight(property, label, PropertyDrawers, true);
		}

		sealed public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// Need to make a copy of label as EditorGUI.GetPropertyHeight triggers original to be cleared/recycled.

			DoGUI(position, property, new GUIContent(label), PropertyDrawers);
		}

		#endregion

		#region Static methods

		internal static bool InWondeluxeEditor { get; set; }
		internal static int IndentLevel { get; set; }
		internal static int GroupIndentLevel { get; set; }

		/// <summary>
		/// Get the height for SerializedProperty's field in the Editor.
		/// </summary>
		/// <param name="property">The SerializedProperty to get the height for.</param>
		/// <param name="label">Label to use for the field.</param>
		/// <param name="propertyDrawers">A list of custom PropertyDrawers to use for the field.</param>
		/// <param name="includeChildren">Should the returned height include the height of child properties?</param>
		/// <returns>The height to use for the SerializedProperty's field.</returns>

		internal static float GetHeight(SerializedProperty property, GUIContent label, List<PropertyDrawer> propertyDrawers, bool includeChildren)
		{
			// NOTE includeChildren parameter is ignored for properties with a custom PropertyDrawer!

			if (propertyDrawers == null || propertyDrawers.Count == 0)
			{
				return EditorGUI.GetPropertyHeight(property, label, includeChildren);
			}

			float customPropertyHeight = includeChildren ? EditorGUI.GetPropertyHeight(property, label, includeChildren) : EditorGUIUtility.singleLineHeight;
			float additionalPropertyHeight = 0f;

			bool hasCustomLayout = false;

			foreach (PropertyDrawer propertyDrawer in propertyDrawers)
			{
				if (propertyDrawer is WondeluxePropertyDrawer wondeluxePropertyDrawer)
				{
					if (wondeluxePropertyDrawer.HasCustomLayout)
					{
						if (hasCustomLayout)
						{
							throw new Exception($"Multiple WondeluxePropertyDrawers modify property ({property.propertyPath}) height.");
						}

						hasCustomLayout = true;
						customPropertyHeight = wondeluxePropertyDrawer.GetCustomPropertyHeight(property, label);
					}

					if (wondeluxePropertyDrawer.HasAdditionalLayout)
					{
						additionalPropertyHeight += wondeluxePropertyDrawer.GetAdditionalPropertyHeight(property, label);
					}

					continue;
				}

				if (hasCustomLayout)
				{
					throw new Exception($"PropertyDrawer combined with WondeluxePropertyDrawer that modifies layout (property: {property.propertyPath}).");
				}

				customPropertyHeight = propertyDrawer.GetPropertyHeight(property, label);
			}

			return customPropertyHeight + additionalPropertyHeight;
		}

		/// <summary>
		/// Draws a field for a SerializedProperty in the Editor, respecting the custom PropertyDrawers of the property.
		/// </summary>
		/// <param name="position">Rectangle on the screen to use for the field.</param>
		/// <param name="property">The SerializedProperty to make a field for.</param>

		internal static void DoGUI(Rect position, SerializedProperty property)
		{
			DoGUI(position, property, new GUIContent(property.displayName), ScriptAttributeUtilityExtensions.GetDrawers(property.GetFieldInfo()));
		}

		/// <summary>
		/// Draws a field for a SerializedProperty in the Editor.
		/// </summary>
		/// <param name="position">Rectangle on the screen to use for the field.</param>
		/// <param name="property">The SerializedProperty to make a field for.</param>
		/// <param name="propertyDrawers">A list of custom PropertyDrawers to use for the field.</param>

		internal static void DoGUI(Rect position, SerializedProperty property, List<PropertyDrawer> propertyDrawers)
		{
			DoGUI(position, property, new GUIContent(property.displayName), propertyDrawers);
		}

		/// <summary>
		/// Draws a field for a SerializedProperty in the Editor.
		/// </summary>
		/// <param name="position">Rectangle on the screen to use for the field.</param>
		/// <param name="property">The SerializedProperty to make a field for.</param>
		/// <param name="label">Label to use for the field.</param>
		/// <param name="propertyDrawers">A list of custom PropertyDrawers to use for the field.</param>

		internal static void DoGUI(Rect position, SerializedProperty property, GUIContent label, List<PropertyDrawer> propertyDrawers)
		{
			DoGUI(position, property, label, propertyDrawers, true);
		}

		/// <summary>
		/// Draws a field for a SerializedProperty in the Editor.
		/// </summary>
		/// <param name="position">Rectangle on the screen to use for the field.</param>
		/// <param name="property">The SerializedProperty to make a field for.</param>
		/// <param name="label">Label to use for the field.</param>
		/// <param name="propertyDrawers">A list of custom PropertyDrawers to use for the field.</param>
		/// <param name="includeChildren">If <c>true</c> the property including children is drawn; otherwise only the control itself (such as only a foldout but nothing below it).</param>

		internal static void DoGUI(Rect position, SerializedProperty property, GUIContent label, List<PropertyDrawer> propertyDrawers, bool includeChildren)
		{
			//Debug.Log($"DoGUI (position = {position}, property = {property.propertyPath}, label = {label}, depth = {property.depth} (EditorGUI.indentLevel = {EditorGUI.indentLevel}, IndentLevel = {IndentLevel}, IndentOffset = {IndentOffset}), propertyDrawers = PropertyDrawer[{propertyDrawers.Count}], includeChildren = {includeChildren})");

			// ReorderableLists reset EditorGUI.indentLevel to 0, which causes EditorGUIUtility.labelWidth to become out of sync.
			// If this is the case we need to manually adjust labelWidth so that fields align nicely in the inspector.
			// We can check if this is required by checking the property's depth against the current indentLevel.

			int indentLevelOffset = InWondeluxeEditor ? (IndentLevel - GroupIndentLevel + IndentLevel - EditorGUI.indentLevel + property.depth - EditorGUI.indentLevel) : (EditorGUI.indentLevel != property.depth) ? EditorGUI.indentLevel : 0;
			float indentOffset = indentLevelOffset * EditorGUIExtensions.IndentWidth;
			float labelWidth = EditorGUIUtility.labelWidth;

			EditorGUIUtility.labelWidth -= indentOffset;

			bool hasCustomLabel = false;

			Action<Rect, SerializedProperty, GUIContent> customGUI = null;
			float customPropertyHeight = 0f;
			bool hasCustomLayout = false;

			Func<Rect, SerializedProperty, Object> getValidObjectSelection = null;
			bool validateObjectSelection = false;

			foreach (PropertyDrawer propertyDrawer in propertyDrawers)
			{
				if (propertyDrawer is WondeluxePropertyDrawer wondeluxePropertyDrawer)
				{
					if (wondeluxePropertyDrawer.HasCustomLabel)
					{
						if (hasCustomLabel)
						{
							throw new Exception($"Multiple WondeluxePropertyDrawers modify label (property: {property.propertyPath} propertyDrawers: ({string.Join(", ", propertyDrawers)})).");
						}

						label = wondeluxePropertyDrawer.GetCustomLabel(property, label);
						hasCustomLabel = true;
					}

					if (wondeluxePropertyDrawer.HasCustomLayout)
					{
						if (hasCustomLayout)
						{
							throw new Exception($"Multiple WondeluxePropertyDrawers have custom layouts (property: {property.propertyPath}).");
						}

						customGUI = wondeluxePropertyDrawer.OnCustomGUI;
						customPropertyHeight = wondeluxePropertyDrawer.GetCustomPropertyHeight(property, label);
						hasCustomLayout = true;
					}

					if (wondeluxePropertyDrawer.ValidateObjectSelection)
					{
						if (validateObjectSelection)
						{
							throw new Exception($"Multiple WondeluxePropertyDrawers validate object selection (property: {property.propertyPath} propertyDrawers: ({string.Join(", ", propertyDrawers)})).");
						}

						getValidObjectSelection = wondeluxePropertyDrawer.GetValidObjectSelection;
						validateObjectSelection = true;
					}

					continue;
				}

				if (hasCustomLayout)
				{
					throw new Exception($"PropertyDrawer combined with WondeluxePropertyDrawer that modifies layout (property: {property.propertyPath}).");
				}

				customGUI = propertyDrawer.OnGUI;
				customPropertyHeight = propertyDrawer.GetPropertyHeight(property, label);
				hasCustomLayout = true;
			}

			foreach (PropertyDrawer propertyDrawer in propertyDrawers)
			{
				if (propertyDrawer is WondeluxePropertyDrawer wondeluxePropertyDrawer)
				{
					wondeluxePropertyDrawer.OnPreGUI(property, label);
				}
			}

			EditorGUI.BeginChangeCheck();
			EditorGUI.BeginProperty(position, label, property);

			Object validatedObject = validateObjectSelection ? getValidObjectSelection(position, property) : null;

			if (hasCustomLayout)
			{
				Rect rect = new Rect(position.x, position.y, position.width, customPropertyHeight);

				position = new Rect(position.x, position.y + rect.height + EditorGUIUtility.standardVerticalSpacing, position.width, position.height - rect.height - EditorGUIUtility.standardVerticalSpacing);

				//Debug.Log($"customGUI (rect = {rect}, label = {label})");

				customGUI(rect, property, label);
			}
			else if (includeChildren)
			{
				//Debug.Log($"EditorGUIExtensions.DrawPropertyField (rect = {position}, label = {label})");

				EditorGUIExtensions.DrawPropertyField(property, label, true, ref position);
			}
			else
			{
				//Debug.Log($"EditorGUIExtensions.DrawDefaultPropertyField (rect = {position}, label = {label})");

				EditorGUIExtensions.DrawDefaultPropertyField(property, label, ref position);
			}

			if (validatedObject != null)
			{
				property.objectReferenceValue = validatedObject;
			}

			EditorGUI.EndProperty();

			bool valueChanged = EditorGUI.EndChangeCheck();

			foreach (PropertyDrawer propertyDrawer in propertyDrawers)
			{
				if (propertyDrawer is WondeluxePropertyDrawer wondeluxePropertyDrawer && wondeluxePropertyDrawer.HasAdditionalLayout)
				{
					float additionalPropertyHeight = wondeluxePropertyDrawer.GetAdditionalPropertyHeight(property, label);

					Rect rect = new Rect(position.x, position.y, position.width, additionalPropertyHeight);

					wondeluxePropertyDrawer.OnAdditionalGUI(position, property, label);
					position = new Rect(position.x, position.y + rect.height + EditorGUIUtility.standardVerticalSpacing, position.width, position.height - rect.height - EditorGUIUtility.standardVerticalSpacing);
				}
			}

			foreach (PropertyDrawer propertyDrawer in propertyDrawers)
			{
				if (propertyDrawer is WondeluxePropertyDrawer wondeluxePropertyDrawer)
				{
					wondeluxePropertyDrawer.OnPostGUI(property, label, valueChanged);
				}
			}

			EditorGUIUtility.labelWidth = labelWidth;
		}

		#endregion
	}
}