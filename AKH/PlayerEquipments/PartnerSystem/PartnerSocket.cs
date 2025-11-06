using System;
using UnityEngine;
using Work.Item;

namespace Scripts.PlayerEquipments.PartnerSystem
{
    public class PartnerSocket : MonoBehaviour, IEquipSocket
    {
        private Partner _currentPartner;
        public Partner CurrentPartner => _currentPartner;

        public event Action<EquipableItemSO> OnChange;

        public void ChangeItem(IEquipItem itemData)
        {
            if (_currentPartner != null)
                _currentPartner.gameObject.SetActive(false);
            _currentPartner = itemData as Partner;
            if (_currentPartner != null)
            {
                OnChange?.Invoke(_currentPartner.ItemData);
                _currentPartner.gameObject.SetActive(true);
                _currentPartner.transform.position = transform.position;
                _currentPartner?.ChangeState("MOVE",true);
            }
            else
            {
                OnChange?.Invoke(null);
            }
        }
        public void Reload() => ChangeItem(CurrentPartner);
    }
}
