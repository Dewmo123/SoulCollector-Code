using Scripts.Combat;
using Scripts.Entities;
using Scripts.PlayerEquipments.SkillSystem;
using Scripts.StatSystem;
using UnityEngine;
using Work.Common.Core;
using Work.Events;
using Work.Sound;

namespace Work.PlayerSkill
{
    public class ReaperSmashSkill : Skill
    {
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private SoundSO reaperSound;
        private readonly PlaySFXEvent playSFXEvent = SoundEventChannel.PlaySFXEvent;
        public override void InitSkill(Entity owner)
        {
            base.InitSkill(owner);
            damageCaster.InitCaster(owner);
        }

        public override void UseSkill()
        {
            damageCaster.CastDamage(Damage,false);
            GameEventBus.RaiseEvent(CameraEventChannel.CameraImpulseEvent);
            GameEventBus.RaiseEvent(playSFXEvent.Initializer(reaperSound));
        }

        public override void LevelUp()
        {
            Level++;
        }
    }
}