using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Engine.CitationPlugin
{
    public class CitationSystem : MonoBehaviour
    {
        [Header("Settings")] [SerializeField] private bool _displayRandomCitation;
        [SerializeField] private List<CitationData> _citations;
        
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _author;
        [SerializeField] private TMP_Text _citation;

        private HashSet<CitationData> _displayedCitations = new HashSet<CitationData>(); //or something else like a dict count or bias
        public static CitationSystem Instance;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            _citations = CitationBank.LoadAll();
            Debug.Log($"Loaded {_citations.Count} citations.");
            
            DisplayCitation();
        }
        

        public void DisplayCitation()
        {
            Hide(0);
            
            CitationData citationData = _citations[Random.Range(0, _citations.Count)];
            _author.text = citationData.Author;
            _citation.text = citationData.Citation;

            Sequence citationSequence = DOTween.Sequence();

            citationSequence.Append(_canvasGroup.DOFade(1, 1));
            citationSequence.AppendInterval(2.5f);
            citationSequence.AppendCallback(() => Hide(1f));
        }

        public void Hide(float duration = 0)
        {
            _canvasGroup.DOFade(0, duration);
        }
    }
}