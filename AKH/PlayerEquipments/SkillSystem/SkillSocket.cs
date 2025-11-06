using System;
using UnityEngine;
using Work.Item;

namespace Scripts.PlayerEquipments.SkillSystem
{
    public delegate void OnCoolDown(float current, float total);
    public class SkillSocket : IEquipSocket
    {
        public event OnCoolDown OnCoolDown;
        public event Action<EquipableItemSO> OnChange;
        private float _cooldownTimer;
        public Skill CurrentSkill { get; set; }

        public void UpdateSocket()
        {
            if (_cooldownTimer >= 0 && CurrentSkill != null)
            {
                _cooldownTimer -= Time.deltaTime;
                if (_cooldownTimer <= 0)
                    _cooldownTimer = 0;
                OnCoolDown?.Invoke(_cooldownTimer, CurrentSkill.SkillData.cooldown);
            }
        }
        public bool CanUseSkill()
            => _cooldownTimer <= 0f && CurrentSkill != null;
        public void SetCooldown()
            => _cooldownTimer = CurrentSkill.SkillData.cooldown;

        public void ChangeItem(IEquipItem itemData)
        {
            CurrentSkill = itemData as Skill;
            if (CurrentSkill == null)
            {
                OnChange?.Invoke(null);
                _cooldownTimer = 0;
            }
            else
            {
                _cooldownTimer = CurrentSkill.SkillData.cooldown;
                OnChange?.Invoke(CurrentSkill.SkillData);
            }
        }

        public void Reload()
        {
            ChangeItem(CurrentSkill);
        }
    }
}
