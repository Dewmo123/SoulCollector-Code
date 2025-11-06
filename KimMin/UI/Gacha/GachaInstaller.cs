using DewmoLib.Dependencies;
using Inventory;
using Scripts.Players;
using Scripts.Players.Storages;
using UnityEngine;
using Work.Core;
using Work.Item;

namespace Work.UI.Gacha
{
    public class GachaInstaller : MonoBehaviour
    {
        [SerializeField] private InventoryDataList inventory;
        [SerializeField] private GachaView view;
        [SerializeField] private ItemType itemType;

        [Inject] private PlayerInfoStorage _storage;

        private void Start()
        {
            string key = $"{itemType}{_storage.Id}";
            int init = PlayerPrefs.GetInt(key);
            GachaModel gachaModel = new GachaModel(itemType, inventory, _storage, init);
            IGachaView gachaView = view;

            var presenter = new GachaPresenter(gachaModel, gachaView);
            gachaView.Initialize(init);
        }
    }
}