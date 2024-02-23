using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Events {
    public class WorldEventManager : MonoBehaviour
    {
        public UI.BossHealthBar bossHealthBar;
        AI.EnemyBossManager boss;

        public bool bossFightStarted;
        public bool bossAwakened;
        public bool bossDefeated;

        public void ActivateBossFight()
        {
            bossFightStarted = true;
            bossAwakened = true;
            bossHealthBar.gameObject.SetActive(true);
            bossHealthBar.ActiveBossHealthBar();
        }

        public void BossHasBeenDefeated()
        {
            bossFightStarted = false;
            bossAwakened = false;
            bossDefeated = true;
            bossHealthBar.DeactiveBossHealthBar();
        }
    }
}
