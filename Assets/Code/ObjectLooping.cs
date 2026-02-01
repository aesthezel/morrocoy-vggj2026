using UnityEngine;

namespace Code
{
    public class ObjectLooping : MonoBehaviour
    {
        private BoxCollider2D areaCollider;
        private Bounds areaBounds;
        
        [SerializeField]
        private SpawnerType spawnerTargetType;

        private void Start()
        {
            var spawner = SpawnerManager.Instance.GetSpawnerByType(spawnerTargetType);
            
            if (spawner != null)
            {
                areaCollider = spawner.GetComponent<BoxCollider2D>();
                areaBounds = areaCollider.bounds;
            }
            else
            {
                Debug.LogWarning("No se encontró un SpawnerArea en la escena para el looping.");
                enabled = false;
            }
        }

        private void Update()
        {
            areaBounds = areaCollider.bounds;
            CheckAndLoop();
        }

        private void CheckAndLoop()
        {
            Vector3 pos = transform.position;

            // Loop Horizontal
            if (pos.x > areaBounds.max.x)
            {
                pos.x = areaBounds.min.x;
            }
            else if (pos.x < areaBounds.min.x)
            {
                pos.x = areaBounds.max.x;
            }

            // Loop Vertical
            if (pos.y > areaBounds.max.y)
            {
                pos.y = areaBounds.min.y;
            }
            else if (pos.y < areaBounds.min.y)
            {
                pos.y = areaBounds.max.y;
            }

            transform.position = pos;
        }
    }
}