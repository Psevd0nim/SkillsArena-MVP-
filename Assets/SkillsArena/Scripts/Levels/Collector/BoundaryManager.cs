using UnityEngine;

namespace SkillsArena
{
    public class BoundaryManager : MonoBehaviour
    {
        public float boundarySize;
        public Transform boundariesParent;
        public RectTransform topBar;

        private Rect _playField;
        private float _leftPos;
        private float _rightPos;
        private float _upPos;
        private float _downPos;

        public void SetBoundWalls()
        {
            Camera cam = Camera.main;

            float screenHeight = cam.orthographicSize * 2f;
            float screenWidth = screenHeight * cam.aspect;

            float halfW = screenWidth / 2f;
            float halfH = screenHeight / 2f;

            Vector2 startPos = new Vector2(transform.position.x + (-halfW), transform.position.y + (-halfH));
            Vector2 scale = new Vector2(screenWidth, screenHeight);
            _playField = new Rect(startPos, scale);

            Wall tempWall;

            tempWall = CreateWall(
                name: "BoundaryLeft",
                position: new Vector2(_playField.xMin, 0f),
                size: new Vector2(boundarySize, screenHeight)
            );
            _leftPos = tempWall.transform.position.x + tempWall.Collider.size.x / 2;

            tempWall = CreateWall(
                name: "BoundaryRight",
                position: new Vector2(_playField.xMax, 0f),
                size: new Vector2(boundarySize, screenHeight)
            );
            _rightPos = tempWall.transform.position.x - tempWall.Collider.size.x / 2;

            float value = topBar.rect.height / Screen.height;
            float stepValue = screenHeight * value / 2;

            tempWall = CreateWall(
                name: "BoundaryTop",
                position: new Vector2(0f, _playField.yMax - stepValue),
                size: new Vector2(screenWidth, value * screenHeight)
            );
            _upPos = tempWall.transform.position.y - tempWall.Collider.size.y / 2;

            tempWall = CreateWall(
                name: "BoundaryBottom",
                position: new Vector2(0f, _playField.yMin),
                size: new Vector2(screenWidth, boundarySize)
            );
            _downPos = tempWall.transform.position.y + tempWall.Collider.size.y / 2;
        }

        public Vector2 GetRandomPositionInsideBoundary()
        {
            return new Vector2(Random.Range(_leftPos, _rightPos), Random.Range(_downPos, _upPos));
        }

        private Wall CreateWall(string name, Vector2 position, Vector2 size)
        {
            GameObject gameObject = new GameObject();
            Wall wall = gameObject.AddComponent<Wall>();
            wall.transform.SetParent(boundariesParent);
            wall.name = name;
            wall.transform.position = position;
            wall.Init(size);
            return wall;
        }
    }
}