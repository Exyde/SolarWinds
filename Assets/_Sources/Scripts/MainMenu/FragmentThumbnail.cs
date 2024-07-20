using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Sources.Scripts.MainMenu
{
    public class FragmentThumbnail : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private FragmentData _fragmentData;

        [FormerlySerializedAs("_fragmentNameTMP")] [SerializeField] private TMP_Text _fragmentMainTextTMP;
        [SerializeField] private TMP_Text _fragmentDescriptionTMP;

        [SerializeField] private Button _fragmentButton;


        private void Start()
        {
            Init();
        }
        
        public void Init()
        {
            _fragmentMainTextTMP.text = _fragmentData.FragmentName;
            _fragmentDescriptionTMP.text = _fragmentData.Description;
            
        }
        private void OnEnable()
        {
            _fragmentButton.onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            _fragmentButton.onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            FragmentLoader.LoadFragment(_fragmentData.SceneToLoad);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _fragmentMainTextTMP.text = _fragmentData.Description;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _fragmentMainTextTMP.text = _fragmentData.FragmentName;
        }
    }
}