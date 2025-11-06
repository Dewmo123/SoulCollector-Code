using AYellowpaper.SerializedCollections;
using UnityEngine;
using System.Collections.Generic; // For Dictionary

namespace Scripts.StageSystem
{
    [CreateAssetMenu(fileName = "GameDB", menuName = "SO/GameDB")]
    public class GameDB : ScriptableObject
    {
        [field: SerializeField] public SerializedDictionary<string, StageSO> Stages { get; private set; }
        [field: SerializeField] public SerializedDictionary<string, DungeonSO> Dungeons { get; private set; }

        public T GetStageData<T>(string id) where T : BaseStageDataSO
        {
            if (typeof(T) == typeof(StageSO) && Stages.TryGetValue(id, out StageSO stage))
            {
                return stage as T;
            }
            if (typeof(T) == typeof(DungeonSO) && Dungeons.TryGetValue(id, out DungeonSO dungeon))
            {
                return dungeon as T;
            }
            // Add checks for other types
            Debug.LogWarning($"GameDB: Could not find stage data of type {typeof(T).Name} with ID {id}");
            return null;
        }
    }
}
