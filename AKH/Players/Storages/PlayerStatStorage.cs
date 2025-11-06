
using System.Collections.Generic;
using System.Threading.Tasks;
using Scripts.Network;
using Work.Common.Core;

namespace Scripts.Players.Storages
{
    public class PlayerStatStorage
    {
        private WebClient _webClient;
        public Dictionary<StatType, int> Stats { get; private set; }

        public void Initialize(WebClient webClient, Dictionary<StatType, int> stats)
        {
            _webClient = webClient;
            Stats = stats;
        }

        public async Task<bool> ChangeStat(StatType statType, int level)
        {
            StatDTO stat = new()
            {
                Level = level,
                StatType = statType
            };
            bool success = await _webClient.SendPostRequest<StatDTO>("player/stat/level-up", stat);
            if (success)
            {
                int prev = Stats[statType];
                Stats[statType] += level;
                GameEventBus.RaiseEvent(NetworkEvents.ChangeStatEvent.Init(statType, prev, Stats[statType]));
            }
            return success;
        }
    }
}
