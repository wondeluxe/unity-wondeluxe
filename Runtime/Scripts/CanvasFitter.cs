using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// An alternative component to CanvasScaler that controls the scale and pixel density of a Screen Space Canvas and its children.
	/// The CanvasFitter componenet only works for canvases that render in Screen Space, and scales the canvas so that the reference
	/// resolution always fits inside the screen space area.
	/// </summary>

	[ExecuteAlways]
	public class CanvasFitter : MonoBehaviour
	{
		#region Internal fields

		[SerializeField]
		private Canvas canvas;

		[SerializeField]
		[Tooltip("The resolution the UI layout is designed for. If the aspect ratio of the screen is wider than the aspect ratio of the reference resolution, the UI will be scaled so that the height fits the screen, and if the aspect ratio of the screen is taller, the UI will be scaled so that the width fits the screen.")]
		private ScreenResolution referenceResolution = new ScreenResolution(1920, 1080);

		[SerializeField]
		[Tooltip("If a sprite has this 'Pixels Per Unit' setting, then one pixel in the sprite will cover one unit in the UI.")]
		private int referencePixelsPerUnit = 100;

		#endregion

		#region Public API

		/// <summary>
		/// The Canvas to scale.
		/// </summary>

		public Canvas Canvas
		{
			get => canvas;
		}

		/// <summary>
		/// The resolution the UI layout is designed for. If the aspect ratio of the screen is wider than the aspect ratio of the reference resolution, the UI will be
		/// scaled so that the height fits the screen, and if the aspect ratio of the screen is taller, the UI will be scaled so that the width fits the screen.
		/// </summary>

		public ScreenResolution ReferenceResolution
		{
			get => referenceResolution;
			set
			{
				referenceResolution = value;
				Refit();
			}
		}

		/// <summary>
		/// If a sprite has this 'Pixels Per Unit' setting, then one pixel in the sprite will cover one unit in the UI.
		/// </summary>

		public int ReferencePixelsPerUnit
		{
			get => referencePixelsPerUnit;
			set
			{
				referencePixelsPerUnit = value;
				Refit();
			}
		}

		/// <summary>
		/// Scales the canvas to fit the screen.
		/// </summary>

		public void Refit()
		{
			if (canvas != null && canvas.isRootCanvas && canvas.renderMode != RenderMode.WorldSpace)
			{
				Vector2 canvasSize = canvas.pixelRect.size;
				float canvasAspectRatio = canvasSize.x / canvasSize.y;

				if (canvasAspectRatio > referenceResolution.AspectRatio)
				{
					// Canvas is wider, scale to height.
					canvas.scaleFactor = canvasSize.y / referenceResolution.Height;
				}
				else
				{
					// Canvas is taller, scale to width.
					canvas.scaleFactor = canvasSize.x / referenceResolution.Width;
				}

				canvas.referencePixelsPerUnit = referencePixelsPerUnit;
			}
		}

		#endregion

		#region Unity messages

		private void OnEnable()
		{
			Refit();
		}

		private void Update()
		{
			Refit();
		}

		private void Reset()
		{
			canvas = GetComponent<Canvas>();
		}

		#endregion
	}
}