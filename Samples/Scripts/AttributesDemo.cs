using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wondeluxe;

namespace Wondeluxe.Samples
{
	public class AttributesDemo : MonoBehaviour
	{
		[SerializeField]
		[ImplicitBool("Empty")]
		private DemoData exampleData;

		[SerializeField]
		[FoldoutGroup("Layer Examples")]
		[Layer]
		private int layerID;

		[SerializeField]
		[FoldoutGroup("Layer Examples")]
		[Layer]
		private string layerName;

		[SerializeField]
		[Group("Flags")]
		[Order(10)]
		private bool isPowerful;

		[SerializeField]
		[LabeledArray(new string[] { "First", "Second", "Third" })]
		private float[] numbers = new float[] { 1f, 2f, 3f };

		[SerializeField]
		[NumberedArray("Inverval", 1)]
		private float[] intervals = new float[] { 3f, 8f, 5f };

		[SerializeField]
		[Group("Flags")]
		private bool isMagical;

		[SerializeField]
		[Group("Flags")]
		private bool isFast;

		[Readonly]
		public Vector3 Direction = Vector3.up;

		[ShowInInspector]
		private string secretName = "Fuzzy";

		[SerializeField]
		[SortingLayer]
		private string exampleLayer;

		[SerializeField]
		[Tag]
		private string exampleTag;

		[SerializeField]
		[OnModified("OnObjectReferenceModified")]
		[Label("AWESOME OBJECT")]
		[Component]
		private Object objectReference;

		[Button]
		private void LogSecretName()
		{
			Debug.Log($"Secret name is '{secretName}'.");
		}

		private void OnObjectReferenceModified()
		{
			if (objectReference != null)
			{
				Debug.Log($"objectReference = {objectReference.name} ({objectReference.GetType().Name})");
			}
			else
			{
				Debug.Log($"objectReference = null");
			}
		}
	}
}