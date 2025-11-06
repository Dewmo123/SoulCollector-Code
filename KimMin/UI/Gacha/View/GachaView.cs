using System;
using System.Collections.Generic;
using DewmoLib.Dependencies;
using Inventory;
using Scripts.Network;
using Scripts.Players;
using Scripts.Players.Storages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Work.Common.Core;
using Work.Core;

namespace Work.UI.Gacha
{
    public class GachaView : MonoBehaviour, IGachaView
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private ItemType itemType;
        [SerializeField] private Button roll1Button;
        [SerializeField] private Button roll10Button;
        [SerializeField] private Button roll100Button;
        [SerializeField] private GachaResultUI resultUI;
        [SerializeField] private GachaChanceUI chanceUI;
        [SerializeField] private Button chanceButton;
        [Inject] private PlayerInfoStorage _storage;
        public event Action<int> OnRollClicked;
        private bool _isRolling;
        private int _rngMeter = 0;
        private int _rollCount = 0;

        public void Initialize(int initCount)
        {
            roll1Button.onClick.AddListener(() => TryRoll(1));
            roll10Button.onClick.AddListener(() => TryRoll(10));
            roll100Button.onClick.AddListener(() => TryRoll(100));
            chanceButton.onClick.AddListener(HandleChanceClick);
            OnRollClicked += ChangeMeter;
            resultUI.OnEndResult += HandleEnd;
            ChangeMeter(initCount);
            
            GameEventBus.AddListener<ChangeGoodsEvent>(HandleChangeGoodsEvent);
            CheckValid(_storage.GoodsStorage.Goods[GoodsType.Crystal]);
        }

        private void CheckValid(int cost)
        {
            roll1Button.interactable = cost > 50;
            roll10Button.interactable = cost > 500;
            roll100Button.interactable = cost > 5000;
        }

        private void HandleChangeGoodsEvent(ChangeGoodsEvent evt)
        {
            if (evt.goodsType == GoodsType.Crystal)
            {
                CheckValid(evt.prev + evt.next);
            }
        }

        private void HandleChanceClick()
        {
            chanceUI.gameObject.SetActive(true);
            chanceUI.SetChance(_rollCount);
        }

        private void TryRoll(int count)
        {
            if (_storage.GoodsStorage.Goods[GoodsType.Crystal] < count * 50 || _isRolling) return;
            _isRolling = true;
            OnRollClicked?.Invoke(count);
            _rollCount += count;
        }
        
        public void HandleEnd() => _isRolling = false;

        public void ShowResult(Dictionary<ItemDataSO, int> items)
        {
            resultUI.HandleShowResult(items);
        }

        private void OnDestroy()
        {
            roll1Button.onClick.RemoveAllListeners();
            roll10Button.onClick.RemoveAllListeners();
            roll100Button.onClick.RemoveAllListeners();
            chanceButton.onClick.RemoveAllListeners();
            OnRollClicked -= ChangeMeter;
        }
        
        public void ChangeMeter(int value)
        {
            if (_rngMeter + value > 1000)
            {
                _rngMeter = value - (1000 - _rngMeter);
            }
            
            _rngMeter += value;
            text.text = $"{Mathf.Clamp(_rngMeter, 0, 1000)} / 1000";
            fillImage.transform.localScale = new Vector3(_rngMeter / 1000f, 1, 1);
        }
    }
}