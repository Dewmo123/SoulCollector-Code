using DewmoLib.Utiles;
using System.Collections.Generic;
using Work.Common.Core;
using Work.Dungeon;

namespace Scripts.StageSystem
{
    public static class StageEvents
    {
        public static readonly MoveEvent MoveEvent = new();
        public static readonly StopEvent StopEvent = new();
        public static readonly DropItemEvent DropItemEvent = new();
        public static readonly EnterDungeonEvent EnterDungeonEvent = new();
        public static readonly BossKeyEnableEvent BossKeyEnableEvent = new();
        public static readonly EnterBossEvent EnterBossEvent = new();
    }
    public class MoveEvent : GameEvent
    {
        public float velocity;
        public MoveEvent Init(float vel)
        {
            velocity = vel;
            return this;
        }
    }
    public class StopEvent : GameEvent { }
    public class DropItemEvent : GameEvent
    {
    }
    public class EnterDungeonEvent : GameEvent
    {
        public string dungeonName;

        public EnterDungeonEvent Init(string dungeonName)
        {
            this.dungeonName = dungeonName;
            return this;
        }
    }
    public class BossKeyEnableEvent : GameEvent
    {
        
    }
    public class EnterBossEvent : GameEvent
    {

    }
}
