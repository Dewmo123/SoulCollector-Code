using System;
using TMPro;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Base
{
    public class MessagePopup : BaseUI
    {
        [SerializeField] private TextMeshProUGUI messageText;
        [SerializeField] private Button acceptButton;
        [SerializeField] private Button cancelButton;
        
        private Action _onAccept;
        private Action _onCancel;
        
        public void ShowPopup(string message, bool hasCanel = false,
            Action onAccept = null, Action onCancel = null)
        {
            messageText.text = message;
            cancelButton.gameObject.SetActive(false);
            
            _onAccept = onAccept;
            _onCancel = onCancel;
            
            acceptButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();
            
            acceptButton.onClick.AddListener(() =>
            {
                _onAccept?.Invoke();
                HidePopup();
            });

            if (hasCanel)
            {
                cancelButton.gameObject.SetActive(true);
                cancelButton.onClick.AddListener(() =>
                {
                    _onCancel?.Invoke();
                    HidePopup();
                });
            }
  
            
            UIUtility.ShowUI(gameObject);
        }

        public void HidePopup()
        {
            UIUtility.ShowUI(gameObject, false);
        }
        
        private void OnDestroy()
        {
            acceptButton?.onClick.RemoveAllListeners();
            cancelButton?.onClick.RemoveAllListeners();
        }
    }
}