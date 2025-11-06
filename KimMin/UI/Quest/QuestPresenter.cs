using UnityEngine;

namespace Work.UI.Quest
{
    public class QuestPresenter
    {
        private readonly QuestModel _model;
        private readonly QuestView _view;

        public QuestPresenter(QuestModel model, QuestView view)
        {
            _model = model;
            _view = view;
            _model.OnNewQuestStart += _view.SetNewQuest;
            _model.OnProgressChanged += view.UpdateQuest;
            _view.OnGetNewQuest += _model.CompleteQuest;
            _view.OnGetNewQuest += _model.SetNewGetNewQuest;
        }
    }
}