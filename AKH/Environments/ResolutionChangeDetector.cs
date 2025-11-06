using UnityEngine;
using Work.Common.Core;

namespace Scripts.Environments
{
    public class ResolutionChangedEvent : GameEvent{ };
    public class ResolutionChangeDetector : MonoBehaviour
    {
        private ResolutionChangedEvent changeEvent;
        private int lastWidth;
        private int lastHeight;

        private void Awake()
        {
            lastWidth = Screen.width;
            lastHeight = Screen.height;
        }

        private void Update()
        {
            if (Screen.width != lastWidth || Screen.height != lastHeight)
            {
                lastWidth = Screen.width;
                lastHeight = Screen.height;
                GameEventBus.RaiseEvent(changeEvent);
            }
        }
    }
}
