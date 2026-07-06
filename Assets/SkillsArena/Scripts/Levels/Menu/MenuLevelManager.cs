using UnityEngine;

namespace SkillsArena
{
    public class MenuLevelManager : LevelManager
    {
        public MenuLevelStateType MenuLevelStateType { get; private set; }

        [SerializeField] private Transition_UI _transition;
        [SerializeField] private MenuLevel_UI_Manager _menuLevel_UI_Manager;

        public override void Init()
        {
            _menuLevel_UI_Manager.Init(this);
            _menuLevel_UI_Manager.OnPlayPressed += LoadBattleLevel;
        }

        public override void StartLevel()
        {
            MenuLevelStateType = MenuLevelStateType.InMenu;
            _transition.StartOpenAnim();
        }

        public void LoadBattleLevel()
        {
            MenuLevelStateType = MenuLevelStateType.None;
            _transition.StartCloseAnim();
            OnExitLevel?.Invoke(this, Constants.BattleLevelSceneName, 1.2f);
        }
    }

    //I don't think it's the best solution, I added this at the end. State Machine is probably better.
    public enum MenuLevelStateType
    {
        None, InMenu
    }
}