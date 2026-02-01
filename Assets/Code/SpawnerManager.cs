using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code
{
    public class SpawnerManager : MonoBehaviour
    {
        public static SpawnerManager Instance { get; private set; }
        
        [SerializeField] private List<SpawnerArea> spawners;
        
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }
        
        public SpawnerArea GetSpawnerByType(SpawnerType type) => spawners.First(spawner => spawner.SpawnerType == type);
    }
}