
using System.Collections.Generic;
using System.Threading.Tasks;
using Scripts.Network;
using Work.Common.Core;

namespace Scripts.Players.Storages
{
    public class PlayerGoodsStorage
    {
        private WebClient _webClient;
        public Dictionary<GoodsType, int> Goods { get; private set; }

        public void Initialize(WebClient webClient, Dictionary<GoodsType, int> goods)
        {
            _webClient = webClient;
            Goods = goods;
        }

        public async Task<bool> ChangeGoods(GoodsType goodsType, int amount)
        {
            GoodsDTO dto = new()
            {
                Amount = amount,
                GoodsType = goodsType
            };
            bool success = await _webClient.SendPostRequest<GoodsDTO>("player/goods/changed", dto);
            if (success)
            {
                int prev = Goods[goodsType];
                Goods[goodsType] += amount;
                GameEventBus.RaiseEvent(NetworkEvents.ChangeGoodsEvent.Init(goodsType, prev, Goods[goodsType]));
            }
            return success;
        }
    }
}
