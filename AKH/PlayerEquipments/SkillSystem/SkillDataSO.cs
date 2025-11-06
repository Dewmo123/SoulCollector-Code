using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;
using Work.Item;

namespace Scripts.PlayerEquipments.SkillSystem
{
    [CreateAssetMenu(fileName ="SkillData",menuName ="SO/Skill/Data")]
    public class SkillDataSO : EquipableItemSO
    {
        public float cooldown;
        public string animName = "Default";
        public int animhash => Animator.StringToHash(animName);
    }
}
