using System;
using DewmoLib.Dependencies;
using Scripts.Players;
using Scripts.Players.Storages;
using UnityEngine;
using Work.Quest;

namespace Work.UI.Quest
{
    public class QuestInstaller : MonoBehaviour
    {
        [SerializeField] private QuestView view;
        [SerializeField] private QuestSequence quests;
        [Inject] private PlayerInfoStorage _playerInfo;

        private void Start()
        {
            var model = new QuestModel(quests, _playerInfo);
            var presenter = new QuestPresenter(model, view);
            model.SetNewGetNewQuest();
        }
    }
}