using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Engine.CitationPlugin
{
    public static class CitationBank
    {
        public static List<CitationData> LoadAll()
        {
            var citations = Resources.LoadAll<CitationData>("");

            if (citations.Length == 0)
            {
                Debug.LogError($"[CitationBank]: Failed to load citations !");
            }
            
            return citations.ToList();
        }
    }
}