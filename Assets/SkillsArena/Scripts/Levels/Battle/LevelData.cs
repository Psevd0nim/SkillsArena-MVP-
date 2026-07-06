namespace SkillsArena
{
    public class LevelData
    {
        public int CurrentRound { get; private set; }

        private GameData _gameData;
        private BattleLevel_UI_Manager _battleLevelUIManager;

        public void Init(BattleLevel_UI_Manager battleLevelUIManager, GameData gameData)
        {
            _battleLevelUIManager = battleLevelUIManager;
            _gameData = gameData;
            CurrentRound = _gameData.CurrentRound;
            UpdateRoundText();
        }

        public void NextRound()
        {
            CurrentRound++;
            if (CurrentRound > 3)
                CurrentRound = 1;
            UpdateRoundText();
            _gameData.SetCurrentRound(CurrentRound);
        }

        private void UpdateRoundText()
        {
            _battleLevelUIManager.UpdateCurrentRoundText(CurrentRound);
        }
    }
}