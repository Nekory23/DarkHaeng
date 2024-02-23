using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class EnemyAnimatorManager : Managers.AnimatorManager
    {
        EnemyManager enemyManager;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;
            enemyManager.GetRigidbody().drag = 0;
            Vector3 deltaPosition = anim.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = Vector3.zero;

            velocity = deltaPosition / delta;
            enemyManager.GetRigidbody().velocity = velocity;

            if (enemyManager.isRotatingWithRootMotion)
            {
                enemyManager.transform.rotation *= anim.deltaRotation;
            }
        }

        public void EnableCombo()
        {
        }

        public void DisableCombo()
        {
        }
    }
}
