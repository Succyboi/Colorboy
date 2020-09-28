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

		[CustomEditor(typeof(ColorBoy))]
		public class ColorBoyEditor : Editor
        {
			ColorBoy colorBoy;

			Texture2D bayer2x2Texture;
			Texture2D bayer3x3Texture;
			Texture2D bayer4x4Texture;
			Texture2D bayer8x8Texture;
			Texture2D bnoise64x64Texture;
			string[] ditherOptions = { "Bayer 2x2", "Bayer 4x4", "Bayer 8x8", "Blue noise 64x64" };

			BuiltInPalettes builtInPalettes = new BuiltInPalettes();
			int currentPalette = 0;

			Texture2D bannerTexture;
			private int labelSize = 6;

            private void OnEnable()
            {
				colorBoy = ((MonoBehaviour)target).gameObject.GetComponent<ColorBoy>();

				bannerTexture = new Texture2D(0, 0);
				bannerTexture.LoadImage(System.IO.File.ReadAllBytes(Application.dataPath + "/Stupid/Generic/Icons/Banner.png"));


				bayer2x2Texture = new Texture2D(0, 0);
				bayer2x2Texture.LoadImage(System.IO.File.ReadAllBytes(Application.dataPath + "/Stupid/Image Effects/ColorBoy/Textures/Bayer2x2.png"));

				bayer3x3Texture = new Texture2D(0, 0);
				bayer3x3Texture.LoadImage(System.IO.File.ReadAllBytes(Application.dataPath + "/Stupid/Image Effects/ColorBoy/Textures/Bayer3x3.png"));

				bayer4x4Texture = new Texture2D(0, 0);
				bayer4x4Texture.LoadImage(System.IO.File.ReadAllBytes(Application.dataPath + "/Stupid/Image Effects/ColorBoy/Textures/Bayer4x4.png"));

				bayer8x8Texture = new Texture2D(0, 0);
				bayer8x8Texture.LoadImage(System.IO.File.ReadAllBytes(Application.dataPath + "/Stupid/Image Effects/ColorBoy/Textures/Bayer8x8.png"));

				bnoise64x64Texture = new Texture2D(0, 0);
				bnoise64x64Texture.LoadImage(System.IO.File.ReadAllBytes(Application.dataPath + "/Stupid/Image Effects/ColorBoy/Textures/BlueNoise64.png"));

                if (colorBoy.ditherTexture == null)
                {
					colorBoy.ditherTexture = bayer4x4Texture;
				}
                else
                {
					//could not use a switch case for this, don't look at me like that
					if (colorBoy.ditherTexture == bayer2x2Texture)
                    {
						colorBoy.currentTexture = 0;
                    }
                    else if (colorBoy.ditherTexture == bayer3x3Texture)
                    {
						colorBoy.currentTexture = 1;
					}
					else if (colorBoy.ditherTexture == bayer4x4Texture)
					{
						colorBoy.currentTexture = 2;
					}
					else if (colorBoy.ditherTexture == bayer8x8Texture)
					{
						colorBoy.currentTexture = 3;
					}
					else if (colorBoy.ditherTexture == bnoise64x64Texture)
					{
						colorBoy.currentTexture = 4;
					}
				}


				//set palette if just added component
				if (colorBoy.paletteName == "" || colorBoy.paletteName == null)
				{
					currentPalette = Random.Range(0, builtInPalettes.palettes.Length);

					colorBoy.colorCount = builtInPalettes.palettes[currentPalette].colors.Length;
					colorBoy.paletteColors = builtInPalettes.palettes[currentPalette].GetColors();
					colorBoy.paletteName = builtInPalettes.palettes[currentPalette].name;
				}
                else
                {
					//select the current palette
					for (int p = 0; p < builtInPalettes.palettes.Length; p++)
					{
						if (colorBoy.paletteName == builtInPalettes.palettes[p].name)
						{
							currentPalette = p;
						}
					}
				}
			}

            public override void OnInspectorGUI()
            {
				//if component has been reset
				if (colorBoy.ditherTexture == null)
				{
					colorBoy.ditherTexture = bayer4x4Texture;
				}
				if (colorBoy.paletteName == "" || colorBoy.paletteName == null)
				{
					currentPalette = Random.Range(0, builtInPalettes.palettes.Length);

					colorBoy.colorCount = builtInPalettes.palettes[currentPalette].colors.Length;
					colorBoy.paletteColors = builtInPalettes.palettes[currentPalette].GetColors();
					colorBoy.paletteName = builtInPalettes.palettes[currentPalette].name;
				}



				EditorGUILayout.BeginVertical(EditorStyles.helpBox);

				GUILayout.Label("Dithering", EditorStyles.boldLabel);

				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Dither: ", GUILayout.Width(GUI.skin.label.lineHeight * labelSize));
				colorBoy.dither = EditorGUILayout.Toggle(colorBoy.dither);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Scale: ", GUILayout.Width(GUI.skin.label.lineHeight * labelSize));
				colorBoy.ditherScale = EditorGUILayout.Slider(colorBoy.ditherScale, 1, 8);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Power: ", GUILayout.Width(GUI.skin.label.lineHeight * labelSize));
				colorBoy.ditherPower = EditorGUILayout.IntSlider(colorBoy.ditherPower, 256, 16);
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Current dither:", GUILayout.Width(GUI.skin.label.lineHeight * labelSize));
				int newTexture = EditorGUILayout.Popup(colorBoy.currentTexture, ditherOptions);

				if (newTexture != colorBoy.currentTexture)
				{
					colorBoy.currentTexture = newTexture;
					
					//pick the new texture
					if (colorBoy.currentTexture == 0)
                    {
						colorBoy.ditherTexture = bayer2x2Texture;
					}
                    else if (colorBoy.currentTexture == 1)
                    {
						colorBoy.ditherTexture = bayer3x3Texture;
					}
					else if (colorBoy.currentTexture == 2)
					{
						colorBoy.ditherTexture = bayer4x4Texture;
					}
					else if (colorBoy.currentTexture == 3)
					{
						colorBoy.ditherTexture = bayer8x8Texture;
					}
					else if (colorBoy.currentTexture == 4)
					{
						colorBoy.ditherTexture = bnoise64x64Texture;
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.EndVertical();



				EditorGUILayout.BeginVertical(EditorStyles.helpBox);

				//get all the option names
				string[] options = new string[builtInPalettes.palettes.Length];
				for (int o = 0; o < options.Length; o++)
                {
					options[o] = builtInPalettes.palettes[o].name;
                }

				GUILayout.Label("Select preset:", EditorStyles.boldLabel);
				int newPalette = EditorGUILayout.Popup(currentPalette, options);

				if(newPalette != currentPalette)
                {
					currentPalette = newPalette;

					colorBoy.colorCount = builtInPalettes.palettes[currentPalette].colors.Length;
					colorBoy.paletteColors = builtInPalettes.palettes[currentPalette].GetColors();
					colorBoy.paletteName = builtInPalettes.palettes[currentPalette].name;
				}

				EditorGUILayout.EndVertical();



				EditorGUILayout.BeginVertical(EditorStyles.helpBox);

				EditorGUILayout.BeginHorizontal();
				for (int o = 0; o < colorBoy.colorCount; o++)
				{
					if(o % 4 == 0)
                    {
						//new line
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.BeginHorizontal();
					}

					EditorGUILayout.ColorField(new GUIContent(""), colorBoy.paletteColors[o], false, false, false, GUILayout.MinWidth(0));
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("+"))
                {
					AddColor();
                }
				if (GUILayout.Button("-"))
				{
					RemoveColor();
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.EndVertical();



				MarkDirty();

				//watermark
				if (bannerTexture != null)
				{
					GUILayout.Label(bannerTexture);
				}
			}

			public void AddColor()
            {
				colorBoy.colorCount = Mathf.Clamp(colorBoy.colorCount + 1, 0, colorBoy.paletteColors.Length);
            }

			public void RemoveColor()
            {
				colorBoy.colorCount = Mathf.Clamp(colorBoy.colorCount - 1, 0, colorBoy.paletteColors.Length);
			}

			public void MarkDirty()
			{
				if (!Application.isPlaying && GUI.changed)
				{
					Undo.RecordObject(this, this.name);
					EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
				}
			}
		}

        #endregion
#endif

        [ExecuteAlways, AddComponentMenu("Image Effects/ColorBoy")]
		public class ColorBoy : MonoBehaviour
		{
			//dithering
			public bool dither = true;
			public Texture2D ditherTexture;
			public float ditherScale = 1;
			public int ditherPower = 16;
			public int currentTexture = 0;

			private Material ditherMat;

			//palettes
			public string paletteName;
			public Color[] paletteColors = new Color[256];
			public int colorCount;

			private Material colorMat;

			private void OnEnable()
			{
				colorMat = new Material(Shader.Find("Hidden/ColorBoy"));
				ditherMat = new Material(Shader.Find("Hidden/DitherBoy"));
			}

			private void OnRenderImage(RenderTexture source, RenderTexture destination)
			{
				RenderTexture dithered = new RenderTexture(source);

				//dithering
				if (ditherMat && dither)
                {
					ditherMat.SetTexture("_DitherTex", ditherTexture);
					ditherMat.SetFloat("_Scale", ditherScale);
					ditherMat.SetFloat("_Power", ditherPower);

					Graphics.Blit(source, dithered, ditherMat);
				}
				else
				{
					Graphics.Blit(source, dithered);
				}

				//palettes
				if (colorMat && paletteColors.Length > 0)
				{
					colorMat.SetInt("_ColorCount", colorCount);
					colorMat.SetColorArray("_Colors", paletteColors);

					Graphics.Blit(dithered, destination, colorMat);
				}
				else
				{
					Graphics.Blit(source, destination);
				}

				DestroyImmediate(dithered);
			}

			void OnDestroy()
			{
				DestroyImmediate(colorMat);
				DestroyImmediate(ditherMat);
			}
		}

		[System.Serializable]
		public class Palette
        {
			public string name;
			public string[] colors;

			public Palette(string name, string[] colors)
			{
				this.name = name;
				this.colors = colors;
			}

			public Color[] GetColors()
            {
				Color[] colors = new Color[256];

				//convert the hex colors to regular color
				for (int c = 0; c < this.colors.Length; c++)
				{
					if (!ColorUtility.TryParseHtmlString(this.colors[c].ToUpper(), out colors[c]))
					{
						Debug.LogWarning("Some hex color codes in this palette could not be converted.");
					}
				}

				return colors;
			}
		}
	}
}