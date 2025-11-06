using Scripts.Players;

namespace Work.UI.Enchant
{
    public class EnchantPresenter
    {
        private readonly EnchantModel _model;
        private readonly EnchantView _view;
        
        public EnchantPresenter(EnchantModel model, EnchantView view)
        {
            _model = model;
            _view = view;
            _view.Initialize();
            _view.OnClickEnchant += _model.TryEnchant;
            _model.OnEnchant += _view.OnTryEnchant;
        }
    }
}