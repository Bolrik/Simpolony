using Simpolony.Misc;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    [CreateAssetMenu(fileName = "Wave Manager", menuName = "Data/Enemies/new Wave Manager")]
    public class WaveManager : ManagerComponent
    {
        [field: SerializeField, Header("Data")] public WaveManagerData WaveData { get; private set; }

        [field: SerializeField, Header("Info")] public int EnemiesRemaining { get; private set; } = 0;

        private Wave Wave { get; set; }

        public int WaveNumber { get; private set; }
        public float WaveTimer { get; private set; } = 0;
        public float WaveTimerMax { get; private set; } = 0;

        public float TotalWaveTime { get; private set; }

        private bool Spawning { get; set; } = false;

        bool IsPaused { get; set; }


        public override void DoAwake()
        {
            this.Wave = null;

            this.WaveNumber = 0;
            this.WaveTimer = 0;

            this.TotalWaveTime = 0;

            this.Spawning = false;

            this.EnemiesRemaining = 0;
            this.IsPaused = false;
        }

        public override void DoStart()
        {

        }

        // public void UpdateManager(Vector3 spawnPoint, float baseSpawnDistance)
        public override void DoUpdate()
        {
            if (this.IsPaused)
                return;

            this.TotalWaveTime += Time.deltaTime;

            if (this.Spawning)
            {
                if (this.Wave.SpawnBudget <= 0)
                {
                    this.Spawning = false;
                    return;
                }

                if (this.Wave.SpawnTime <= Time.time)
                {
                    if (this.SpawnEnemy())
                    {
                        this.Wave.SetSpawnTime(Time.time + this.WaveData.TimeBetweenSpawns);
                        this.EnemiesRemaining++;
                    }
                    else
                    {
                        this.Spawning = false;
                        return;
                    }
                }
            }
            else
            {
                bool extraTime = this.EnemiesRemaining > 0;

                this.WaveTimer += Time.deltaTime * (extraTime ? .5f : 1);
                this.WaveTimerMax = this.WaveData.TimeBetweenWaves;

                if (this.WaveTimer >= this.WaveTimerMax)
                {
                    this.StartWave();
                    this.WaveTimer = 0;
                }
            }
        }

        private void StartWave()
        {
            this.WaveNumber++;

            int budget = this.WaveData.BaseWaveBudget + this.WaveNumber * this.WaveData.WaveBudgetIncreasePerLevel + this.WaveNumber;
            Debug.Log($"New Wave {this.WaveNumber} -- $ {budget}");


            this.Wave = new Wave(budget);
            // this.EnemiesRemaining = 0;

            this.Spawning = true;
        }

        private bool SpawnEnemy()
        {
            if (this.WaveData.GetRandomEnemyData(this.WaveNumber, this.Wave.SpawnBudget, out EnemyWaveData enemyWaveData))
            {
                Vector3 spawnPoint = this.WaveData.SpawnPoint;
                float baseSpawnDistance = this.WaveData.MinimumSpawnDistance;

                // float angle = Random.Range(0, 360);
                float angle = Time.time * 0.2f * Mathf.PI;

                float distance = baseSpawnDistance + Random.Range(this.WaveData.SpawnDistance.Min, this.WaveData.SpawnDistance.Max);
                Vector3 spawnPos = spawnPoint + new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * distance;

                int realWave = (this.WaveNumber - enemyWaveData.StartingWave);
                int level = enemyWaveData.WavesPerLevel > 0 ? (realWave / enemyWaveData.WavesPerLevel) : 0;
                
                Debug.Log("Level:" + level);

                Enemy enemy = Instantiate(this.WaveData.EnemyPrefab, spawnPos, Quaternion.identity);
                enemy.SetData(enemyWaveData.EnemyData, level);

                enemy.Health.OnDestroyed += this.Enemy_OnEnemyDestroyed;

                this.Wave.SetSpawnBudget(this.Wave.SpawnBudget - enemyWaveData.Cost);
                return true;
            }

            return false;
        }

        private void Enemy_OnEnemyDestroyed(Enemy enemy)
        {
            this.EnemiesRemaining--;
        }


        public void Pause()
        {
            this.IsPaused = true;
        }

        public void Resume()
        {
            this.IsPaused = false;
        }
    }
}