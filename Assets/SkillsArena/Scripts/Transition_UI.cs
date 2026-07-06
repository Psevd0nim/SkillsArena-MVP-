using System;
using UnityEngine;
using UnityEngine.UI;

namespace SkillsArena
{
    public class Transition_UI : MonoBehaviour
    {
        [Range(0, 1f)]
        public float value;
        public Image image;
        public Animator transitionAnimator;

        private void Awake()
        {
            InitProgressBar();
        }

        private void Update()
        {
            image.material.SetFloat("_FillAmount", value);
        }

        public void StartOpenAnim()
        {
            transitionAnimator.SetTrigger("Open");
        }

        public void StartCloseAnim()
        {
            transitionAnimator.SetTrigger("Close");
        }

        private void InitProgressBar()
        {
            Material mat = Instantiate(image.material);
            image.material = mat;
            image.material.SetFloat("_FillAmount", 1);
        }
    }
}
