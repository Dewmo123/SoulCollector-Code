using System;
using System.Collections.Generic;
using Inventory;

namespace Work.UI.Gacha
{
    public interface IGachaView
    {
        event Action<int> OnRollClicked;
        
        void Initialize(int initCount);
        void ShowResult(Dictionary<ItemDataSO, int> items);
    }
}