
using System.Threading.Tasks;
using Scripts.Network;

namespace Scripts.Players.Storages
{
    public class PlayerChapterStorage
    {
        private WebClient _webClient;
        public ChapterDTO Chapter { get; private set; }

        public void Initialize(WebClient webClient, ChapterDTO chapter)
        {
            _webClient = webClient;
            Chapter = chapter;
        }

        public async Task<bool> ChangeChapter(int chapter)
        {
            ChangeChapterDTO dto = new()
            {
                Chapter = chapter
            };
            ChapterDTO newDto = await _webClient.SendPostRequest<ChangeChapterDTO, ChapterDTO>("player/stage/chapter-changed", dto);
            if (newDto.Chapter == Chapter.Chapter + chapter)
            {
                Chapter = newDto;
                return true;
            }
            return false;
        }

        public async Task<bool> ChangeStage(int stage)
        {
            ChangeStageDTO dto = new()
            {
                Stage = stage
            };
            ChapterDTO newDto = await _webClient.SendPostRequest<ChangeStageDTO, ChapterDTO>("player/stage/stage-changed", dto);
            if (newDto.Stage == Chapter.Stage + stage)
            {
                Chapter = newDto;
                return true;
            }
            return false;
        }

        public async Task<bool> EnemyDead(int cnt)
        {
            bool success = await _webClient.SendPostRequest<EnemyDeadDTO>("player/stage/enemy-dead", new() { EnemyCount = cnt });
            if (success)
                Chapter.EnemyCount += cnt;
            return success;
        }
    }
}
