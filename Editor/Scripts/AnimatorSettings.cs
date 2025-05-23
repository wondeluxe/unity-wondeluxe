using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using Wondeluxe;

using AnimatorController = UnityEditor.Animations.AnimatorController;

namespace WondeluxeEditor
{
	[FilePath("ProjectSettings/AnimatorSettings.asset", FilePathAttribute.Location.ProjectFolder)]
	public class AnimatorSettings : ScriptableSingleton<AnimatorSettings>
	{
		[SerializeField]
		private AnimatorControllerConfig[] animatorControllerConfigs;

		internal void UpdateAnimatorControllers()
		{
			if (animatorControllerConfigs == null)
			{
				return;
			}

			Dictionary<TextAsset, AnimatorController> updatedAssets = new();

			foreach (AnimatorControllerConfig config in animatorControllerConfigs)
			{
				if (config.HasValues)
				{
					if (updatedAssets.TryGetValue(config.Script, out AnimatorController sourceController))
					{
						// Newly added config elements in the inspector will initialise to the last config in the list,
						// therefore we'll silently ignore configs with identical values. Warn if a script has multiple
						// source animator controllers.

						if (config.AnimatorController != sourceController)
						{
							Debug.LogWarning($"Script ({config.Script.name}) already updated by another animator controller ({sourceController.name}).");
						}

						continue;
					}

					updatedAssets.Add(config.Script, config.AnimatorController);

					UpdateAnimatorControllerScript(config);

					ProjectModificationProcessor.OnAssetSaved.Add(AssetDatabase.GetAssetPath(config.AnimatorController), UpdateAnimatorController);
				}
			}
		}

		internal void UpdateAnimatorController(string animatorControllerPath)
		{
			if (animatorControllerConfigs == null)
			{
				return;
			}

			AnimatorController animatorController = AssetDatabase.LoadAssetAtPath<AnimatorController>(animatorControllerPath);

			bool animatorControllerHasConfig = false;

			foreach (AnimatorControllerConfig config in animatorControllerConfigs)
			{
				if (config.AnimatorController == animatorController && config.Script != null)
				{
					UpdateAnimatorControllerScript(config);
					animatorControllerHasConfig = true;
				}
			}

			if (!animatorControllerHasConfig)
			{
				ProjectModificationProcessor.OnAssetSaved.Remove(animatorControllerPath, UpdateAnimatorController);
			}
		}

		internal void SaveAsset()
		{
			Save(true);
		}

		private void Initialize()
		{
			foreach (AnimatorControllerConfig config in animatorControllerConfigs)
			{
				if (config.HasValues)
				{
					ProjectModificationProcessor.OnAssetSaved.Add(AssetDatabase.GetAssetPath(config.AnimatorController), UpdateAnimatorController);
				}
			}
		}

		internal static void UpdateAnimatorControllerScript(AnimatorControllerConfig config)
		{
			string[] stateDefinitions = GetAnimatorStateDefinitions(config.AnimatorController);

			AssetDatabaseExtensions.InjectMemberValues(config.Script, stateDefinitions);
		}

		private static string[] GetAnimatorStateDefinitions(AnimatorController animatorController)
		{
			Dictionary<string, string> states = new();

			foreach (AnimatorControllerLayer layer in animatorController.layers)
			{
				foreach (ChildAnimatorState state in layer.stateMachine.states)
				{
					string name = state.state.name.ToPascal();

					if (!states.TryAdd(name, $"public const int {name} = {state.state.nameHash};"))
					{
						Debug.LogWarning($"Duplicate state identifier \"{name}\" found on animator controller ({animatorController.name}).");
					}
				}
			}

			string[] stateNamesArray = new string[states.Count];

			states.Values.CopyTo(stateNamesArray, 0);

			return stateNamesArray;
		}

		[Serializable]
		public class AnimatorControllerConfig
		{
			[SerializeField]
			private AnimatorController animatorController;

			[SerializeField]
			private TextAsset script;

			public AnimatorController AnimatorController
			{
				get { return animatorController; }
			}

			public TextAsset Script
			{
				get { return script; }
			}

			public bool HasValues
			{
				get { return animatorController != null && script != null; }
			}
		}

		[InitializeOnLoad]
		private static class Initializer
		{
			static Initializer()
			{
				instance.Initialize();
			}
		}
	}
}