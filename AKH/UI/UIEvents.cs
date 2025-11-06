using Work.Common.Core;

namespace Scripts.UI
{
    public static class UIEvents
    {
        public readonly static SetStageTextEvent SetStageTextEvent = new();
        public readonly static SetStageProgressEvent SetStageProgressEvent = new();
        public readonly static SetMenuButtonEnableEvent SetMenuButtonEnableEvent = new();

    }
    public class SetStageTextEvent : GameEvent
    {
        public string text;
        public SetStageTextEvent Init(string text)
        {
            this.text = text;
            return this;
        }
    }
    public class SetStageProgressEvent : GameEvent
    {
        public string text;
        public float current, total;
        public SetStageProgressEvent Init(string text,float current,float total)
        {
            this.text = text;
            this.current = current;
            this.total = total;
            return this;
        }
    }
    public class SetMenuButtonEnableEvent : GameEvent
    {
        public bool enabled;
        public SetMenuButtonEnableEvent Init(bool enabled)
        {
            this.enabled = enabled;
            return this;
        }
    }
}
