using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI {
    public class EnemyStats : CharacterStats
    {
        Animator animator;
        EnemyBossManager bossManager;
        public EnemyUI.EnemyHealthBar healthBar;

        public bool isBoss;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            maxHealth = SetMaxHealthFromHeatlLevel();
            currentHealth = maxHealth;

            if (isBoss)
                bossManager = GetComponent<EnemyBossManager>();
        }

        void Start()
        {
            if (!isBoss)
                healthBar.SetMaxHealth(maxHealth);
        }

        private int SetMaxHealthFromHeatlLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            if (isDead)
                return;

            currentHealth -= damage;
            if (!isBoss)
                healthBar.SetHealth(currentHealth);
            else
                bossManager.UpdateBossHealthBar(currentHealth);

            animator.Play("Damage_01");
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                animator.Play("Dead_01");
                isDead = true;
                if (isBoss)
                    bossManager.bossHealthBar.DeactiveBossHealthBar();
            }
        }
    }
}
