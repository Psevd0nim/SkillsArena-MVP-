using UnityEngine;

namespace SkillsArena
{
    public class Wall : MonoBehaviour
    {
        public BoxCollider2D Collider { get; private set;}

        public void Init(Vector2 colliderSize)
        {
            Collider = gameObject.AddComponent<BoxCollider2D>();
            Collider.size = colliderSize;
        }
    }
}