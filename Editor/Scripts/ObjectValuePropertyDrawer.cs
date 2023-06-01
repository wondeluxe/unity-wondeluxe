using UnityEngine;
using UnityEditor;

namespace WondeluxeEditor
{
	/// <summary>
	/// Base class for PropertyDrawers that replace a value field with an Object field.
	/// The OnCustomUI method implemented in ObjectValuePropertyDrawer handles drag and drop and object picker events.
	/// </summary>
	/// <typeparam name="TValue">The property's value type (int, float, string, etc).</typeparam>
	/// <typeparam name="TObject">The type of Object to use as the replacement field (MonoBehaviour, DefaultAsset, etc).</typeparam>

	public abstract class ObjectValuePropertyDrawer<TValue, TObject> : WondeluxePropertyDrawer where TObject : Object
	{
		public override sealed bool HasCustomLayout => true;

		public override sealed void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EventType eventType = Event.current.type;
			string commandName = Event.current.commandName;
			int objectPickerControlID = EditorGUIUtility.GetObjectPickerControlID();

			TValue propertyValue = property.GetValue<TValue>();

			if (commandName == "ObjectSelectorSelectionDone")
			{
				if (objectPickerControlID == ObjectPickerControlID)
				{
					TValue pickedValue = GetValidValue(EditorGUIUtility.GetObjectPickerObject());

					if (pickedValue != null)
					{
						propertyValue = pickedValue;
					}
				}
			}
			else if (eventType == EventType.MouseDown)
			{
				Rect pickerButtonArea = EditorGUIExtensions.ObjectPickerButtonRect(position);

				if (pickerButtonArea.Contains(Event.current.mousePosition) && objectPickerControlID == 0)
				{
					EditorGUIUtility.ShowObjectPicker<TObject>(GetObjectReference(propertyValue), true, GetObjectPickerFilter(), ObjectPickerControlID);
					Event.current.Use();
				}
			}
			else if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
			{
				if (position.Contains(Event.current.mousePosition))
				{
					TValue stringValue = GetValidValue(DragAndDrop.objectReferences);

					if (stringValue == null)
					{
						DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
						Event.current.Use();
					}
					else if (eventType == EventType.DragPerform)
					{
						propertyValue = stringValue;
					}
				}
			}

			// BeginProperty/EndProperty calls not required.
			// Called in WondeluxePropertyDrawer before/after OnCustomGUI.

			Object objectReference = GetObjectReference(propertyValue);
			objectReference = EditorGUI.ObjectField(position, label, objectReference, typeof(TObject), false);

			property.SetValue(GetValidValue(objectReference));
		}

		protected abstract TValue GetValidValue(params Object[] objectReferences);

		protected abstract Object GetObjectReference(TValue value);

		protected abstract string GetObjectPickerFilter();
	}
}