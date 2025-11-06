using System.Collections.Generic;
using System.Threading.Tasks;
using Inventory;

namespace Work.UI.Gacha
{
    public class GachaPresenter
    {
        private readonly GachaModel _model;
        private readonly IGachaView _view;

        public GachaPresenter(GachaModel model, IGachaView view)
        {
            _model = model;
            _view = view;

            _view.OnRollClicked += HandleRoll;
        }

        private async void HandleRoll(int count)
        {
            Dictionary<ItemDataSO, int> items = await _model.Roll(count);
            _view.ShowResult(items);
        }
    }
}