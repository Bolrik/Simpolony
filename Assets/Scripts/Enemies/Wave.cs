using UnityEngine;

namespace Enemies
{
    public class Wave
    {
        public int SpawnBudget { get; private set; } = 0;
        public float SpawnTime { get; private set; } = 0;

        public Wave(int spawnBudget)
        {
            this.SpawnBudget = spawnBudget;
        }

        public void SetSpawnTime(float spawnTime)
        {
            this.SpawnTime = spawnTime;
        }

        public void SetSpawnBudget(int value)
        {
            Debug.Log($"New Budget: {value}");
            this.SpawnBudget = value;
        }
    }
}