using System;
using Inventory;
using TMPro;
using UI.Core;
using UnityEngine;
using Work.Core;

namespace UI.Base
{
    public abstract class BasePopup : MonoBehaviour
    {
    }

    public abstract class BasePopup<T> : BasePopup, IPopup where T : ScriptableObject
    {
        public virtual void EnableFor(T data, bool hasOption = false, Action<ItemDataSO> callback = null) { }

        void IPopup.EnableFor(ScriptableObject data, bool hasOption, Action<ItemDataSO> callback)
            => EnableFor((T)data, hasOption, callback);
    }
}