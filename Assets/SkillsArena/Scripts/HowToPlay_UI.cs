using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SkillsArena
{
    public class HowToPlay_UI : MonoBehaviour
    {
        public event Action OnClosed;

        public List<GameObject> pagesList;

        private int _currentPageIndex;

        void OnEnable()
        {
            _currentPageIndex = 0;
            DisableAllPages();
            EnableCurrentPage();
        }

        public void NextPressed()
        {
            AudioManager.Instance.PlaySomeSound(SoundType.Tutorial);
            _currentPageIndex++;
            DisableAllPages();
            EnableCurrentPage();
        }

        public void ClosePressed()
        {
            AudioManager.Instance.PlaySomeSound(SoundType.Tutorial);
            OnClosed?.Invoke();
        }

        private void DisableAllPages()
        {
            foreach(var page in pagesList)
                page.SetActive(false);
        }

        private void EnableCurrentPage()
        {
            pagesList.ElementAt(_currentPageIndex).SetActive(true);
        }
    }
}
