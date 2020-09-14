//Based on script from: https://github.com/oxysoft/RetroSuite3D
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stupid
{
    namespace ImageEffects
    {
#if UNITY_EDITOR
		#region Custom inspector
		using UnityEditor;
		using UnityEditor.SceneManagement;

		[CustomEditor(typeof(BlurryBoy))]
		public class BlurryBoyEditor : Editor
		{
			BlurryBoy blurryBoy;

			Texture2D bannerTexture;

			BuiltInResolutions builtInResolutions = new BuiltInResolutions();
			int currentResolution;
			Texture2D consoleTexture;

			private int labelSize = 6;

			private void OnEnable()
			{
				blurryBoy = ((MonoBehaviour)target).gameObject.GetComponent<BlurryBoy>();

				bannerTexture = new Texture2D(0, 0);
				bannerTexture.LoadImage(System.IO.File.ReadAllBytes(Application.dataPath + "/Stupid/Generic/Icons/Banner.png"));

				//set palette if just added component
				if (blurryBoy.resolutionName == "" || blurryBoy.resolutionName == null)
				{
					currentResolution = Random.Range(0, builtInResolutions.resolutions.Length);

					blurryBoy.resolutionName = builtInResolutions.resolutions[currentResolution].name;
					blurryBoy.horizontalResolution = builtInResolutions.resolutions[currentResolution].horizontalResolution;
					blurryBoy.verticalResolution = builtInResolutions.resolutions[currentResolution].verticalResolution;
				}
				else
				{
					//select the current palette
					for (int p = 0; p < builtInResolutions.resolutions.Length; p++)
					{
						if (blurryBoy.resolutionName == builtInResolutions.resolutions[p].name)
						{
							currentResolution = p;
						}
					}
				}

				//get game icon
				consoleTexture = new Texture2D(0, 0);
				consoleTexture.LoadImage(System.IO.File.ReadAllBytes(Application.dataPath + "/Stupid/Image Effects/BlurryBoy/Textures/" + builtInResolutions.resolutions[currentResolution].iconName + ".png"));
			}

            public override void OnInspectorGUI()
            {
				//set palette if component reset
				if (blurryBoy.resolutionName == "" || blurryBoy.resolutionName == null)
				{
					currentResolution = Random.Range(0, builtInResolutions.resolutions.Length);

					blurryBoy.resolutionName = builtInResolutions.resolutions[currentResolution].name;
					blurryBoy.horizontalResolution = builtInResolutions.resolutions[currentResolution].horizontalResolution;
					blurryBoy.verticalResolution = builtInResolutions.resolutions[currentResolution].verticalResolution;

					//get game icon
					consoleTexture = new Texture2D(0, 0);
					consoleTexture.LoadImage(System.IO.File.ReadAllBytes(Application.dataPath + "/Stupid/Image Effects/BlurryBoy/Textures/" + builtInResolutions.resolutions[currentResolution].iconName + ".png"));
				}



				EditorGUILayout.BeginVertical(EditorStyles.helpBox);

				//get all the option names
				string[] options = new string[builtInResolutions.resolutions.Length];
				for (int o = 0; o < options.Length; o++)
				{
					options[o] = builtInResolutions.resolutions[o].name;
				}

				GUILayout.Label("Current resolution:", EditorStyles.boldLabel);
				int newPalette = EditorGUILayout.Popup(currentResolution, options);

				if (newPalette != currentResolution)
				{
					currentResolution = newPalette;

					blurryBoy.resolutionName = builtInResolutions.resolutions[currentResolution].name;
					blurryBoy.horizontalResolution = builtInResolutions.resolutions[currentResolution].horizontalResolution;
					blurryBoy.verticalResolution = builtInResolutions.resolutions[currentResolution].verticalResolution;

					//get game icon
					consoleTexture = new Texture2D(0, 0);
					consoleTexture.LoadImage(System.IO.File.ReadAllBytes(Application.dataPath + "/Stupid/Image Effects/BlurryBoy/Textures/" + builtInResolutions.resolutions[currentResolution].iconName + ".png"));
				}

				EditorGUILayout.EndVertical();



				EditorGUILayout.BeginVertical(EditorStyles.helpBox);

				EditorGUILayout.BeginHorizontal();
				GUILayout.Label(consoleTexture);

				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				GUILayout.Label("Resolution:", EditorStyles.boldLabel);

				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("X: ", GUILayout.Width(GUI.skin.label.lineHeight * 2));
				blurryBoy.horizontalResolution = EditorGUILayout.IntField(blurryBoy.horizontalResolution);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Y: ", GUILayout.Width(GUI.skin.label.lineHeight * 2));
				blurryBoy.verticalResolution = EditorGUILayout.IntField(blurryBoy.verticalResolution);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.EndVertical();
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.EndVertical();



				MarkDirty();

				//watermark
				if (bannerTexture != null)
				{
					GUILayout.Label(bannerTexture);
				}
			}

			public void MarkDirty()
			{
				if (!Application.isPlaying && GUI.changed)
				{
					EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
				}
			}
		}

		#endregion
#endif

		[ExecuteAlways, AddComponentMenu("Image Effects/Blurry Boy")]
		public class BlurryBoy : MonoBehaviour
        {
			public string resolutionName;
            public int horizontalResolution;
            public int verticalResolution;

			public void OnRenderImage(RenderTexture src, RenderTexture dest)
			{
				horizontalResolution = Mathf.Clamp(horizontalResolution, 1, 2048);
				verticalResolution = Mathf.Clamp(verticalResolution, 1, 2048);

				RenderTexture scaled = RenderTexture.GetTemporary(horizontalResolution, verticalResolution);
				scaled.filterMode = FilterMode.Point;
				Graphics.Blit(src, scaled);
				Graphics.Blit(scaled, dest);
				RenderTexture.ReleaseTemporary(scaled);
			}
		}

		[System.Serializable]
		public class Resolution
        {
			public string name;
			public string iconName;
			public int horizontalResolution, verticalResolution;

			public Resolution(string name, string iconName, int horizontalResolution, int verticalResolution)
            {
				this.name = name;
				this.iconName = iconName;
				this.horizontalResolution = horizontalResolution;
				this.verticalResolution = verticalResolution;
            }
		}
    }
}
