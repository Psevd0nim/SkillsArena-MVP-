using System.Collections;
using UnityEngine;

namespace SkillsArena
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator enumerator);
    }
}