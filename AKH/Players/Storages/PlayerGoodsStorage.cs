
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Scripts.Network;
using UnityEngine;
using Work.Common.Core;

namespace Scripts.Players.Storages
{
    public class PlayerGoodsStorage
    {
        private WebClient _webClient;
        public Dictionary<GoodsType, int> Goods { get; private set; } = new();

        public void Initialize(WebClient webClient, Dictionary<GoodsType, int> goods)
        {
            _webClient = webClient;
            Goods = goods ?? new Dictionary<GoodsType, int>();
        }

        public async Task<bool> ChangeGoods(GoodsType goodsType, int amount)
        {
            if (!TryCalculateNextAmount(goodsType, amount, out int prev, out int next))
            {
                return false;
            }

            if (amount == 0)
            {
                return true;
            }

            GoodsDTO dto = new()
            {
                Amount = amount,
                GoodsType = goodsType
            };

            bool success = await _webClient.SendPostRequest<GoodsDTO>("player/goods/changed", dto);
            if (!success)
            {
                Debug.LogWarning($"PlayerGoodsStorage: Server rejected goods change. Type: {goodsType}, Amount: {amount}");
                return false;
            }

            Goods[goodsType] = next;
            GameEventBus.RaiseEvent(NetworkEvents.ChangeGoodsEvent.Init(goodsType, prev, next));
            return true;
        }

        public bool TryGetGoods(GoodsType goodsType, out int amount)
        {
            amount = 0;
            return Goods.TryGetValue(goodsType, out amount);
        }

        public int GetGoodsAmount(GoodsType goodsType)
        {
            return TryGetGoods(goodsType, out int amount) ? amount : 0;
        }

        public bool HasEnoughGoods(GoodsType goodsType, int amount)
        {
            if (amount <= 0)
            {
                return true;
            }

            return TryGetGoods(goodsType, out int currentAmount) && currentAmount >= amount;
        }

        private bool TryCalculateNextAmount(GoodsType goodsType, int amount, out int prev, out int next)
        {
            prev = 0;
            next = 0;

            if (!Goods.TryGetValue(goodsType, out prev))
            {
                if (amount < 0)
                {
                    Debug.LogWarning($"PlayerGoodsStorage: Not enough goods. Type: {goodsType}, Current: 0, Change: {amount}");
                    return false;
                }

                prev = 0;
            }

            int nextAmount = prev + amount;
            if (nextAmount < 0)
            {
                Debug.LogWarning($"PlayerGoodsStorage: Goods amount cannot be negative. Type: {goodsType}, Current: {prev}, Change: {amount}");
                return false;
            }

            if (nextAmount > int.MaxValue)
            {
                Debug.LogWarning($"PlayerGoodsStorage: Goods amount exceeds int range. Type: {goodsType}, Current: {prev}, Change: {amount}");
                return false;
            }

            next = (int)nextAmount;
            return true;
        }
    }
}
