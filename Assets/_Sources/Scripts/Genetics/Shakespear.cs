using System;
using EnhancedEditor;
using UnityEngine;

namespace _Sources.Scripts.Genetics
{
    public class Shakespear : MonoBehaviour
    {
        public int wordLength;

        [Button(ActivationMode.Always)]
        public void LogRandomString()
        {
            Debug.Log(StringExtension.GetRandomString(wordLength));
        }
    }
}