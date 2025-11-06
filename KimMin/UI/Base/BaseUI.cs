using UnityEngine;

namespace UI.Base
{
    public class BaseUI : MonoBehaviour
    {
        public virtual void InitUI() { }

        protected virtual void Awake()
        {
            InitUI();
        }

        public virtual void OpenUI()
        {
            gameObject.SetActive(true);
        }
        
        public virtual void CloseUI()
        {
            gameObject.SetActive(false);
        }
    }
}