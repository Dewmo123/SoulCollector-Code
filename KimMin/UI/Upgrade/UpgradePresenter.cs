using Work.Upgrade;

namespace Work.UI
{
    public class UpgradePresenter
    {
        private readonly IUpgradeModel _model;
        private readonly IUpgradeView _view;

        public UpgradePresenter(IUpgradeModel model, IUpgradeView view, UpgradeDataSO data)
        {
            _model = model;
            _view = view;
            
            _view.SetUpUI(data);
            _view.OnUpgradeClicked += model.Upgrade;
            _model.OnUpdateStat += _view.UpdateUI;
        }
    }
}