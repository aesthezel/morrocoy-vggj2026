using UnityEngine;

namespace Code
{
    public class SpawnerArea : MonoBehaviour
    {
        [Header("Configuración de Spawn")]
        [SerializeField] private GameObject[] prefabsToSpawn;
        [SerializeField] private float spawnRate = 2f;
        
        private BoxCollider2D spawnArea;
        
        [field: SerializeField]
        public SpawnerType SpawnerType { get; private set; }

        private void Awake()
        {
            spawnArea = GetComponent<BoxCollider2D>();
            spawnArea.isTrigger = true;
        }

        private void Start()
        {
            if (GameManager.Instance.CurrentState == GameState.Gameplay)
            {
                StartSpawning();
            }
            else
            {
                GameManager.Instance.OnGameStart += StartSpawning;
            }
        }
        
        private void StartSpawning()
        {
            InvokeRepeating(nameof(SpawnObject), 1f, spawnRate);
        }

        private void SpawnObject()
        {
            if (prefabsToSpawn.Length == 0) return;

            Vector2 spawnPoint = GetRandomPointInBounds();
            int randomIndex = Random.Range(0, prefabsToSpawn.Length);
            
            Instantiate(prefabsToSpawn[randomIndex], spawnPoint, Quaternion.identity);
        }

        private Vector2 GetRandomPointInBounds()
        {
            Bounds bounds = spawnArea.bounds;
            
            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);
            
            return new Vector2(x, y);
        }
        
        private void OnDrawGizmos()
        {
            if (spawnArea == null) spawnArea = GetComponent<BoxCollider2D>();
            
            Gizmos.color = new Color(0, 1, 0, 0.3f);
            Gizmos.DrawCube(spawnArea.bounds.center, spawnArea.bounds.size);
        }
    }
}