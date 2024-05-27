using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using _Sources.Scripts.Utils;
using Companion;
using DG.Tweening;
using Manager;
using QuickOutline;
using UnityEngine;
using Object = UnityEngine.Object;
using Ludogram.CategoryConsole;
using TMPro;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Utils {
	public static class GeneralUtils {
		
		private static GUIStyle _debugStyle;
		private static int _waitUntilCount;
		public static int WaitUntilCount
		{
			get => _waitUntilCount;
			private set
			{
#if UNITY_EDITOR
				Dbg.LogDebug("WaitUntil: " + _waitUntilCount + " => " + value + "\n" + new StackTrace(), "wait");
#endif
				_waitUntilCount = value;
			}
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Stream ToStream(this string str)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(str);
			writer.Flush();
			stream.Position = 0;
			return stream;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AfterXFrames(this MonoBehaviour mono, int frames, Action action) {
			if (mono == null || !mono.isActiveAndEnabled) { // if the object is destroyed or disabled, use ManagerStory (we are sure it will be always active and not destroyed)
				mono = NLoadingScreen.Instance;
			}
			
			mono.StartCoroutine(AfterXFramesCoroutine(frames, action));
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AfterTime(this MonoBehaviour monoBehaviour, float time, Action action) {
			if (monoBehaviour == null || !monoBehaviour.isActiveAndEnabled) {
				monoBehaviour = NLoadingScreen.Instance;
			}

			monoBehaviour.StartCoroutine(AfterTimeCoroutine(time, action));
		}
		
		public static IEnumerator AfterXFramesCoroutine(int frames, Action action) {
			for (var i = 0; i < frames; i++) {
				yield return new WaitForEndOfFrame();
			}
			action();
		}
		private static IEnumerator AfterTimeCoroutine(float time, Action action) {
			if (time > 0)
			{
				yield return new WaitForSeconds(time);
			}
			action();
		}
		
		public static void Display(this CanvasGroup canvasGroup, bool display, float duration = 0.2f, bool activation = true, Action onComplete = null) {
			if (DOTween.IsTweening(canvasGroup))
			{
				DOTween.Kill(canvasGroup);
			}

			if (display && activation) canvasGroup.gameObject.SetActive(true);
			if (display && !activation) canvasGroup.blocksRaycasts = true;

			canvasGroup.DOFade(display ? 1 : 0, duration).OnComplete(() => {
				if (!display && activation) canvasGroup.gameObject.SetActive(false);
				if (!display && !activation) canvasGroup.blocksRaycasts = false;

				if (onComplete != null) onComplete();
			});
		}
		
		public static void WaitUntil(this MonoBehaviour mono, Func<bool> condition, Action onComplete) {
			if (mono == null || !mono.isActiveAndEnabled) { // if the object is destroyed or disabled, use ManagerStory (we are sure it will be always active and not destroyed)
				mono = NLoadingScreen.Instance;
			}

			if (MSettings.Current.wailUntilDelay == 0 && condition()) {
				onComplete();
			} else
			{
				WaitUntilCount++;
				mono.StartCoroutine(WaitUntilCoroutine(condition, onComplete));
			}
		}
		
		private static IEnumerator WaitUntilCoroutine(Func<bool> condition, Action onComplete) {
			while (!condition()) yield return null;
			if (MSettings.Current.wailUntilDelay > 0) yield return new WaitForSeconds(MSettings.Current.wailUntilDelay);
			WaitUntilCount--;
			onComplete();
		}
        
		// Function to upper the first letter of a string
		public static string FirstCharToUpper(this string input) =>
			input switch
			{
				null => throw new ArgumentNullException(nameof(input)),
				"" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
				_ => input[0].ToString().ToUpper() + input.Substring(1)
			};
		
		public static void Shuffle<T>(this IList<T> list, WichmannRng rng)  
		{  
			int n = list.Count;  
			while (n > 1) {  
				n--;  
				int k = rng.Next(n + 1);  
				(list[k], list[n]) = (list[n], list[k]); // Swap elements
			}  
		}
		
		// Adapt debug mode Gui text size
		public static int GetDebugSize()
		{
			var scaleFactor = Screen.width / 1920f;
			return (int)System.Math.Max(14, System.Math.Min(22 * scaleFactor, 32));
		}
		
		public static GUIStyle GetDebugStyle()
		{
			_debugStyle ??= new GUIStyle(GUI.skin.GetStyle("label"))
			{
				fontSize = GeneralUtils.GetDebugSize()
			};

			return _debugStyle;
		}
		
		public static int Count(this byte b)
		{
			int count = 0;
			for (int i = 0; i < 8; i++)
			{
				if (b.Get(i))
				{
					count++;
				}
			}
			return count;
		}
		
		public static byte Set(this byte b, int index, bool value)
		{
			if (value)
			{
				b |= (byte)(1 << index);
			}
			else
			{
				b &= (byte)~(1 << index);
			}
			
			return b;
		}
		
		public static bool Get(this byte b, int index)
		{
			return (b & (1 << index)) != 0;
		}

		public static bool Masked(this byte b, byte mask)
		{
			return (b & mask) > 0;
		}
		
		public static string GetSkillLocalization(string skill)
		{
			// Build skill localisation key
			string key = string.Join("", skill.Replace('_', ' ').Split().AsEnumerable().Select(s => s.FirstCharToUpper()).ToArray()); // Capitalize first letter for localization
			return "UI/Skill_" + key;
		}

		public static void LimitFrameRate()
		{
			// Cap framerate at 60 (at least)
			Application.targetFrameRate = 60;
			// Sync the frame rate to the screen's refresh rate
			QualitySettings.vSyncCount = 1;
		}

		public static uint BinaryFromIndices(ICollection<int> indexes)
		{
			uint result = 0;
			var count = indexes.Count;

			for (var i = 0; i < count; i++)
			{
				result |= 1u << indexes.ElementAt(i);
			}

			return result;
		}
		
		public static string ToLanguageCode(this MPlayerSettings.Settings.Language language)
		{
			return language switch
			{
				MPlayerSettings.Settings.Language.EN => "en",
				MPlayerSettings.Settings.Language.FR => "fr",
				_ => "en"
			};
		}
		
		public static int GetGCD(int num1, int num2)
		{
			while (num1 != num2)
			{
				if (num1 > num2)
					num1 -= num2;
				
				if (num2 > num1)
					num2 -= num1;
			}

			return num1;
		}

		public static (int x, int y) AspectRatioFromResolution(Resolution res)
		{
			var gcd = GetGCD(res.width, res.height);
				
			return (res.width / gcd, res.height / gcd);
		}

		public static string ResolutionToString(this Resolution res)
		{
			return res.width + "x" + res.height;
		}

		public static bool Equal(this Color colorA, Color colorB, bool compareAlpha)
		{
			if (compareAlpha)
			{
				return colorA == colorB;
			}
			else
			{
				return colorA.r == colorB.r && colorA.g == colorB.g && colorA.b == colorB.b;
			}
		}

		public static bool Equal(this Color colorA, Color colorB)
		{
			return colorA.Equal(colorB, true);
		}

		public static bool Equal(this Color? colorA, Color? colorB, bool compareAlpha)
		{
			if (colorA == null || colorB == null)
			{
				return false;
			}
			
			return ((Color) colorA).Equal((Color) colorB, compareAlpha);
		}

		public static void SetAlpha(this Image image, float alpha)
		{
			image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
		}
		
		public static void SetColor(this Image image, Color color, bool setAlpha)
		{
			if (setAlpha)
			{
				image.color = color;
			}
			else
			{
				image.color = new Color(color.r, color.g, color.b, image.color.a);
			}
		}
		
		public static Tween DOColor(this Image image, Color color, float duration, bool setAlpha)
		{
			if (setAlpha)
			{
				return image.DOColor(color, duration);
			}
			else
			{
				return image.DOColor(new Color(color.r, color.g, color.b, image.color.a), duration);
			}
		}
		
		public static float GetWordOffset(TextMeshProUGUI tmp)
		{
			TMP_TextInfo textInfo = tmp.textInfo;
			float wordOffset = 0;

			for (int wordIndex = 0; wordIndex < textInfo.wordCount; wordIndex++)
			{
				TMP_WordInfo wordInfo = textInfo.wordInfo[wordIndex];
				int currentLine = textInfo.characterInfo[wordInfo.firstCharacterIndex].lineNumber;
				
				float tempWordOffset = 0;
				
				for (int charIndex = wordInfo.firstCharacterIndex; charIndex <= wordInfo.lastCharacterIndex; charIndex++)
				{
					TMP_CharacterInfo charInfo = textInfo.characterInfo[charIndex];
					if (charInfo.lineNumber != currentLine)
					{
						float charWidth = charInfo.topRight.x - charInfo.bottomLeft.x;
						tempWordOffset += charWidth;
					}
				}
				
				if (tempWordOffset > wordOffset)
				{
					wordOffset = tempWordOffset;
				}
			}
			
			return wordOffset;
		}
		
		public static void DrawBounds(Bounds b, float delay=0)
		{
			// bottom
			var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
			var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
			var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
			var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

			Gizmos.color = Color.blue;
			Gizmos.DrawLine(p1, p2);
			Gizmos.color = Color.red;
			Gizmos.DrawLine(p2, p3);
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(p3, p4);
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine(p4, p1);

			// top
			var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
			var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
			var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
			var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

			Gizmos.color = Color.blue;
			Gizmos.DrawLine(p5, p6);
			Gizmos.color = Color.red;
			Gizmos.DrawLine(p6, p7);
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(p7, p8);
			Gizmos.color = Color.magenta;
			Gizmos.DrawLine(p8, p5);

			// sides
			Gizmos.color = Color.white;
			Gizmos.DrawLine(p1, p5);
			Gizmos.color = Color.gray;
			Gizmos.DrawLine(p2, p6);
			Gizmos.color = Color.green;
			Gizmos.DrawLine(p3, p7);
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine(p4, p8);
			
		}
		
		public static bool IsChildOf(NavElement child, NavElement parent)
		{
			bool isChild = false;
			NavElement currentElement = child;
			while (currentElement != null && currentElement != parent)
			{
				currentElement = currentElement.parent;
				if (currentElement == parent)
				{
					isChild = true;
					break;
				}
			}
			return isChild;
		}
		
		public static void SetLayer(GameObject _gameObject, int _layer) {

			_gameObject.layer = _layer;

			foreach (Transform _child in _gameObject.transform) {
				SetLayer(_child.gameObject, _layer);
			}
		}
	}
}