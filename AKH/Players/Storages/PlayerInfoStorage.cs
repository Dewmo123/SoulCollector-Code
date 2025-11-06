using DewmoLib.Dependencies;
using Scripts.Network;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Players.Storages
{
    [Provide]
    public class PlayerInfoStorage : MonoBehaviour, IDependencyProvider
    {
        [Inject] private WebClient _webClient;
        public string Id { get; private set; }
        public PlayerStatStorage StatStorage { get; private set; }
        public PlayerGoodsStorage GoodsStorage { get; private set; }
        public PlayerSkillStorage SkillStorage { get; private set; }
        public PlayerPartnerStorage PartnerStorage { get; private set; }
        public PlayerChapterStorage ChapterStorage { get; private set; }

        private async void Awake()
        {
            DontDestroyOnLoad(gameObject);
            PlayerDTO dto = await _webClient.SendGetRequest<PlayerDTO>("player/get-player-infos");
            if (dto != null)
            {
                Id = dto.Id;
                StatStorage = new PlayerStatStorage();
                StatStorage.Initialize(_webClient, dto.Stats);

                GoodsStorage = new PlayerGoodsStorage();
                GoodsStorage.Initialize(_webClient, dto.Goods);

                SkillStorage = new PlayerSkillStorage();
                SkillStorage.Initialize(_webClient, dto.Skills, dto.SkillEquips);

                PartnerStorage = new PlayerPartnerStorage();
                PartnerStorage.Initialize(_webClient, dto.Partners, dto.PartnerEquips);

                ChapterStorage = new PlayerChapterStorage();
                ChapterStorage.Initialize(_webClient, dto.Chapter);

                SceneManager.LoadScene("AKH");
            }
            else
            {
                SceneManager.LoadScene("TitleScene");
            }
        }

        private async void OnDestroy()
        {
            await _webClient.SendDeleteRequest("player/log-out");
            await _webClient.SendDeleteRequest("authorize/log-out");
        }
    }
}
