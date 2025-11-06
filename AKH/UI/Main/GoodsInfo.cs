using DewmoLib.Dependencies;
using Scripts.Environments;
using Scripts.Goods;
using Scripts.Network;
using Scripts.Players;
using Scripts.Players.Storages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Main
{
    public class GoodsInfo : MonoBehaviour
    {
        [SerializeField] private GoodsSO goodsSO;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image image;
        [Inject] private PlayerInfoStorage _storage;
        private void Awake()
        {
            image.sprite = goodsSO.itemIcon;
        }
        private void Update()
        {
            text.SetText(NumberFormatter.Format(_storage.GoodsStorage.Goods[goodsSO.goodsType]));
        }
    }
}
