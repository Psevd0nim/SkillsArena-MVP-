using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkillsArena
{
    public class SceneLoader
    {
        private ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }

        public void LoadSceneByName(string name, float time, Action onLoadedCallback)
        {
            _coroutineRunner.StartCoroutine(LoadSceneAsync(name, time, onLoadedCallback));
        }

        private IEnumerator LoadSceneAsync(string name, float time, Action onLoadedCallback)
        {
            yield return new WaitForSeconds(time);
            AsyncOperation loadSceneOperation = SceneManager.LoadSceneAsync(name);
            while (!loadSceneOperation.isDone)
            {
                yield return null;
            }
            onLoadedCallback?.Invoke();
        }
    }
}