using Enemies;
using Simpolony.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Simpolony.Score
{
    [CreateAssetMenu(fileName = "ScoreManager", menuName = "Data/Score/new Score Manager")]
    public class ScoreManager : ManagerComponent
    {
        public int Score { get; set; }

        public override void DoAwake()
        {
            this.Score = 0;
        }

        public override void DoStart()
        {
            
        }

        public override void DoUpdate()
        {
            
        }

        public void Add(EnemyData enemyData)
        {
            this.Score += enemyData.Points;
        }
    }
}
