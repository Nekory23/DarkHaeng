using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public class IdleState : State
    {
        [Header ("State management")]
        public LayerMask detectionLayer;
        public PursueState pursueState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            #region Handle Enemy Detection
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Player")
                {
                    Vector3 targetDirection = colliders[i].transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle
                            && viewableAngle < enemyManager.maximumDetectionAngle)
                        enemyManager.currentTarget = colliders[i].transform;
                }
            }
            #endregion

            #region Handle Switch state
            if (enemyManager.currentTarget != null)
                return pursueState;
            return this;
            #endregion
        }
    }
}
