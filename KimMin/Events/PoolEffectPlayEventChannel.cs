using DewmoLib.ObjectPool.RunTime;
using UnityEngine;
using Work.Common.Core;

namespace Work.Events
{
    public static class PoolEffectPlayEventChannel
    {
        public static PoolingEffectPlayEvent PoolingEffectPlayEvent = new();
    }

    public class PoolingEffectPlayEvent : GameEvent
    {
        public PoolItemSO item;
        public Vector2 position;
        public Quaternion rotation;

        public PoolingEffectPlayEvent Initializer(PoolItemSO item, Vector2 position, Quaternion rotation)
        {
            this.item = item;
            this.position = position;
            this.rotation = rotation;
            return this;
        }
    }
}