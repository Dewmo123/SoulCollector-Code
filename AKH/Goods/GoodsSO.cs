using Inventory;
using Scripts.Network;
using UnityEngine;

namespace Scripts.Goods
{
    [CreateAssetMenu(fileName = "GoodsSO", menuName = "SO/GoodsSO")]
    public class GoodsSO : ItemDataSO
    {
        public GoodsType goodsType;
    }
}