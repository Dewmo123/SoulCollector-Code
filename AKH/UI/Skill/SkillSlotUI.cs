using DewmoLib.Dependencies;
using Inventory;
using Scripts.PlayerEquipments.SkillSystem;
using Scripts.Players;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Skill
{
    public class SkillSlotUI : MonoBehaviour
    {
        [SerializeField] private int slotIndex;
        [SerializeField] private Image skillImage;
        [SerializeField] private Image cooldownImage;
        private PlayerSkillManager _skillManager;
        [Inject] private Player _player;
        private SkillSocket _mySocket => _skillManager.Sockets[slotIndex] as SkillSocket;
        private void Start()
        {
            _skillManager = _player.GetCompo<PlayerSkillManager>();
            _mySocket.OnChange += HandleSkillChange;
            _mySocket.OnCoolDown += HandleCooldown;
            if (_mySocket.CurrentSkill != null)
                HandleSkillChange(_mySocket.CurrentSkill.SkillData);
        }
        public void UseSkill()
        {
            _skillManager.RegisterSkill(slotIndex);
        }
        private void HandleCooldown(float current, float total)
        {
            cooldownImage.fillAmount = current / total;
        }

        private void HandleSkillChange(ItemDataSO skillData)
        {
            if (skillData == null)
                skillImage.sprite = null;
            else
                skillImage.sprite = skillData.itemIcon;
            cooldownImage.fillAmount = 1;
        }
    }
}
