using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public class EnemyLocomotionManager : MonoBehaviour
    {
        EnemyManager enemyManager;
        EnemyAnimatorManager enemyAnimatorManager;

        void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
        }
    }
}
