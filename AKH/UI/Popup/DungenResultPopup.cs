using Cysharp.Threading.Tasks;
using Inventory;
using Scripts.StageSystem;
using System;
using System.Collections.Generic;
using TMPro;
using UI.Base;
using UI.Core;
using UnityEngine;
using UnityEngine.UI;
using Work.Dungeon;

namespace Scripts.UI.Popup
{
    public class DungenResultPopup : BasePopup<DungeonSO>
    {
        [SerializeField] private TextMeshProUGUI resultTxt;
        [SerializeField] private DungeonItemUI itemPrefab;
        [SerializeField] private Transform root;
        [SerializeField] private Button okButton;
        private Action<ItemDataSO> _callback;
        private List<DungeonItemUI> _itemUIs = new(20);
        private void Awake()
        {
            okButton.onClick.AddListener(HandleButtonClick);
        }
        private void OnDestroy()
        {
            okButton.onClick.RemoveListener(HandleButtonClick);
        }

        public override async void EnableFor(DungeonSO data, bool hasOption = false, Action<ItemDataSO> callback = null)
        {
            UIUtility.ShowUI(gameObject);
            if (hasOption)
            {
                resultTxt.text = "Victory";
                int i = 0;
                foreach (var item in data.rewards)
                {
                    await UniTask.NextFrame();
                    if (_itemUIs.Count <= i)
                    {
                        var newitem = Instantiate(itemPrefab, root);
                        _itemUIs.Add(newitem);
                    }
                    DungeonItemUI itemUI = _itemUIs[i];
                    itemUI.EnableFor(item.Key.itemIcon, item.Value);
                    i++;
                }
            }
            else
            {
                resultTxt.text = "Failed";
            }
            _callback = callback;
        }
        private void HandleButtonClick()
        {
            _callback?.Invoke(null);
            UIUtility.ShowUI(gameObject, false);
        }
    }
}
