using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Work.UI.Gacha
{
    public class GachaChanceUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tier1Chance;
        [SerializeField] private TextMeshProUGUI tier2Chance;
        [SerializeField] private TextMeshProUGUI tier3Chance;
        [SerializeField] private Button closeButton;

        private void Awake()
        {
            closeButton.onClick.AddListener(delegate { gameObject.SetActive(false); });
        }

        public void SetChance(int rollCount)
        {
            tier1Chance.text = $"티어1 확률 : {1 + (rollCount / 200)}%";
            tier2Chance.text = $"티어2 확률 : {19 + rollCount / 50}%";
            tier3Chance.text = $"티어3 확률 : {80 - rollCount / 100}%";
        }
    }
}