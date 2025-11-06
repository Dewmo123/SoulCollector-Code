using DewmoLib.Dependencies;
using Scripts.Entities;
using Scripts.Players;
using Scripts.Players.Storages;
using Scripts.StatSystem;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Work.Upgrade;

namespace Work.UI
{
    public class UpgradeInstaller : MonoBehaviour
    {
        [SerializeField] private UpgradeDataSO[] upgradeView;
        [SerializeField] private Transform root;
        private List<IDisposable> _models = new(); 
        [Inject] private PlayerInfoStorage _storage;
        [Inject] private Player _player;
        private void Start()
        {
            foreach (var data in upgradeView)
            {
                int level = _storage.StatStorage.Stats[data.statType];
                var statCompo = _player.GetCompo<EntityStat>();
                var model = new UpgradeModel(data, level, statCompo);
                _models.Add(model);
                var newView = Instantiate(data.upgradeView, root);
                var presenter = new UpgradePresenter(model, newView, data);
                StatSO stat = statCompo.GetStat(data.upgradeStat);
                model.HandleStatChanged(stat, stat.Value, 0);
            }
        }
        private void OnDestroy()
        {
            foreach(var item in _models)
                item.Dispose();
        }
    }
}