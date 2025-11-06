using Scripts.StageSystem;
using System;
using UnityEngine;

namespace Work.Dungeon
{
    public class DungeonUI : MonoBehaviour
    {
        [SerializeField] private GameDB dungeonList;
        [SerializeField] private DungeonContentUI dungeonContentUI;
        [SerializeField] private Transform root;

        private void Awake()
        {
            foreach (var dungeon in dungeonList.Dungeons)
            {
                Instantiate(dungeonContentUI, root).EnableFor(dungeon.Value);
            }
        }
    }
}