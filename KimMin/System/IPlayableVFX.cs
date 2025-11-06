using UnityEngine;

namespace Work.System
{
    public interface IPlayableVFX
    {
        public string VfxName { get; }
        public void PlayVFX(Vector2 position, Quaternion rotation);
        public void StopVFX();
    }
}