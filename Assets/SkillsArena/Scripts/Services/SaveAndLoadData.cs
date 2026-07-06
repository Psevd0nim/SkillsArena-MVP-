using Newtonsoft.Json;
using UnityEngine;

namespace SkillsArena
{
    public class SaveAndLoadData : IService
    {
        private GameData _gameData;

        public void SaveGameData()
        {
            string jsonData = JsonConvert.SerializeObject(_gameData);
            PlayerPrefs.SetString(Constants.GameDataKey, jsonData);
        }

        public GameData LoadGameData()
        {
            string jsonData = PlayerPrefs.GetString(Constants.GameDataKey, JsonConvert.SerializeObject(new GameData()));
            _gameData = JsonConvert.DeserializeObject<GameData>(jsonData);
            return _gameData;
        }
    }
}