using System;
using UnityEngine;

namespace Wondeluxe
{
	/// <summary>
	/// Interface for managing time.
	/// </summary>
	/// <remarks>
	/// TimeManager is a designed as a singleton component to be added once to the heirarchy of a Unity application.
	/// It should be added to the Script Execution Order with a negative value, so that it executes before default time, so that it is updated before other behaviours in the application.
	/// As well as providing methods for modifying time in the application, it also provides events as a single point of reference for FixedUpdate, Update and LateUpdate messages, that can be
	/// listened to instead of having many objects registering for those messages independently (as a way to improve performance).
	/// </remarks>

	public class TimeManager : MonoBehaviour
	{
		#region Static fields

		private static TimeManager instance;

		#endregion

		#region Static API

		/// <summary>
		/// Dispatched on FixedUpdate.
		/// </summary>

		public static event Action OnFixedUpdate;

		/// <summary>
		/// Dispatched on Update.
		/// </summary>

		public static event Action OnUpdate;

		/// <summary>
		/// Dispatched on LateUpdate.
		/// </summary>

		public static event Action OnLateUpdate;

		/// <summary>
		/// The current FixedUpdate frame. This value is updated at the start of each FixedUpdate cycle.
		/// Will roll over to 0 after max value is reached.
		/// </summary>

		public static ulong CurrentFixedUpdateFrame => instance.currentFixedUpdateFrame;

		/// <summary>
		/// The current Update frame. This value is updated at the start of each Update cycle.
		/// Will roll over to 0 after max value is reached.
		/// </summary>

		public static ulong CurrentUpdateFrame => instance.currentUpdateFrame;

		/// <summary>
		/// Sets Time.timeScale and modifies Time.fixedDeltaTime to match.
		/// Using this method will modify the speed of time, but FixedUpdate will be called at the same rate.
		/// See <a href="https://docs.unity3d.com/ScriptReference/Time-timeScale.html">Time.timeScale</a>.
		/// </summary>
		/// <param name="timeScale">The scale at which time passes.</param>

		public static void SetTimeScale(float timeScale)
		{
			Time.timeScale = timeScale;
			Time.fixedDeltaTime = instance.defaultFixedDeltaTime * timeScale;
		}

		/// <summary>
		/// Resets Time.timeScale and Time.fixedDeltaTime to their default values.
		/// </summary>

		public static void ResetTimeScale()
		{
			Time.timeScale = instance.defaultTimeScale;
			Time.fixedDeltaTime = instance.defaultFixedDeltaTime;
		}

		/// <summary>
		/// The default value of Time.timeScale.
		/// </summary>

		public static float DefaultTimeScale => instance.defaultTimeScale;

		/// <summary>
		/// The default value of Time.fixedDeltaTime.
		/// </summary>

		public static float DefaultFixedDeltaTime => instance.defaultFixedDeltaTime;

		#endregion

		#region Internal fields

		private ulong currentFixedUpdateFrame;
		private ulong currentUpdateFrame;

		private float defaultTimeScale;
		private float defaultFixedDeltaTime;

		#endregion

		#region Unity messages

		private void Awake()
		{
			if (instance == null)
			{
				instance = this;
				defaultTimeScale = Time.timeScale;
				defaultFixedDeltaTime = Time.fixedDeltaTime;
			}
			else
			{
				Debug.LogWarning($"Multiple instances of TimeManager found. Additional instance '{name}' will be destroyed.");
				enabled = false;
				Destroy(this);
			}
		}

		private void FixedUpdate()
		{
			currentFixedUpdateFrame++;
			OnFixedUpdate?.Invoke();
		}

		private void Update()
		{
			currentUpdateFrame++;
			OnUpdate?.Invoke();
		}

		private void LateUpdate()
		{
			OnLateUpdate?.Invoke();
		}

		#endregion
	}
}