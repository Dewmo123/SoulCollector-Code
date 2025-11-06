using Inventory;
using UnityEngine;
using Work.Core;

namespace UI.Inventory
{
    public class InventoryInstaller : MonoBehaviour
    {
        [SerializeField] private InventoryDataList inventoryList;
        [SerializeField] private InventoryView inventoryView;
        [SerializeField] private InventoryFilterSO filter;
        
        private void Start()
        {
            var model = new InventoryModel(inventoryList, filter);
            var view = inventoryView;
            var presenter = new InventoryPresenter(model, view);
        }
    }
}