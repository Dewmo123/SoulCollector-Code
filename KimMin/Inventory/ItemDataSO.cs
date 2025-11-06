using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using Scripts.Network;
using Scripts.StatSystem;
using UnityEditor;
using UnityEngine;
using Work.Core;
using Work.Item;

namespace Inventory
{
    [CreateAssetMenu(fileName = "item", menuName = "SO/Inventory/Item", order = 100)]
    public class ItemDataSO : ScriptableObject
    {
        public Sprite itemIcon;
        public string itemName;
        [TextArea] public string itemDescription;
        public ItemType itemType;
        public ItemTier itemTier;
        public bool rollable;
        public bool enchantable;
        
        
        [field:SerializeField] public int ItemID { get; private set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(this, out string guid, out long localId);
            Debug.Assert(!string.IsNullOrEmpty(guid), "Item must have a valid GUID.");
            ItemID = Animator.StringToHash(guid);
        }
#endif
    }
}