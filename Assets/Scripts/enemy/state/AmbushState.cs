using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public class AmbushState : State
    {
        public bool isSleeping;
        public float detectionLayerRadius = 2f;
        public string sleepAnimation;
        public string wakeAnimation;

        public LayerMask detectionLayer;
        public PursueState pursueState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (isSleeping && !enemyManager.isInterracting)
                enemyAnimatorManager.PlayTargetAnimation(sleepAnimation, true);

            #region Detect Player
            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionLayerRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++) {
                if (colliders[i].tag == "Player") {
                    Vector3 targetDirection = colliders[i].transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle) {
                        enemyManager.currentTarget = colliders[i].transform;
                        isSleeping = false;
                        enemyAnimatorManager.PlayTargetAnimation(wakeAnimation, true);
                    }
                }
            }
            #endregion

            #region Handle state change
            if (enemyManager.currentTarget != null)
                return pursueState;
            return this;
            #endregion
        }
    }
} // namespace AI
