using System;
using Inventory;
using UnityEngine;
using Work.Core;

namespace UI.Base
{
    public interface IPopup
    {
        void EnableFor(ScriptableObject data, bool hasOption = false, Action<ItemDataSO> callback = null);
    }
}