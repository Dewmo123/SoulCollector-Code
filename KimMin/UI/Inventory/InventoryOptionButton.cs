using UnityEngine;
using UnityEngine.UI;
using Work.Core;

namespace UI.Inventory
{
    [RequireComponent(typeof(Button))]
    public class InventoryOptionButton : MonoBehaviour
    {
        [field: SerializeField] public ItemType ItemType { get; private set; }
        [field: SerializeField] public InventoryEquipButton[] EquipButtons { get; private set; }
        [field: SerializeField] public GameObject Parent { get; private set; }
        [SerializeField] private GameObject view;
        [SerializeField] private GameObject slots;
        public Button Button { get; private set; }

        private void Awake()
        {
            Button = GetComponent<Button>();
        }

        public void SetActiveView(bool isActive) => view.SetActive(isActive);
    }
}