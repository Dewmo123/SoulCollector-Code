using System.Collections.Generic;
using System.Linq;
using Inventory;
using UnityEditor;
using UnityEngine;
using Work.Core;

namespace Work.Item
{
    [CreateAssetMenu(fileName = "RollDataSO", menuName = "SO/Roll/RollDataList", order = 200)]
    public class RollDataListSO : ScriptableObject
    {
        public List<ItemDataSO> rollDataList;
        public ItemType itemType;
        
        public List<ItemDataSO> GetItemFromTier(ItemTier itemTier)
            => rollDataList.Where(item => item.itemTier == itemTier).ToList();
        
        private void OnValidate()
        {
            foreach (var item in rollDataList)
            {
                if (item.itemType != itemType)
                {
                    Debug.LogWarning($"{item} is not group on item type {itemType}");
                }
            }
        }
#if UNITY_EDITOR
        [ContextMenu("SetUp")]
        private void AutoFillRollData()
        {
            var allItems = AssetDatabase.FindAssets("t:ItemDataSO")
                .Select(guid => AssetDatabase.LoadAssetAtPath<ItemDataSO>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(item => item != null && item.itemType == itemType && item.rollable)
                .ToList();

            rollDataList = allItems;

            EditorUtility.SetDirty(this);
        }
#endif
    }
}