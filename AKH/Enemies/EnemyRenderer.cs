using Scripts.Enemies;
using Scripts.Entities;
using System;
using UnityEngine;

namespace Assets.Work.AKH.Scripts.Enemies
{
    public class EnemyRenderer : MonoBehaviour, IEntityComponent, IChangeableCompo<EnemySO>
    {
        [SerializeField] private Transform shadowTrm;
        public void Initialize(Entity entity)
        {
        }
        public void Change(EnemySO before, EnemySO current)
        {
            shadowTrm.localScale = current.shadowSize;
        }
    }
}
