namespace UI.Inventory
{
    public class InventoryPresenter
    {
        private readonly IInventoryModel _model;
        private readonly IInventoryView _view;

        public InventoryPresenter(IInventoryModel model, IInventoryView view)
        {
            _model = model;
            _view = view;

            _model.OnInventoryChanged += () => _view.UpdateInventoryUI(_model.Items);
            _model.OnItemChanged += (item) => _view.UpdateItemUI(item);
            _view.UpdateInventoryUI(_model.Items);
        }
    }
}