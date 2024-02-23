using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace AI {
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimatorManager enemyAnimatorManager;
        NavMeshAgent navMeshAgent;
        EnemyStats enemyStats;
        Rigidbody enemyRigidbody;

        //public float distanceFromTarget;
        public float rotationSpeed = 15f;
        [SerializeField] float currentRecoveryTime = 0;

        [Header ("State management")]
        public State currentState;
        public State deadState;
        public Transform currentTarget;
        public bool isPerformingAction = false;
        public bool isInterracting = false;

        [Header ("AI settings")]
        public float detectionRadius = 20f;
        public float minimumDetectionAngle = -50f;
        public float maximumDetectionAngle = 50f;
        public float maximumAggroRadius = 5f;
        //public float viewableAngle;

        void Awake() 
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponentInChildren<EnemyAnimatorManager>();
            enemyStats = GetComponent<EnemyStats>();
            enemyRigidbody = GetComponent<Rigidbody>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        }

        void Start() 
        {
            enemyRigidbody.isKinematic = false;
        }

        void FixedUpdate() 
        {
            HandleStateMachine();
        }

        private void Update() 
        {
            HandleRecoveryTime();

            isRotatingWithRootMotion = enemyAnimatorManager.anim.GetBool("isRotatingWithRootMotion");
            isInterracting = enemyAnimatorManager.anim.GetBool("isInteracting");
            canRotate = enemyAnimatorManager.anim.GetBool("canRotate");
        }

        private void HandleStateMachine()
        {
            if (currentState != null && currentState != deadState) {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimatorManager);

                if (nextState != null)
                    SwitchToNextState(nextState);
                if (enemyStats.currentHealth <= 0)
                    SwitchToNextState(deadState);
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTime()
        {
            if (currentRecoveryTime > 0)
                currentRecoveryTime -= Time.deltaTime;
            if (isPerformingAction && currentRecoveryTime <= 0)
                isPerformingAction = false;
        }

        #region Getters and Setters
        public NavMeshAgent GetNavMeshAgent()
        {
            return navMeshAgent;
        }

        public Rigidbody GetRigidbody()
        {
            return enemyRigidbody;
        }

        public float GetCurrentRecoveryTime()
        {
            return currentRecoveryTime;
        }

        public void SetCurrentRecoveryTime(float recoveryTime)
        {
            currentRecoveryTime = recoveryTime;
        }
        #endregion
    }
}
