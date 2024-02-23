using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract State Tick(
        AI.EnemyManager enemyManager,
        AI.EnemyStats enemyStats,
        AI.EnemyAnimatorManager enemyAnimatorManager);
}
