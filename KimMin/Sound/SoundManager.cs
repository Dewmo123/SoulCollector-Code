using System.Collections.Generic;
using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using UnityEngine;
using Work.Common.Core;
using Work.Core;
using Work.Events;

namespace Work.Sound
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private PoolItemSO soundPlayer;

        [Inject] private PoolManagerMono _poolManager;

        private Dictionary<int, SoundPlayer> soundPlayerDict = new();

        private void Awake()
        {
            GameEventBus.AddListener<PlaySFXEvent>(HandlePlaySFXEvent);
            GameEventBus.AddListener<StopSoundEvent>(HandleStopSoundEvent);
        }

        private void OnDestroy()
        {
            GameEventBus.RemoveListener<PlaySFXEvent>(HandlePlaySFXEvent);
            GameEventBus.RemoveListener<StopSoundEvent>(HandleStopSoundEvent);   
        }

        private void HandleStopSoundEvent(StopSoundEvent evt)
        {
            if (soundPlayerDict.TryGetValue(evt.channel, out SoundPlayer beforePlayer))
            {
                beforePlayer.StopAndGotoPool();
                soundPlayerDict.Remove(evt.channel);
            }
        }

        private void HandlePlaySFXEvent(PlaySFXEvent evt)
        {
            SoundPlayer player = _poolManager.Pop<SoundPlayer>(soundPlayer);
            player.PlaySound(evt.clip);

            if (evt.channel > 0 && evt.clip.loop)
            {
                if (soundPlayerDict.TryGetValue(evt.channel, out SoundPlayer beforePlayer))
                {
                    beforePlayer.StopAndGotoPool();
                    soundPlayerDict.Remove(evt.channel);
                }
                soundPlayerDict.Add(evt.channel, player);
            }
            else if (evt.channel <= 0 && evt.clip.loop)
            {
                Debug.LogWarning($"사운드 루프 설정이 되었으나  채널이 0 이하입니다. {evt.clip.name}");
            }
        }
    }
}