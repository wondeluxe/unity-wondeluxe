using UnityEngine;
using UnityEditor;

namespace WondeluxeEditor
{
	public abstract class ObjectPropertyDrawer<TObject> : WondeluxePropertyDrawer where TObject : Object
	{
		public override sealed bool ValidateObjectSelection => true;

		public override sealed Object GetValidObjectSelection(Rect position, SerializedProperty property)
		{
			EventType eventType = Event.current.type;
			string commandName = Event.current.commandName;
			int objectPickerControlID = EditorGUIUtility.GetObjectPickerControlID();

			if (commandName == "ObjectSelectorSelectionDone")
			{
				if (objectPickerControlID == ObjectPickerControlID)
				{
					Object pickedObjectValue = GetValidValue(EditorGUIUtility.GetObjectPickerObject());

					if (pickedObjectValue != null)
					{
						return pickedObjectValue;
					}
				}
			}
			else if (eventType == EventType.MouseDown)
			{
				Rect pickerButtonArea = EditorGUIExtensions.ObjectPickerButtonRect(position);

				if (pickerButtonArea.Contains(Event.current.mousePosition) && objectPickerControlID == 0)
				{
					EditorGUIUtility.ShowObjectPicker<TObject>(property.objectReferenceValue, true, GetObjectPickerFilter(), ObjectPickerControlID);
					Event.current.Use();
				}
			}
			else if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
			{
				if (position.Contains(Event.current.mousePosition))
				{
					Object draggedObjectValue = GetValidValue(DragAndDrop.objectReferences);

					if (draggedObjectValue == null)
					{
						DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
						Event.current.Use();
					}
					else if (eventType == EventType.DragPerform)
					{
						return draggedObjectValue;
					}
				}
			}

			return null;
		}

		//public override sealed void OnCustomGUI(Rect position, SerializedProperty property, GUIContent label)
		//{
		//	EventType eventType = Event.current.type;
		//	string commandName = Event.current.commandName;
		//	int objectPickerControlID = EditorGUIUtility.GetObjectPickerControlID();

		//	Object propertyValue = null;

		//	if (commandName == "ObjectSelectorSelectionDone")
		//	{
		//		if (objectPickerControlID == ObjectPickerControlID)
		//		{
		//			Object pickedObjectValue = GetValidValue(EditorGUIUtility.GetObjectPickerObject());

		//			if (pickedObjectValue != null)
		//			{
		//				propertyValue = pickedObjectValue;
		//			}
		//		}
		//	}
		//	else if (eventType == EventType.MouseDown)
		//	{
		//		Rect pickerButtonArea = EditorGUIExtensions.ObjectPickerButtonRect(position);

		//		if (pickerButtonArea.Contains(Event.current.mousePosition) && objectPickerControlID == 0)
		//		{
		//			EditorGUIUtility.ShowObjectPicker<TObject>(property.objectReferenceValue, true, GetObjectPickerFilter(), ObjectPickerControlID);
		//			Event.current.Use();
		//		}
		//	}
		//	else if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
		//	{
		//		if (position.Contains(Event.current.mousePosition))
		//		{
		//			Object objectReferenceValue = GetValidValue(DragAndDrop.objectReferences);

		//			if (objectReferenceValue == null)
		//			{
		//				DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
		//				Event.current.Use();
		//			}
		//			else if (eventType == EventType.DragPerform)
		//			{
		//				propertyValue = objectReferenceValue;
		//			}
		//		}
		//	}

		//	// BeginProperty/EndProperty calls not required.
		//	// Called in WondeluxePropertyDrawer before/after OnCustomGUI.

		//	//EditorGUI.BeginProperty(position, label, property);
		//	EditorGUI.PropertyField(position, property, label, true);

		//	if (propertyValue != null)
		//	{
		//		property.objectReferenceValue = propertyValue;
		//	}

		//	//EditorGUI.EndProperty();
		//}

		protected abstract Object GetValidValue(params Object[] objectReferences);

		protected abstract string GetObjectPickerFilter();
	}
}