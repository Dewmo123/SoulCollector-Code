using TMPro;
using UI.Base;
using UnityEngine;
using UnityEngine.UI;

namespace Work.Dungeon
{
    public class DungeonItemUI : MonoBehaviour, IUIElement<Sprite, int>
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI text;

        public void EnableFor(Sprite item, int count)
        {
            icon.sprite = item;
            if (count > 0)
                text.text = count.ToString();
            else
                text.text = "";
        }

        public void Disable() { }
    }
}