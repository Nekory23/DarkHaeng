using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public class EnemyBossManager : MonoBehaviour
    {
        public UI.BossHealthBar bossHealthBar;
        EnemyStats enemyStats;

        public string bossName;

        private void Awake() 
        {
            enemyStats = GetComponent<EnemyStats>();
        }

        private void Start() 
        {
            bossHealthBar.SetBossName(bossName);
            bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
            bossHealthBar.SetBossHealth(enemyStats.maxHealth);
            bossHealthBar.ActiveBossHealthBar();    
        }

        public void UpdateBossHealthBar(int currentHealth)
        {
            bossHealthBar.SetBossHealth(currentHealth);
        }
    }
}
