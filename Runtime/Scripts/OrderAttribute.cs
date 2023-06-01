using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Changes the order that fields are displayed in the Inspector.
	/// </summary>

	public class OrderAttribute : PropertyAttribute
	{
		/// <summary>
		/// Value used to override the display order. Negative values cause the field to appear before the first field,
		/// positive values cause the field to appear after the last field.
		/// </summary>

		public int PropertyOrder { get; private set; }

		/// <summary>
		/// Changes the order that fields are displayed in the Inspector.
		/// </summary>
		/// <param name="propertyOrder">Value used to override the display order.</param>

		public OrderAttribute(int propertyOrder)
		{
			PropertyOrder = propertyOrder;
		}
	}
}