using Unity.VisualScripting;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Work.System
{
    public class PlayAnimationVFX : MonoBehaviour, IPlayableVFX
    {
        [SerializeField] private AnimationClip clip;
        [SerializeField] private Animator animator;
        [SerializeField] private GameObject effectObj;
        public string VfxName => clip.name;
        private bool _isPlaying;
        
        private void Update()
        {
            if(!_isPlaying) return;
            var info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.normalizedTime >= 1f && !info.loop)
            {
                effectObj.SetActive(false);
                _isPlaying = false;
            }
        }

        public void PlayVFX(Vector2 position, Quaternion rotation)
        {
            effectObj.SetActive(true);
            animator.Play(VfxName);
            transform.position = position;
            transform.rotation = rotation;
            _isPlaying = true;
        }

        public void StopVFX()
        {
            animator.StopPlayback();
        }
    }
}