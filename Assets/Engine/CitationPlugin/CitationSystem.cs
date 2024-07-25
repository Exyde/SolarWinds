using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Engine.CitationPlugin
{
    public class CitationSystem : MonoBehaviour
    {
        public static CitationSystem Instance;

        [FormerlySerializedAs("_displayRandomCitation")] [Header("Settings")] [SerializeField] private bool _displayRandomCitationAtStart;
        [SerializeField] private bool _displayRandomCitationOnJump; 
        [SerializeField] private float _inDisplayDuration;
        [SerializeField] private float _outDisplayDuration;
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _author;
        [SerializeField] private TMP_Text _citation;
        
        private Sequence _citationSequence;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            CitationBank.LoadAll();
            
            if (_displayRandomCitationAtStart) DisplayRandomCitation(_inDisplayDuration, _outDisplayDuration);
        }

        private void OnEnable()
        {
            //That make it not plugin anymore :[[[
            if (_displayRandomCitationOnJump) PlayerController.OnJump += DisplayRandomCitation;
        }

        private void OnDisable()
        {
            //That make it not plugin anymore :[[[
            if (_displayRandomCitationOnJump) PlayerController.OnJump -= DisplayRandomCitation;
        }

        public void DisplayRandomCitation() => DisplayRandomCitation(_inDisplayDuration, _outDisplayDuration);
        private void DisplayRandomCitation( float inDuration, float outDuration)
        {
            Hide(0);
            
            _citationSequence?.Kill(false);
            _citationSequence = DOTween.Sequence();
            
            CitationData citationData = CitationBank.GetRandomCitation();
            _author.text = citationData.Author;
            _citation.text = citationData.Citation;
            
            _citationSequence.Append(_canvasGroup.DOFade(1, inDuration));
            _citationSequence.AppendInterval(2.5f);
            _citationSequence.AppendCallback(() => Hide(outDuration));
        }

        private void Hide(float duration = 0)
        {
            _canvasGroup.DOFade(0, duration);
        }
    }
}