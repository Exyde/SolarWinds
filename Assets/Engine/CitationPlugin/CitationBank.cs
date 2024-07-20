using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Engine.CitationPlugin
{
    public static class CitationBank
    {
        private static List<CitationData> _citations = new();
        private static HashSet<CitationData> _displayedCitations = new();

        
        
        public static bool LoadAll()
        {
            _citations = Resources.LoadAll<CitationData>("").ToList();

            if (_citations.Count == 0)
            {
                Debug.LogError($"[CitationBank]: Failed to load citations !");
                return false;
            }

            return true;
        }
        
        public static CitationData GetRandomCitation()
        {
            if (_displayedCitations.Count == _citations.Count)
            {
                _displayedCitations.Clear();
            }
            
            var pickedCitation =  _citations[Random.Range(0, _citations.Count)];

            int i = 0;
            int maxIterations = 100;
            
            while (!_displayedCitations.Add(pickedCitation) && i < maxIterations)
            {
                pickedCitation = _citations[Random.Range(0, _citations.Count)];
                i++;
            }

            if (i == maxIterations)
            {
                Debug.LogError("Returned already displayed citation, something sucks in your algo");
            }

            return pickedCitation;
        }
    }
}