using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Engine.CitationPlugin.GenericMessagePlugin
{
    public class GenericMessageDisplayer : MonoBehaviour
    {
        //Todo : Afficher le nombre de fish dans la room à la fin via un callbakc avec ça :) 5400kg/s
        [Header("References")]
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _tmpMainText;
        [SerializeField] private TMP_Text _tmpSubText;

        [SerializeField] private List<Message> _messages; 
        [SerializeField] private int _currentMessageIndex = -1;

        private void Start()
        {
            Hide();
        }

        //Todo : Skip/Next & Quit Button

        public void ShowNextMessage()
        {
            _currentMessageIndex++;

            //-- If no more message, Hide
            if (_currentMessageIndex >= _messages.Count)
            {
                Hide(1f);
                return;
            }
            
            ShowMessage(_messages[_currentMessageIndex]);
        }

        public void ShowMessage(Message message)
        {
            Sequence showSequence = DOTween.Sequence();
            _tmpMainText.text = message.Content;
            _tmpSubText.text = message.SubContent;
            
            showSequence.Append(_canvasGroup.DOFade(1, message.InDuration));
            showSequence.Append(_tmpMainText.DOFade(1, message.InDuration));

            if (!String.IsNullOrEmpty(message.SubContent))
            {
                showSequence.AppendInterval(message.DisplayDuration / 2f);
                showSequence.Append(_tmpSubText.DOFade(1, message.InDuration));
            }

            DOVirtual.DelayedCall(message.DisplayDuration, () => Hide(message.OutDuration));
        }

        private void Hide(float duration = 0)
        {
            _canvasGroup.DOFade(0, duration);
            _tmpMainText.DOFade(0, duration);
            _tmpSubText.DOFade(0, duration);
        }
        
        [System.Serializable]
        public class Message
        {
            [SerializeField] public string Content = string.Empty;
            [SerializeField] public string SubContent = string.Empty;
            [SerializeField] public float InDuration = 1f;
            [SerializeField] public float DisplayDuration = 2f;
            [SerializeField] public float OutDuration = 1f;
            [SerializeField] public bool AutoSkip = true;
        }
    }
}