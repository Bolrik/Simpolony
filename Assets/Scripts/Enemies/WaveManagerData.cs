using FreschGames.Core.Misc;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies
{
    [CreateAssetMenu(fileName = "Wave Manager Data", menuName = "Data/Enemies/new Wave Manager Data")]
    public class WaveManagerData : ScriptableObject
    {
        [field: SerializeField, Header("Settings")] public float TimeBetweenWaves { get; private set; }
        [field: SerializeField] public float TimeBetweenSpawns { get; private set; }
        [field: SerializeField] public MinMax<float> SpawnDistance { get; private set; }

        [field: SerializeField] public int BaseWaveBudget { get; private set; }
        [field: SerializeField] public int WaveBudgetIncreasePerLevel { get; private set; }

        [field: SerializeField] public EnemyWaveData[] EnemyData { get; private set; }

        [field: SerializeField, Header("Prefab")] public Enemy EnemyPrefab { get; private set; }


        public Vector3 SpawnPoint { get; set; }
        public float MinimumSpawnDistance { get; set; }


        public bool GetRandomEnemyData(int wave, int remainingCost, out EnemyWaveData enemyWaveData)
        {
            var validWaveData = this.GetValidWaveData(wave, remainingCost);

            enemyWaveData = null;

            if (validWaveData.Length == 0)
            {
                Debug.Log("No Valid Data");
                return false;
            }

            enemyWaveData = this.GetRandom(wave, validWaveData);
            return true;
        }

        private EnemyWaveData GetRandom(int wave, EnemyWaveData[] enemyWaveData)
        {
            float totalChance = 0;
            for (int i = 0; i < enemyWaveData.Length; i++)
            {
                totalChance += enemyWaveData[i].Chance + enemyWaveData[i].WaveChanceModifier * wave;
            }

            float randomChance = Random.Range(0, totalChance);

            for (int i = 0; i < enemyWaveData.Length; i++)
            {
                randomChance -= enemyWaveData[i].Chance + enemyWaveData[i].WaveChanceModifier * wave;
                if (randomChance <= 0)
                {
                    return enemyWaveData[i];
                }
            }

            return enemyWaveData[enemyWaveData.Length - 1];

            //float totalChance = 0;
            //for (int i = 0; i < enemyWaveData.Length; i++)
            //{
            //    totalChance += enemyWaveData[i].Chance;
            //}

            //float randomChance = Random.Range(0, totalChance);

            //for (int i = 0; i < enemyWaveData.Length; i++)
            //{
            //    randomChance -= enemyWaveData[i].Chance;
            //    if (randomChance <= 0)
            //    {
            //        return enemyWaveData[i];
            //    }
            //}

            //return enemyWaveData[enemyWaveData.Length - 1];
        }

        private EnemyWaveData[] GetValidWaveData(int wave, int remainingCost)
        {
            List<EnemyWaveData> toReturn = new List<EnemyWaveData>();

            foreach (var enemyWaveData in this.EnemyData)
            {
                if (wave < enemyWaveData.StartingWave)
                    continue;

                if (remainingCost < enemyWaveData.Cost)
                    continue;

                toReturn.Add(enemyWaveData);
            }

            return toReturn.ToArray();
        }
    }

    [System.Serializable]
    public class EnemyWaveData
    {
        [field: SerializeField] public EnemyData EnemyData { get; private set; }
        [field: SerializeField] public float Chance { get; private set; }
        [field: SerializeField] public float WaveChanceModifier { get; private set; }
        [field: SerializeField] public int StartingWave { get; private set; }

        [field: SerializeField] public int Cost { get; private set; }
        // The required waves to increase level
        [field: SerializeField] public int WavesPerLevel { get; private set; }
    }
}