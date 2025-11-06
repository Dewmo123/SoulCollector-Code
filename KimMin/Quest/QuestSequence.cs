using System.Collections.Generic;
using UnityEngine;

namespace Work.Quest
{
    [CreateAssetMenu(fileName = "QuestSequence", menuName = "SO/Quest/QuestSequence", order = 0)]
    public class QuestSequence : ScriptableObject
    {
        public List<QuestSO> quests;
    }
}