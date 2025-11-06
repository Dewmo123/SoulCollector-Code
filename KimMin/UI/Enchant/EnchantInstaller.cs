using System;
using DewmoLib.Dependencies;
using Scripts.Players;
using Scripts.Players.Storages;
using Unity.VisualScripting;
using UnityEngine;

namespace Work.UI.Enchant
{
    public class EnchantInstaller : MonoBehaviour
    {
        [SerializeField] private EnchantView view;

        private void Start()
        {
            var model = new EnchantModel();
            var presenter = new EnchantPresenter(model, view);
        }
    }
}