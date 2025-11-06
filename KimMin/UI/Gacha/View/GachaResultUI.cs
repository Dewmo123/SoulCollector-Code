using System;
using System.Collections;
using System.Collections.Generic;
using Inventory;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Work.UI.Gacha
{
    public class GachaResultUI : MonoBehaviour
    {
        [SerializeField] private GameObject resultUI;
        [SerializeField] private Transform root;
        [SerializeField] private GachaItemUI itemUI;
        [SerializeField] private Button endButton;

        public event Action OnEndResult; 
        private Dictionary<ItemDataSO, int> _items;
        private List<GachaItemUI> _gachaList = new();
        private readonly WaitForSeconds _delay = new(0.1f);
        
        public void HandleShowResult(Dictionary<ItemDataSO, int> items)
        {
            _items = items;
            UIUtility.FadeUI(resultUI, 0.5f, false, HandleCompelted);
        }

        private void HandleCompelted()
        {
            for (int i = _gachaList.Count; i < _items.Count; i++)
            {
                var gacha = Instantiate(itemUI, root);
                _gachaList.Add(gacha);
            }
            
            StartCoroutine(UIResultRoutine());
        }

        private IEnumerator UIResultRoutine()
        {
            int idx = 0;
            foreach (var item in _items)
            {
                _gachaList[idx].gameObject.SetActive(true);
                _gachaList[idx++].EnableFor(item.Key, item.Value);
                yield return _delay;
            }

            endButton.gameObject.SetActive(true);
            endButton.onClick.AddListener(HandleEndResult);
        }

        private void HandleEndResult()
        {
            for (int i = 0; i < _items.Count; i++)
            {
                _gachaList[i].Disable();
            }
            
            OnEndResult?.Invoke();
            endButton.onClick.RemoveAllListeners();
            endButton.gameObject.SetActive(false);
            UIUtility.FadeUI(resultUI, 0.5f, true);
        }
    }
}